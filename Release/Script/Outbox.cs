using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Game.Script.Outbox
{

	public class ActivateDC64 : IOutboxActivation
    {

        private string[] _items;
        public ActivateDC64()
        {
            this._items = new string[] { "DC64" };
        }

        public string ApplyTo
        {
            get { return "DC64"; }
        }

        public string[] Items
        {
            get { return this._items; }
        }

        public void Activate(Client.Client sender, int duration)
        {
            Util.PremiumUser(sender, Base.Enum.Premium.Platin, 30);
        }

    }

}