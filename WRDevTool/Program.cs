using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Parity.WRDevTool
{
    class Program
    {

        public static Controller.Basic Controller { get; private set; }

        static void Main(string[] args)
        {
            Program.Controller = new Controller.Basic();
            if (Program.Controller.Run())
            {
                Base.IO.GetInstance().SetHeading("This is the end!");
                Console.WriteLine("- - - - - - - - - - -");
                Console.WriteLine("        This is the end ...");
                Console.WriteLine("... beautiful friend, this is the end ...");
                Console.WriteLine("  ... My only friend, the end...");
                Console.ReadLine(); //System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
            }
            else
                Console.ReadLine();
        }
    }
}
