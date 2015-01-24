using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Base.Exception
{
    public class CustomException : System.Exception
    {

        public CustomFlags Flags { get; private set; }
        public void Handle()
        {
            if (this.Flags.HasFlag(CustomFlags.Critical))
                throw new System.Exception("Critical CustomException. See InnerException", this);
        }
        public CustomException(CustomFlags flags)
			: base()
        {
            this.Flags = flags;
        }

    }
}
