using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Game.Client
{
    public class SessionDetails
    {

        public readonly Client Owner;
        public InventoryDetails Inventory { get; private set; }
        public CharacterDetails Character { get; private set; }
        public short SessionID { get; private set; }
        public bool IsActive { get; private set; }
        public string MacAddress { get; private set; }
        public int SessionTime { get; set; }
        public int SessionTimeKA { get; set; }

        public System.Net.IPEndPoint LocalNetwork { get; set; }
        public System.Net.IPEndPoint RemoteNetwork { get; set; }
        public long PingTime { get; set; }

        public DS.Account Account { get; private set; }

        public SessionDetails(Client owner)
        {
            this.Owner = owner;
            {
                // network defaults
                System.Net.IPEndPoint ipe = (System.Net.IPEndPoint)owner.Socket.RemoteEndPoint;
                this.LocalNetwork = new System.Net.IPEndPoint(ipe.Address, ipe.Port);
                this.RemoteNetwork = new System.Net.IPEndPoint(ipe.Address, ipe.Port);
            }
        }

        public void SetMacAddress(string macAddress)
        {
            this.MacAddress = macAddress;
        }

        public void SetSession(DS.Account newSession)
        {
            if (this.IsActive)
                throw new Exception("Session already set");
            this.Account = newSession;
            this.SessionID = Parity.Game.Management.IdDistributor.GetSession();
            this.Inventory = new InventoryDetails(this.Owner);
            this.Character = new CharacterDetails(this.Owner);
            this.IsActive = true;
            this.Account.MacAddress = this.MacAddress;

            if(this.Account.AuthLevel == Base.Enum.AuthLevel.Moderator)
                this.Account.Details.Experience = Parity.Game.Management.Level.GetExperience(100);
            else if(this.Account.AuthLevel == Base.Enum.AuthLevel.Administrator)
                this.Account.Details.Experience = Parity.Game.Management.Level.GetExperience(99);
        }

    }
}
