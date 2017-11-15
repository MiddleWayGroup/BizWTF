using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using System.Xml;

using Microsoft.XLANGs.BaseTypes;

namespace BizWTF.Mocking.PipelineComponents
{
    internal class CustomXLANGMessage : XLANGMessage
    {
        List<CustomXLANGPart> parts = new List<CustomXLANGPart>();


        public CustomXLANGMessage()
        {
            parts.Add(new CustomXLANGPart());
        }



        public override void AddPart(object part, string partName)
        {
            throw new NotImplementedException();
        }



        public override void AddPart(XLANGPart part, string partName)
        {
            throw new NotImplementedException();
        }



        public override void AddPart(XLANGPart part)
        {
            throw new NotImplementedException();
        }



        public override int Count
        {
            get { return parts.Count; }
        }



        public override void Dispose()
        {
        }



        public override IEnumerator GetEnumerator()
        {
            return parts.GetEnumerator();
        }



        public override object GetPropertyValue(Type propType)
        {
            throw new NotImplementedException();
        }



        public override string Name
        {
            get { return "CustomXLANGMessage"; }
        }



        public override void SetPropertyValue(Type propType, object value)
        {
            throw new NotImplementedException();
        }



        public override XLANGPart this[int partIndex]
        {
            get { return parts[0]; }
        }



        public override XLANGPart this[string partName]
        {
            get { return parts.FirstOrDefault<MyPart>(mp => mp.Name.Equals(partName)); }
        }

    }



    class CustomXLANGPart : XLANGPart
    {
        public override void Dispose()
        {
        }

        public override object GetPartProperty(Type propType)
        {
            throw new NotImplementedException();
        }

        public override Type GetPartType()
        {
            throw new NotImplementedException();
        }

        public override string GetXPathValue(string xpath)
        {
            throw new NotImplementedException();
        }

        public override void LoadFrom(object source)
        {
            throw new NotImplementedException();
        }

        public override string Name
        {
            get { return "CustomXLANGPart"; }
        }

        public override void PrefetchXPathValue(string xpath)
        {
            throw new NotImplementedException();
        }

        public override object RetrieveAs(Type t)
        {
            
            throw new NotImplementedException();

        }



        public override void SetPartProperty(Type propType, object value)
        {
            throw new NotImplementedException();
        }



        public override System.Xml.Schema.XmlSchema XmlSchema
        {
            get { throw new NotImplementedException(); }
        }



        public override System.Xml.Schema.XmlSchemaCollection XmlSchemaCollection
        {
            get { throw new NotImplementedException(); }
        }

    }



}
