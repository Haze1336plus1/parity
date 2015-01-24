using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Kernel.Config
{
    public class cRate
    {

        public double Experience { get; private set; }
        public double Dinar { get; private set; }
        public double Damage { get; private set; }

        public cRate(System.Xml.XmlNode configNode)
        {
            System.Globalization.CultureInfo culture = (System.Globalization.CultureInfo)Base.IO.Configuration["Culture"];
            this.Experience = double.Parse(configNode["Game"]["Experience"].InnerText, culture);
            this.Dinar = double.Parse(configNode["Game"]["Dinar"].InnerText, culture);
            this.Damage = double.Parse(configNode["Weapon"]["Damage"].InnerText, culture);
        }

    }
}
