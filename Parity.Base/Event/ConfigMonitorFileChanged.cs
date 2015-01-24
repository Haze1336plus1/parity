using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Base.Event
{

    public class ConfigMonitorFileChangedEventArgs : EventArgs
    {

        public string FileName { get; private set; }

        public ConfigMonitorFileChangedEventArgs(string fileName)
             : base()
        {
            this.FileName = fileName;
        }

    }

    public delegate void ConfigMonitorFileChangedDelegate(object sender, ConfigMonitorFileChangedEventArgs e);
}
