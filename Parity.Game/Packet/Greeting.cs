using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Game.Packet
{
    public class Greeting : Net.Packet.OutPacket
    {

        public Greeting()
            : base(Net.PacketCodes.SERIAL_GSERV)
        {
            base.Add(1);
            DateTime now = DateTime.Now.AddYears(-1900).AddMonths(-1);
            string dateString = now.ToString("s/m/H/d/M/yyy/", System.Globalization.CultureInfo.CreateSpecificCulture("en-US")) + ((int)DateTime.Now.DayOfWeek).ToString() + "/" + (DateTime.Now.DayOfYear - 1).ToString() + "/1";
            base.Add(dateString);
        }
    }
}
