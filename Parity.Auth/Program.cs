using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Auth
{
    class Program
    {

        static void Main(string[] args)
        {

            Kernel.Program.Bootstrap(() => { return new Server.Core(); });
            
        }
    }
}