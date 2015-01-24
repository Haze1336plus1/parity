using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Base.Layout
{
    public class LogChannel
    {

        public string Name { get; private set; }
        public string Format { get; set; }
        internal System.IO.StreamWriter Stream;

        public LogChannel(string name, System.IO.StreamWriter stream)
        {
            this.Name = name;
            this.Stream = stream;
            this.Format = "{0} [{1} @ {4}] :: {2}\r\n{3}\r\n\r\n";
        }

        public void Write(string Message, string Title = null)
        {
            System.Diagnostics.StackFrame SF = new System.Diagnostics.StackTrace().GetFrame((int)IO.Configuration["StackDeepness"] - 1);
            string reflection = SF.GetMethod().ReflectedType.Name + "." + SF.GetMethod().Name.Replace(".ctor", "/new");
            this.Stream.Write(this.Format, DateTime.Now.ToString(), this.Name, Title ?? "-", Message, reflection);
            this.Stream.Flush();
        }

    }
}
