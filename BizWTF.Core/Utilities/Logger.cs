using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

namespace BizWTF.Core.Utilities
{
    public class Logger
    {
        public delegate void WriteAction(int indentLevel, string pattern, EventLogEntryType severity, params object[] args);
        public WriteAction Write;
        
        public delegate void InsertBlankAction();
        public InsertBlankAction InsertBlank;

        public delegate void InsertSeparatorAction();
        public InsertSeparatorAction InsertSeparator;

        private void TraceWrite(int indentLevel, string pattern, EventLogEntryType severity, params object[] args)
        {
            for (int i = 0; i < indentLevel; i++)
                pattern = "  " + pattern;

            switch (severity)
            {
                case EventLogEntryType.Information:
                    Trace.TraceInformation(pattern, args);
                    break;
                case EventLogEntryType.Warning:
                    Trace.TraceWarning(pattern, args);
                    break;
                case EventLogEntryType.Error:
                    Trace.TraceError(pattern, args);
                    break;
                default:
                    break;
            }
        }

        private void DebugWrite(int indentLevel, string pattern, EventLogEntryType severity, params object[] args)
        {
            //pattern = string.Format("[{0}] {1}", severity, pattern);
            for (int i = 0; i < indentLevel; i++)
                pattern = "  " + pattern;

            Debug.WriteLine(String.Format(pattern, args), severity.ToString());
        }


        private void TraceBlank()
        {
            this.TraceWrite(0, "", System.Diagnostics.EventLogEntryType.Information);
        }
        private void DebugBlank()
        {
            Debug.WriteLine(string.Empty);
        }

        private void TraceSeparator()
        {
            this.TraceWrite(0, "----------------------------------------------------", System.Diagnostics.EventLogEntryType.Information);
        }
        private void DebugSeparator()
        {
            Debug.WriteLine("----------------------------------------------------");
        }

        public Logger()
        {
            TextWriterTraceListener tr1 = new TextWriterTraceListener(System.Console.Out);
            Debug.Listeners.Add(tr1);

            this.Write += new WriteAction(this.DebugWrite);
            this.InsertBlank += new InsertBlankAction(this.DebugBlank);
            this.InsertSeparator += new InsertSeparatorAction(this.DebugSeparator);
        }

        public Logger(WriteAction write, InsertBlankAction insertBlank, InsertSeparatorAction insertSeparator)
        {
            this.Write += new WriteAction(write);
            this.InsertBlank += new InsertBlankAction(insertBlank);
            this.InsertSeparator += new InsertSeparatorAction(insertSeparator);
        }

        private static Logger _logger;
        public static Logger CurrentLogger
        {
            get
            {
                if (_logger == null)
                        _logger = new Logger();
                lock (_logger)
                {   
                    return _logger;
                }
            }
            set 
            { 
                lock(_logger)
                    _logger = value; 
            }
        }
        
        //public static void Log(int indentLevel, string pattern, EventLogEntryType severity, params object[] args)
        //{
        //    _logger.Write(indentLevel, pattern, severity, args);
        //}
    }
}
