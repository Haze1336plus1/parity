using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Management
{
    public class OutboxActivation
    {

        private Dictionary<string, Script.IOutboxActivation> _activationList;

        public OutboxActivation()
        {
            this._activationList = new Dictionary<string, Script.IOutboxActivation>();
            Script.IOutboxActivation[] tmpActivationList = QA.Core.ParityScript.GetOutboxActivation();
            foreach (Script.IOutboxActivation obActivation in tmpActivationList)
                this._activationList.Add(obActivation.ApplyTo, obActivation);
        }

        public Script.IOutboxActivation GetActivation(string itemCode)
        {
            if (this._activationList.ContainsKey(itemCode))
                return this._activationList[itemCode];
            return null;
        }

    }
}
