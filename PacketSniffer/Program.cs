using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketSniffer
{
    class Program
    {
        static void Main(string[] args)
        {

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Parity.Base.IO.Configuration["LogStack"] = false;
            Parity.Base.IO.SetupWindow("Parity Sniffer", BufferHeight: 8);
            Parity.Base.IO.GetInstance().SetHeading("Parity Sniffer starting ...");
            Parity.Base.IO.Notice("Starting Parity Sniffer ...");

            Parity.Base.IO.Informational("Ports > Login: {0} > Lobby: {1} >  Gameplay: {2}"
                    .Process(
                        Configuration.Get<int>("Port.Login"),
                        Configuration.Get<int>("Port.Lobby"),
                        Configuration.Get<int>("Port.Gameplay")
                    )
                );

            System.Net.IPAddress netIp = System.Net.IPAddress.Any;

            Parity.Base.IO.Notice("Starting sniffing on all network devices");
            List<string> activeMacs = new List<string>();
            foreach (SharpPcap.ICaptureDevice iDevice in SharpPcap.CaptureDeviceList.Instance)
            {
                Parity.Base.IO.Informational(" {0}".Process(iDevice.Description));
                Net.Sniffer deviceSniffer = new Net.Sniffer(iDevice);
                deviceSniffer.StartReceive();
            }

            Parity.Base.IO.Notice("We're up and running!");

            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);

        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.Clear();
            Console.WriteLine("Something went wrong!");
            Console.WriteLine(e.ToString());
            Console.WriteLine("Please report to the Developer!");
            Console.ReadLine();
        }
    }
}
