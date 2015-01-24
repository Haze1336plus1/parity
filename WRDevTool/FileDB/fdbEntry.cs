using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.WRDevTool.FileDB
{
    public class fdbEntry
    {

        public string FileName { get; private set; }

        public string LastChanged { get; private set; }
        public string CreatedAt { get; private set; }
        public string Checksum { get; private set; }

        public fdbEntry(string FileName, string LastChanged, string CreatedAt, string Checksum)
        {
            this.FileName = FileName;
            this.LastChanged = LastChanged;
            this.CreatedAt = CreatedAt;
            this.Checksum = Checksum;
        }

    }
}
