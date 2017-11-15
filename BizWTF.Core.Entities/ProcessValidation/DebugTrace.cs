using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace BizWTF.Core.Entities.ProcessValidation
{
    [Serializable]
    public class DebugTrace
    {
        protected string _processName;
        protected DateTime _startTime;
        protected DateTime _finishTime;
        protected List<DebugShape> _traceDetails;

        public string ProcessName
        {
            get { return this._processName; }
            set { this._processName = value; }
        }
        public DateTime startTime
        {
            get { return this._startTime; }
            set { this._startTime = value; }
        }
        public DateTime finishTime
        {
            get { return this._finishTime; }
            set { this._finishTime = value; }
        }
        public List<DebugShape> TraceDetails
        {
            get
            {
                if (this._traceDetails == null)
                    this._traceDetails = new List<DebugShape>();
                return this._traceDetails;
            }
            set { this._traceDetails = value; }
        }


        public bool FindDebugShape(Guid shapeID, int repeatCount, bool completed)
        {
            bool result = true;

            if (repeatCount == 0)
            {
                if (this.TraceDetails.Where<DebugShape>(shape => shape.ShapeID == shapeID).Count() > 0)
                    result = false;
            }
            else
            {
                var res = this.TraceDetails.Where<DebugShape>(shape => shape.ShapeID == shapeID && shape.FinishTime.HasValue == completed);
                if (res.Count() != repeatCount)
                    result = false;
            }

            return result;
        }


        public void Export(string uri)
        {
            XmlSerializer traceSer = new XmlSerializer(typeof(DebugTrace));
            XmlTextWriter wr = new XmlTextWriter(uri, Encoding.Unicode);

            traceSer.Serialize(wr, this);
        }



        public static DebugTrace ImportTimeLine(string uri)
        {
            XmlSerializer traceSer = new XmlSerializer(typeof(DebugTrace));
            XmlTextReader wr = new XmlTextReader(uri);

            return (DebugTrace)traceSer.Deserialize(wr);
        }

        public static DebugTrace ImportTimeLine(string assemblyName, string resourceName)
        {
            Assembly assembly = Assembly.Load(assemblyName);
            return ImportTimeLine(assembly, resourceName);
        }

        public static DebugTrace ImportTimeLine(Assembly assembly, string resourceName)
        {
            XmlSerializer traceSer = new XmlSerializer(typeof(DebugTrace));
            XmlTextReader wr = new XmlTextReader(assembly.GetManifestResourceStream(resourceName));

            return (DebugTrace)traceSer.Deserialize(wr);
        }
    }


    [Serializable]
    public class DebugShape : ShapeInfo
    {
        protected string _shapePath;
        protected DateTime? _startTime;
        protected DateTime? _finishTime;
        protected int _repeatCount = 1;
        protected bool _completed;
        

        public string ShapePath
        {
            get { return this._shapePath; }
            set { this._shapePath = value; }
        }
        public DateTime? StartTime
        {
            get { return this._startTime; }
            set { this._startTime = value; }
        }
        public DateTime? FinishTime
        {
            get { return this._finishTime; }
            set { this._finishTime = value; }
        }
        public int RepeatCount
        {
            get { return this._repeatCount; }
            set { this._repeatCount = value; }
        }
        public bool Completed
        {
            get { return this._completed; }
            set { this._completed = value; }
        }


        public DebugShape():base()
        {}

        
    }
}
