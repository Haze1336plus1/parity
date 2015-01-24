using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Kernel
{
    public class Program
    {

        public static readonly bool DebugMode = false;

        public static void StartupError(string message)
        {
            Base.IO.Critical(message);

        }

        public static void Bootstrap(Func<ServerBase> activator)
        {
            Base.IO.SetupWindow("Parity Server", 120, (byte)Console.WindowHeight);
            Base.IO.GetInstance().SetHeading("Parity bootstrap ...");
            Base.IO.Notice("Bootstrapping Parity ...");

            if (Program.DebugMode)
            {
                try
                {
                    ServerBase instance = activator.DynamicInvoke() as ServerBase;
                    instance.Start();
                }
                catch (Base.Exception.CustomException cex)
                {
                    cex.Handle();
                }
                catch (System.Exception ex)
                {
                    Base.IO.Emergency(ex.Message);
                    Base.IO.Debug("Exception details written to crash.txt");
                    if (ex.InnerException != null)
                        ex = ex.InnerException;
                    System.IO.File.AppendAllText("crash.txt", DateTime.Now.ToString() + " >>>\r\n" + ex.ToString() + "\r\n\r\n");
                }
            }
            else
            {
                ServerBase instance = activator.DynamicInvoke() as ServerBase;
                instance.Start();
            }

            Base.Thread.Timer.GetInstance().Start();
        }

    }
}
