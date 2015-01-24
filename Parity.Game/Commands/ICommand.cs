using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Commands
{
    public interface ICommand
    {

        string Command { get; }
        string Description { get; }
        Base.Enum.AuthLevel RequiredLevel { get; }

        bool HandleCommand(Base.Layout.CommandInvoker invoker, string command, string[] arguments);

    }
}
