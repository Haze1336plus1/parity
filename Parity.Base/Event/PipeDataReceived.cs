using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Base.Event
{

    public class PipeDataReceivedEventArgs : EventArgs
    {

        public string Data { get; private set; }

        public PipeDataReceivedEventArgs(string data)
            : base()
        {
            this.Data = data;
        }

    }

    public delegate void PipeDataReceivedDelegate(object sender, PipeDataReceivedEventArgs e);
}
