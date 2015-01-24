using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Auth.Packet
{
    public class Patch : Net.Packet.OutPacket
    {

        public Patch(Kernel.Config.cVersion version)
            : base(Net.PacketCodes.LPATCH)
        {

            base.Add(0); // base.AddBlock(version.Format);
            base.Add(0); // base.AddBlock(version.Launcher);
            base.Add(0); // base.AddBlock(version.Updater);
            base.Add(version.Version); // base.AddBlock(version.Client);
            base.Add(0); // base.AddBlock(version.Sub);
            base.Add(0); // base.AddBlock(version.Option);
            base.Add(version.PatchSrv);

        }

    }
}
