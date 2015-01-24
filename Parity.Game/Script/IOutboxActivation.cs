using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Script
{
    public interface IOutboxActivation
    {

        string ApplyTo { get; }
        string[] Items { get; }
        void Activate(Client.Client sender, int duration);

    }
}
