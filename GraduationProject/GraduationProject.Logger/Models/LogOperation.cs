using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.LogHandler.Models
{
    public class LogOperation
    {
        private LogOperation(string value) { Value = value; }

        public string Value { get; private set; }

        public static LogOperation Trace { get { return new LogOperation("Trace"); } }
        public static LogOperation Debug { get { return new LogOperation("Debug"); } }
        public static LogOperation Info { get { return new LogOperation("Info"); } }
        public static LogOperation Warning { get { return new LogOperation("Warning"); } }
        public static LogOperation Error { get { return new LogOperation("Error"); } }
        public static LogOperation Insert { get { return new LogOperation("Insert"); } }
        public static LogOperation Update { get { return new LogOperation("Update"); } }
        public static LogOperation Delete { get { return new LogOperation("Delete"); } }

        public override string ToString()
        {
            return Value;
        }
    }
}
