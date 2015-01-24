using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.WRDevTool.FileDB
{
    public class fdbEntryDiff
    {

        public fdbEntry Original { get; private set; }
        public fdbEntry Difference { get; private set; }
        public string Action { get; private set; }

        public fdbEntryDiff(fdbEntry Original, fdbEntry Difference, string Action)
        {
            this.Original = Original;
            this.Difference = Difference;
            this.Action = Action;
        }

    }
}
