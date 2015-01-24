using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.GameDetails.WRBin.Items.Overall
{
    public struct TargetInfo
    {

        public readonly float[] Personal;
        public readonly float[] Surface;
        public readonly float[] Air;
        public readonly float[] Ship;

        private static float[] ProcessValue(string input)
        {
            float[] outValue = new float[3];
            string[] inputSplit = input.Split(',');
            for (int i = 0; i < 3; i++)
                outValue[i] = float.Parse(inputSplit[i].Fallback("0.0"), System.Globalization.CultureInfo.InvariantCulture) / 100.0f;
            return outValue;
        }

        public TargetInfo(System.Xml.XmlNode targetInfoNode)
        {
            this.Personal = ProcessValue(targetInfoNode["PERSONAL"].InnerText);
            this.Surface = ProcessValue(targetInfoNode["SURFACE"].InnerText);
            this.Air = ProcessValue(targetInfoNode["AIR"].InnerText);
            this.Ship = ProcessValue(targetInfoNode["SHIP"].InnerText);
        }

    }
}
