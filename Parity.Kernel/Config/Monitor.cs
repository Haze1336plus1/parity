using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Kernel.Config
{
    public class Monitor
    {

        public System.IO.FileSystemWatcher FileWatcher { get; private set; }
        public string FileName { get; private set; }

        public Monitor(string fileName)
        {
            System.IO.FileInfo ffileInfo = new System.IO.FileInfo(fileName);
            this.FileName = ffileInfo.FullName;
            this.FileWatcher = new System.IO.FileSystemWatcher(ffileInfo.Directory.FullName);
            this.FileWatcher.Changed += FileWatcher_Changed;
            this.FileWatcher.EnableRaisingEvents = true;
        }

        private void FileWatcher_Changed(object sender, System.IO.FileSystemEventArgs e)
        {
            try
            {
                this.FileWatcher.EnableRaisingEvents = false;
                if (e.ChangeType == System.IO.WatcherChangeTypes.Changed && 
                    e.FullPath == this.FileName)
                {
                    System.Threading.Thread.Sleep(5); // sleep to gain file access caught by filewatcher?
                    this.OnFileChanged(this, new Base.Event.ConfigMonitorFileChangedEventArgs(this.FileName));
                }
            }
            finally
            {
                this.FileWatcher.EnableRaisingEvents = true;
            }
        }

        public event Base.Event.ConfigMonitorFileChangedDelegate OnFileChanged;

    }
}
