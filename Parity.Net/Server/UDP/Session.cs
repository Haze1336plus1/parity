using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Net.Server.UDP
{
    public class Session
    {

        public ManagedUDP Owner { get; private set; }
        public System.Net.IPEndPoint RemoteEndPoint { get; private set; }
        public long LastResponse { get; private set; }

        public bool IsIdle { get { return Base.App.Time.IsOlder(this.LastResponse, Constant.SessionTimeout); } }

        public Session(ManagedUDP owner, System.Net.IPEndPoint remoteEndPoint)
        {
            this.RemoteEndPoint = remoteEndPoint;
            this.LastResponse = Base.App.Time.Get();
            this.Owner = owner;
        }

        public void Send(byte[] data)
        {
            this.Owner.Socket.SendTo(data, this.RemoteEndPoint);
        }

        public void DataReceived()
        {
            this.LastResponse = Base.App.Time.Get();
        }

    }
}
