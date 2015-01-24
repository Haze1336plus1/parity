using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Base
{
    public class Log : Layout.LogChannel
    {

        protected Dictionary<string, Layout.LogChannel> _Channels;
        public Layout.LogChannel[] Channels { get { return this._Channels.Values.ToArray(); } }
        public string FileName { get; private set; }
        public string BaseName { get; private set; }

        public Log(string baseName)
            : base("Default", null)
        {
            this.BaseName = baseName;
            string fileName = System.IO.Path.Combine(Compile.LogDirectory, baseName + ".log");
            this.FileName = fileName;
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(fileName);
            if (!fileInfo.Directory.Exists)
                fileInfo.Directory.Create();
            bool exists = fileInfo.Exists;
            base.Stream = new System.IO.StreamWriter(System.IO.File.Open(fileName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write, System.IO.FileShare.Read));
            if (!exists)
                base.Write("Created a new Log");
            else
            {
                this.Stream.BaseStream.Seek(0, System.IO.SeekOrigin.End);
                base.Write("Opened existing Log (new Session)");
            }
            this._Channels = new Dictionary<string, Layout.LogChannel>();
        }

        public Layout.LogChannel this[int index] { get { return this._Channels.Values.ElementAt(index); } }
        public Layout.LogChannel this[string key] { get { return this._Channels[key]; } }

        public void AddChannel(string name)
        {
            this._Channels.Add(name, new Layout.LogChannel(name, base.Stream));
        }

        //public System.IO.StreamWriter 

    }
}
