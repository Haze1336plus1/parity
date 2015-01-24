using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.GameDetails.Item
{
    public class Basic
    {

        public string Code { get; private set; }

        public Basic(string code)
        {
            this.Code = code;
        }

    }
}
