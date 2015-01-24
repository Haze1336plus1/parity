using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Game
{
    class Program
    {
        static void Main(string[] args)
        {

            // y u read comments, faggot?

            Kernel.Program.Bootstrap(() => { return new Server.Core(); });

        }
    }
}