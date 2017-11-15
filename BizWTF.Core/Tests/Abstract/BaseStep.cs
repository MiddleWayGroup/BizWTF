using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;

using BizWTF.Core.Utilities;

namespace BizWTF.Core.Tests
{
    public enum StepTypes
    {
        Action,
        Get,
        Test
    }

    public enum StepResult
    {
        OK,
        Error,
        Working
    }

    public abstract class BaseStep
    {
        protected string _stepName;
        protected string _testID;
        protected StepResult _result;
        protected StringBuilder _resultDescription = new StringBuilder();
        protected Dictionary<string, object> _properties;
        protected Dictionary<string, string> _params;

        public string StepName
        {
            get { return this._stepName; }
            set { this._stepName = value; }
        }

        public string TestID
        {
            get { return this._testID; }
            set { this._testID = value; }
        }

        public StepResult Result
        {
            get { return this._result; }
            set { this._result = value; }
        }

        public string ResultDescription
        {
            get { return this._resultDescription.ToString(); }
        }

        public abstract StepTypes StepType { get; }



        public BaseStep()
        {}

        public BaseStep(string stepName)
        {
            this._stepName = stepName;

            this.Result = StepResult.Working;
            this.AppendResultDescription(0, "{0} - Creating step [{1}]", this.StepType, this.StepName);
            
        }

        public abstract bool ExecuteStep();

        public delegate void AppendResultDescriptionHandler(int indentLevel, string pattern, params object[] args);
        protected void AppendResultDescription(int indentLevel, string pattern, params object[] args)
        {
            //pattern = string.Format("[{0}] {1}", severity, pattern);
            for (int i = 0; i < indentLevel; i++)
                pattern = "  " + pattern;

            this._resultDescription.AppendLine(String.Format(pattern, args));
        }

    }


    public class BaseStepSerializer : IXmlSerializable
    {

        #region Constructors
        public BaseStepSerializer() { }

        public BaseStepSerializer(BaseStep step)
        {
            this._step = step;
        }
        #endregion Constructors

        #region Properties
        public BaseStep Step
        {
            get { return _step; }
        } 
        BaseStep _step;
        #endregion Properties

        #region IXmlSerializable Implementation
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            Type type = Type.GetType(reader.GetAttribute("type"));
            reader.ReadStartElement();
            this._step = (BaseStep)new
                          XmlSerializer(type).Deserialize(reader);
            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("type", this._step.GetType().ToString());
            new XmlSerializer(this._step.GetType()).Serialize(writer, this._step);
        }
        #endregion IXmlSerializable Implementation

    }
}
