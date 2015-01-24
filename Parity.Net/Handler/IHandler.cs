using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Net.Handler
{
    public abstract class IHandler<clientType> where clientType : Net.Server.TCP.VirtualClient
    {

        private Net.Packet.InPacket _packet;
        private bool _customHandle;

        public virtual Result Handle(clientType sender, Net.Packet.InPacket packet, bool customHandle = false)
        {
            this._packet = packet;
            this._customHandle = customHandle;
            return Result.Default;
        }

        private bool CheckAndReturn(bool retVal)
        {
            if (!retVal && !this._customHandle)
                throw new Base.Exception.CustomException(Base.Exception.CustomFlags.NetHandleBreak);
            return retVal;
        }
        protected bool RequireLength(int required, int max = -1)
        {
            bool retVal = this._packet.ParamsCount == required || (max >= 0 && this._packet.ParamsCount <= max);
            return this.CheckAndReturn(retVal);
        }
        protected bool RequireType<T>(int index)
        {
            bool retVal = Base.Types.IsValid<T>(this._packet[index]);
            return this.CheckAndReturn(retVal);
        }
        protected bool RequireValue<T>(int index, out T value)
        {
            bool retVal = Base.Types.TryParse(this._packet[index], out value);
            return this.CheckAndReturn(retVal);
        }
        protected bool RequireEnum<T>(int index, out T value) where T: struct
        {
            bool retVal = Base.Types.ParseEnum(this._packet[index], out value);
            return this.CheckAndReturn(retVal);
        }
        protected bool RequireExact(int index, string exactValue)
        {
            bool retVal = this._packet[index] == exactValue;
            return this.CheckAndReturn(retVal);
        }

    }
}
