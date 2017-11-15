using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;

using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.PipelineOM;


namespace BizWTF.Mocking.PipelineComponents
{
    public class ExecutableRcvPipeline
    {
        private List<CustomStage> _stages;
        public List<CustomStage> Stages
        {
            get
            {
                if (this._stages == null)
                    this._stages = new List<CustomStage>();
                return this._stages;
            }
        }

        private List<IBaseMessage> _outputMsgs;
        public List<IBaseMessage> OutputMsgs
        {
            get
            {
                if (this._outputMsgs == null)
                    this._outputMsgs = new List<IBaseMessage>();
                return this._outputMsgs;
            }
            set
            {
                this._outputMsgs = value;
            }
        }



        public ExecutableRcvPipeline()
        {
        }

        public ExecutableRcvPipeline(ReceivePipeline source)
        {
            XmlDocument conf = new XmlDocument();
            conf.LoadXml(source.XmlContent);

            foreach (XmlNode stageConf in conf.SelectNodes("/*[local-name()='Document']/*[local-name()='Stages']/*[local-name()='Stage']"))
            {
                XmlNode fileStageConf = stageConf.SelectSingleNode("*[local-name()='PolicyFileStage']");
                ExecutionMode mode = ExecutionMode.allRecognized;
                switch (fileStageConf.Attributes["execMethod"].Value.ToLower())
                {
                    case "all":
                        mode = ExecutionMode.all;
                        break;
                    case "firstmatch":
                        mode = ExecutionMode.firstRecognized;
                        break;
                    default:
                        mode = ExecutionMode.allRecognized;
                        break;
                }


                CustomStage tempStage = new CustomStage
                                            {
                                                StageID = Guid.Parse(fileStageConf.Attributes["stageId"].Value),
                                                StageLevel = Int32.Parse(fileStageConf.Attributes["_locID"].Value),
                                                Mode = mode
                                            };
                this.Stages.Add(tempStage);


                foreach (XmlNode componentConf in stageConf.SelectNodes("*[local-name()='Components']/*[local-name()='Component']"))
                {
                    IBaseComponent realComponent = (IBaseComponent)Activator.CreateInstance(Type.GetType(componentConf.SelectSingleNode("*[local-name()='Name']").InnerText));
                    MethodInfo initNewInfo = realComponent.GetType().GetMethod("InitNew");
                    if (initNewInfo != null)
                    {
                        initNewInfo.Invoke(realComponent, new object[] { });
                    }

                    IPropertyBag componentProps = new CustomPropertyBag();

                    List<string> hiddenProps = new List<string>();
                    XmlNode hiddenNode = componentConf.SelectSingleNode("*[local-name()='Properties']/*[local-name()='Property' and @Name='HiddenProperties']");
                    if (hiddenNode != null)
                        hiddenProps.AddRange(hiddenNode.InnerText.Split(','));
                    hiddenProps.Add("HiddenProperties");

                    foreach (XmlNode propConf in componentConf.SelectNodes("*[local-name()='Properties']/*[local-name()='Property']"))
                    {
                        if (!String.IsNullOrEmpty(propConf.SelectSingleNode("*[local-name()='Value']").InnerText) && !hiddenProps.Contains(propConf.Attributes["Name"].Value) )
                        {
                            componentProps.Write(
                                                propConf.Attributes["Name"].Value,
                                                this.TryConvertToSchemaList(propConf.SelectSingleNode("*[local-name()='Value']").InnerText,
                                                                            propConf.SelectSingleNode("*[local-name()='Value']").Attributes["xsi:type"].Value));
                            //switch (propConf.Attributes["Name"].Value)
                            //{
                            //    case "Microsoft.BizTalk.Component.Utilities.SchemaList":
                            //        componentProps.Write(
                            //                            propConf.Attributes["Name"].Value, 
                            //                            this.ConvertToSchemaList(propConf.SelectSingleNode("*[local-name()='Value']").InnerText));
                            //        break;
                            //    default:
                            //        componentProps.Write(
                            //                            propConf.Attributes["Name"].Value, 
                            //                            Convert.ChangeType(propConf.SelectSingleNode("*[local-name()='Value']").InnerText, propertyInfo.PropertyType));
                            //        break;
                            //}
                        }

                    }

                    if (realComponent.GetType().GetInterfaces().Contains(typeof(IPersistPropertyBag)))
                    {
                        MethodInfo loadInfo = realComponent.GetType().GetMethod("Load");
                        if (loadInfo != null)
                        {
                            loadInfo.Invoke(realComponent, new object[] { componentProps, null });
                        }
                    }

                    tempStage.Components.Add(realComponent);
                }
            }
        }

        public void Run(IPipelineContext context, IBaseMessage msgIn)
        {
            bool hasComponents = false;

            foreach (CustomStage stage in this.Stages)
            {
                foreach (IBaseComponent component in stage.Components)
                {
                    hasComponents = true;

                    if (stage.Mode == ExecutionMode.firstRecognized)
                    {
                        MethodInfo probeInfo = component.GetType().GetMethod("Probe");
                        if (probeInfo != null && (bool)probeInfo.Invoke(component, new object[] { context, msgIn }))
                        {
                            this.executeComponent(stage, component, context, msgIn);
                            break;
                        }
                    }
                    else
                        this.executeComponent(stage, component, context, msgIn);
                }
            }

            if (!hasComponents)
            {
                this.OutputMsgs.Add(msgIn);
            }
        }

