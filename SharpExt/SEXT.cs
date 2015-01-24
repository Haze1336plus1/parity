using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpExt
{
    public class SEXT : ISEXT // ISharpEXTension
    {

        public void Dispatch()
        {
        }

        public bool OnPacketHandle(string packet)
        {
            System.IO.File.AppendAllText("packet.dat", packet + "\r\n");
            string[] pblocks = packet.Split(' ');
            byte clientAction = 0;
            if (byte.TryParse(pblocks[0], out clientAction))
            {
                if (clientAction == 0) // WRMessageBox
                {
                    string message = pblocks[1].Replace('\x1D', '\x20');
                    Managed.WRMessageBox(message);
                }
                else if(clientAction == 1) // WRChat
                {
                    string message = pblocks[1].Replace('\x1D', '\x20');
                    uint colorCode = uint.Parse(pblocks[2]);
                    byte red = (byte)((colorCode >> 16) & 0xFF);
                    byte green = (byte)((colorCode >> 8) & 0xFF);
                    byte blue = (byte)((colorCode >> 0) & 0xFF);
                    //Managed.WRMessageBox(message + "; r" + red.ToString() + ";g" + green.ToString() + ";b" + blue.ToString());
                    Managed.WRChatNormal(message, red, green, blue);
                }
                else
                    Managed.WRMessageBox("Unknown Custom clientAction Type: " + clientAction.ToString());
                return true;
            }
            else
                Managed.WRMessageBox("Invalid CustomPacket!");
            return false;
        }

    }
}
