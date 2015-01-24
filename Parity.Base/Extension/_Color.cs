using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public class Color
    {

        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }
        public uint Code { get { return (uint)((this.Red << 16) | (this.Green << 8) | this.Blue); } }

        public Color(uint code)
        {
            this.Red = (byte)((code >> 16) & 0xFF);
            this.Green = (byte)((code >> 8) & 0xFF);
            this.Blue = (byte)(code & 0xFF);
        }
        public Color(byte red, byte green, byte blue)
        {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }

    }
}
