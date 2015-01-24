using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.GameDetails.WRBin.Items.Overall
{
    public struct UseInfo
    {

        public readonly Base.Enum.Premium ApplyTarget;
        public readonly int ApplyOption;
        public readonly int AddPoing; // wtf
        public readonly int DurationTime;
        public readonly Base.Enum.Channel[] UseableChannel;

        public UseInfo(System.Xml.XmlNode useInfoNode)
        {
            Base.Types.ParseEnum<Base.Enum.Premium>(useInfoNode["APPLY_TARGET"].InnerText, out this.ApplyTarget);
            this.ApplyOption = int.Parse(useInfoNode["APPLY_OPTION"].InnerText);
            this.AddPoing = int.Parse(useInfoNode["ADD_POING"].InnerText);
            if(useInfoNode["DURATIONTIME"] != null)
                this.DurationTime = int.Parse(useInfoNode["DURATIONTIME"].InnerText);
            else
                this.DurationTime = int.Parse(useInfoNode["DURATION_TIME"].InnerText);

            Base.App.FlagList<Base.Enum.Channel> channelFlags = new Base.App.FlagList<Base.Enum.Channel>((Base.Enum.Channel[])Enum.GetValues(typeof(Base.Enum.Channel)));

            if (useInfoNode["USEABLE_CHANNEL"] != null)
            {

                int[] channelFlagValues = new int[4];
                string[] splitInput = useInfoNode["USEABLE_CHANNEL"].InnerText.Split(',');
                Array.Resize(ref splitInput, 4);
                for (int i = 0; i < 4; i++)
                    channelFlagValues[i] = int.Parse(splitInput[i].Fallback("-1"));
                Array.Resize(ref channelFlagValues, 4); // 3 channels
                channelFlags.Change(Base.Enum.Channel.CQC, channelFlagValues[0] == 1);
                channelFlags.Change(Base.Enum.Channel.BG, channelFlagValues[2] == 1);
                channelFlags.Change(Base.Enum.Channel.AI, channelFlagValues[3] == 1);

                this.UseableChannel = channelFlags.GetFlags();

            }
            else
                this.UseableChannel = new Base.Enum.Channel[] { Base.Enum.Channel.CQC, Base.Enum.Channel.BG, Base.Enum.Channel.AI };
        }

        public UseInfo(Base.Enum.Premium applyTarget, int applyOption, int addPoing, int durationTime, Base.Enum.Channel[] useableChannel)
        {
            this.ApplyTarget = applyTarget;
            this.ApplyOption = applyOption;
            this.AddPoing = addPoing;
            this.DurationTime = durationTime;
            this.UseableChannel = useableChannel;
        }

    }
}
