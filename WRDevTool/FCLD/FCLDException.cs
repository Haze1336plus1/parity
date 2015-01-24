using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.WRDevTool.FCLD
{
    public class FCLDException
    {

        public enum eType
        {

            Equals,
            StartsWith,
            EndsWith

        }

        public eType Type { get; private set; }
        public string Value { get; private set; }

        public FCLDException(eType Type, string Value)
        {
            this.Type = Type;
            this.Value = Value.ToLower();
        }

    }
}
