using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Base.Layout
{
    public class EventDetails
    {

        public readonly int EventDuration;
        public readonly DateTime EventStart;
        public int EventRemaining
        {
            get
            {
                DateTime aTime = new DateTime(EventStart.Ticks);
                aTime.AddSeconds(this.EventDuration);
                return (int)Math.Ceiling(aTime.Subtract(DateTime.Now).TotalSeconds);
            }
        }

    }
}
