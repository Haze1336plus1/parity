using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.ManagementConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            Base.IO.SetupWindow("Parity Server [Management Console]");
            Base.IO.Notice("Parity Management Console");
        }
    }
}
