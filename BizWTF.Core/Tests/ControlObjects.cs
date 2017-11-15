using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWT.Testing.Library.Tests
{
    public class ControlProperty
    {
        protected string _namespace;
        protected string _property;
        protected string _value;
        public string TargetContextProperty;

        public string Namespace
        {
            get
            {
                if (!String.IsNullOrEmpty(this.TargetContextProperty))
                    return ((ControlProperty)BTSTestContext.GetParam(this.TargetContextProperty)).Namespace;
                else
                    return this._namespace;
            }
            set
            { this._namespace = value; }
        }
        public string Property
        {
            get
            {
                if (!String.IsNullOrEmpty(this.TargetContextProperty))
                    return ((ControlProperty)BTSTestContext.GetParam(this.TargetContextProperty)).Property;
                else
                    return this._property;
            }
            set
            { this._property = value; }
        }
        public string Value
        {
            get
            {
                if (!String.IsNullOrEmpty(this.TargetContextProperty))
                    return ((ControlProperty)BTSTestContext.GetParam(this.TargetContextProperty)).Value;
                else
                    return this._value;
            }
            set
            { this._value = value; }
        }

        public ControlProperty() { }

        public ControlProperty(string nameSpace, string propName, string val)
        {
            this.Namespace = nameSpace;
            this.Property = propName;
            this.Value = val;
        }
    }

    public class ControlField
    {
        public string XPath;
        public string Value;

        public ControlField() { }

        public ControlField(string xpath, string val)
        {
            this.XPath = xpath;
            this.Value = val;
        }
    }
}
