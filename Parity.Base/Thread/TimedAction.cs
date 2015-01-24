using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Base.Thread
{
    public class TimedAction
    {

        public long At { get; private set; }
        public Action Callback { get; private set; }

        public TimedAction(long At, Action Callback)
        {
            this.At = At;
            this.Callback = Callback;
        }

    }
}