        private void executeComponent(CustomStage stage, IBaseComponent component, IPipelineContext context, IBaseMessage msgIn)
        {
            msgIn.BodyPart.Data.Position = 0;

            MethodInfo probeInfo = null;
            switch (stage.StageLevel)
            {
                case 1:
                    probeInfo = component.GetType().GetMethod("Execute");
                    IBaseMessage stage1Msg = (IBaseMessage)probeInfo.Invoke(component, new object[] { context, msgIn });

                    msgIn = stage1Msg;
                    break;
                case 2:
                    probeInfo = component.GetType().GetMethod("Disassemble");
                    probeInfo.Invoke(component, new object[] { context, msgIn });
                    //msgIn.BodyPart.Data.Position = 0;
                    
                    MethodInfo nextInfo = component.GetType().GetMethod("GetNext");
                    object[] compArgs = new object[] { context };
                    IBaseMessage disassembledMsg = (IBaseMessage)nextInfo.Invoke(component, compArgs);

                    //this.OutputMsgs.Add(disassembledMsg);
                    Guid lastID = Guid.Empty;
                    while (disassembledMsg != null && disassembledMsg.MessageID != lastID)
                    {
                        MemoryStream tempStream = new MemoryStream();
                        disassembledMsg.BodyPart.Data.CopyTo(tempStream);
                        //BizWTF.Core.Entities.MultipartMessage msg = BizWTF.Core.Entities.Mocking.MultipartMessageManager.GenerateFromMessage(disassembledMsg);
                        tempStream.Position = 0;
                        disassembledMsg.BodyPart.Data = tempStream;

                        this.OutputMsgs.Add(disassembledMsg);
                        lastID = disassembledMsg.MessageID;

                        disassembledMsg = (IBaseMessage)nextInfo.Invoke(component, compArgs);
                    }
                    break;
                case 3:
                    probeInfo = component.GetType().GetMethod("Execute");

                    List<IBaseMessage> stage3Msgs = new List<IBaseMessage>();
                    foreach (IBaseMessage previousStageMsg in this.OutputMsgs)
                    {
                        stage3Msgs.Add((IBaseMessage)probeInfo.Invoke(component, new object[] { context, previousStageMsg }));
                    }
                    this.OutputMsgs = stage3Msgs;

                    break;
                case 4:
                    probeInfo = component.GetType().GetMethod("Execute");

                    List<IBaseMessage> stage4Msgs = new List<IBaseMessage>();
                    foreach (IBaseMessage previousStageMsg in this.OutputMsgs)
                    {
                        stage4Msgs.Add((IBaseMessage)probeInfo.Invoke(component, new object[] { context, previousStageMsg }));
                    }
                    this.OutputMsgs = stage4Msgs;

                    break;
                default:
                    break;
            }
        }




        public object TryConvertToSchemaList(string s, string xsiType)
        {
            string declaredType = xsiType.Replace("xsd:", "");
            Type convType = null;
            switch (declaredType)
            {
                case "boolean":
                    convType = typeof(bool);
                    break;
                case "date":
                case "dateTime":
                    convType = typeof(DateTime);
                    break;
                case "integer":
                    convType = typeof(int);
                    break;
                default:
                    //convType = Type.GetType(declaredType);
                    convType = typeof(string);
                    break;
            }
            return Convert.ChangeType(s, convType);

            #region Obsolete
            //try
            //{
            //    Microsoft.BizTalk.Component.Utilities.SchemaList sl = new Microsoft.BizTalk.Component.Utilities.SchemaList();

            //    string[] schemaList = s.Split(new char[] { '|' });
            //    foreach (string schema in schemaList)
            //    {
            //        Microsoft.BizTalk.Component.Utilities.Schema tmpSchema = this.ConvertToSchema(schema.Trim());
            //        if (!String.IsNullOrEmpty(tmpSchema.AssemblyName))
            //            sl.Add(tmpSchema);
            //        else
            //            throw new Exception("Conversion exception");
            //    }
            //    return sl;
            //}
            //catch
            //{
            //    string declaredType = xsiType.Replace("xsd:", "");
            //    Type convType = null;
            //    switch (declaredType)
            //    {
            //        case "boolean":
            //            convType = typeof(bool);
            //            break;
            //        case "date":
            //        case "dateTime":
            //            convType = typeof(DateTime);
            //            break;
            //        case "integer":
            //            convType = typeof(int);
            //            break;
            //        default :
            //            convType = Type.GetType(declaredType);
            //            break;
            //    }
            //    return Convert.ChangeType(s, convType);
            //}
            #endregion
        }

        private Microsoft.BizTalk.Component.Utilities.Schema ConvertToSchema(string s)
        {
            return new Microsoft.BizTalk.Component.Utilities.Schema(s);
        }
    }

    public class CustomStage
    {
        public Guid StageID { get; set; }
        public int StageLevel { get; set; }
        public ExecutionMode Mode { get; set; }

        private List<IBaseComponent> _components;
        public List<IBaseComponent> Components
        {
            get
            {
                if (this._components == null)
                    this._components = new List<IBaseComponent>();
                return this._components;
            }
        }
    }


    public class CustomPropertyBag : IPropertyBag
    {
        protected Dictionary<string, object> _internalBag;
        protected Dictionary<string, object> InternalBag
        {
            get
            {
                if (this._internalBag == null)
                    this._internalBag = new Dictionary<string, object>();
                return this._internalBag;
            }
        }

        public CustomPropertyBag()
        { }

        void IPropertyBag.Read(string propName, out object ptrVar, int errorLog)
        {
            if (this.InternalBag.ContainsKey(propName))
                ptrVar = this.InternalBag[propName];
            else
                ptrVar = null;
        }

        void IPropertyBag.Write(string propName, ref object ptrVar)
        {
            if (this.InternalBag.ContainsKey(propName))
                this.InternalBag[propName] = ptrVar;
            else
                this.InternalBag.Add(propName, ptrVar);
        }
    }
}
