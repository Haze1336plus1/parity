using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.GameDetails.WRBin.Items.Overall
{
    public struct TargetInfo
    {

        public readonly float[] Low;
        public readonly float[] Middle;
        public readonly float[] High;

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
            this.Low = ProcessValue(targetInfoNode["Low"].InnerText);
            this.Middle = ProcessValue(targetInfoNode["Middle"].InnerText);
            this.High = ProcessValue(targetInfoNode["High"].InnerText);
        }

    }
}
