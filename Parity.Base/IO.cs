using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Base
{
    public sealed class IO
    {

        #region Defaults and Values

        public static Dictionary<string, object> Configuration;
        public static Dictionary<Enum.LoggingLevel, Pair<ConsoleColor, ConsoleColor>> ColorScheme;

        private static IO _io;
        private static object _lockingObject = new object();
        public static IO GetInstance()
        {
            lock (IO._lockingObject)
            {
                if (IO._io == null)
                    IO._io = new IO();
                lock (IO._io)
                {
                    return IO._io;
                }
            }
        }

        #endregion

        #region Constructors

        // Constructor
        private IO()
        {
            // release.. the kraken! >:D
            Console.SetCursorPosition(0, 2);
        }
        
        // Static Constructor
        static IO()
        {
            IO.Configuration = new Dictionary<string, object>()
            {
                { "Heading", "" },
                { "FancyOutput", true },
                { "FullTime", false },
                { "LogStack", false },
                { "StackDeepness", 2 },
                { "Culture", new System.Globalization.CultureInfo("de-DE") }
            };
            IO.ColorScheme = new Dictionary<Enum.LoggingLevel, Pair<ConsoleColor, ConsoleColor>>()
            {
                { Enum.LoggingLevel.Emergency, new Pair<ConsoleColor, ConsoleColor>(ConsoleColor.DarkRed, ConsoleColor.Red) },
                { Enum.LoggingLevel.Alert, new Pair<ConsoleColor, ConsoleColor>(ConsoleColor.Yellow, ConsoleColor.Red) },
                { Enum.LoggingLevel.Critical, new Pair<ConsoleColor, ConsoleColor>(ConsoleColor.DarkRed, ConsoleColor.White) },
                { Enum.LoggingLevel.Error, new Pair<ConsoleColor, ConsoleColor>(ConsoleColor.Red, ConsoleColor.White) },
                { Enum.LoggingLevel.Warning, new Pair<ConsoleColor, ConsoleColor>(ConsoleColor.Yellow, ConsoleColor.White) },
                { Enum.LoggingLevel.Notice, new Pair<ConsoleColor, ConsoleColor>(ConsoleColor.DarkCyan, ConsoleColor.Cyan) },
                { Enum.LoggingLevel.Informational, new Pair<ConsoleColor, ConsoleColor>(ConsoleColor.DarkYellow, ConsoleColor.Yellow) },
                { Enum.LoggingLevel.Debug, new Pair<ConsoleColor, ConsoleColor>(ConsoleColor.DarkMagenta, ConsoleColor.Magenta) },
            };
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Sets the mainline of console window and updates it
        /// </summary>
        /// <param name="Message">Mainline message</param>
        public void SetHeading(string Message)
        {
            IO.Configuration["Heading"] = Message;
            this.Update();
        }

        /// <summary>
        /// Balances Buffer and Window size, set's window title
        /// </summary>
        /// <param name="Title">Console Window Title</param>
        /// <param name="BufferWidth">Buffer (and also Window) -width of Console</param>
        /// <param name="BufferHeight">Buffer (and also Window) -height of Console</param>
        public static void SetupWindow(string Title, byte BufferWidth = 98, byte BufferHeight = 32)
        {
            Console.Title = Title;
            Console.WindowWidth = BufferWidth;
            Console.WindowHeight = BufferHeight;
            Console.BufferWidth = BufferWidth;
            Console.BufferHeight = BufferHeight;
        }

        #endregion

        #region Console Operation

        /// <summary>
        /// Update the Console Heading nice and clean
        /// </summary>
        private void Update()
        {
            // MAGIC!
            int oldLeft = Console.CursorLeft, oldTop = Console.CursorTop;
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.SetCursorPosition(0, 0);
            Console.Write(((string)IO.Configuration["Heading"]).PadRight(Console.BufferWidth, ' '));
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(String.Join(" ", Enumerable.Repeat<string>("-", Console.BufferWidth / 2).ToArray()).PadRight(Console.BufferWidth, ' '));
            Console.SetCursorPosition(oldLeft, oldTop);
            Console.ForegroundColor = oldColor;
            Console.BackgroundColor = ConsoleColor.Black;
        }

        /// <summary>
        /// Sends a raw message with given colors to the console
        /// </summary>
        /// <param name="ColorA">Prefixcolor, if fancy</param>
        /// <param name="ColorB">Messagecolor, if fancy</param>
        /// <param name="Message">Message to be sent to console</param>
        private void Message(Enum.LoggingLevel loggingLevel, string Message)
        {
            // we actually dont need to lock at this point anymore, since GetInstance() does a lock for us

            Pair<ConsoleColor, ConsoleColor> scheme = IO.ColorScheme[loggingLevel];

            System.Diagnostics.StackFrame SF = new System.Diagnostics.StackTrace().GetFrame((int)IO.Configuration["StackDeepness"]);
            System.Globalization.CultureInfo culture = (System.Globalization.CultureInfo)IO.Configuration["Culture"];
            bool fullTime = (bool)IO.Configuration["FullTime"];

            if ((bool)IO.Configuration["FancyOutput"])
            {
                Console.ForegroundColor = ConsoleColor.White;
                //Console.Write(string.Empty);
                Console.ForegroundColor = ConsoleColor.Gray;

                if (fullTime)
                    Console.Write(DateTime.Now.ToString("dd.MM HH:mm:ss", culture));
                else
                    Console.Write(DateTime.Now.ToString("HH:mm:ss", culture));
                Console.Write(" [{0}", loggingLevel.ToString());

                if ((bool)IO.Configuration["LogStack"])
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" | ");
                    Console.ForegroundColor = scheme.First;
                    Console.Write(SF.GetMethod().ReflectedType.Name + "." + SF.GetMethod().Name.Replace(".ctor", "/new").Replace(".cctor", "/static"));
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("] > ");
                }
                else
                    Console.Write("] > ");

                Console.ForegroundColor = scheme.Second;
                if (Console.CursorLeft + Message.Length == Console.BufferWidth)
                    Console.Write(Message);
                else
                    Console.WriteLine(Message);
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                if (fullTime)
                    Console.Write(DateTime.Now.ToString("dd.MM HH:mm:ss", culture));
                else
                    Console.Write(DateTime.Now.ToString("HH:mm:ss", culture));
                Console.Write(" [{0}", loggingLevel.ToString());

                if ((bool)IO.Configuration["LogStack"])
                {
                    Console.Write(" | ");
                    Console.Write(SF.GetMethod().ReflectedType.Name + "." + SF.GetMethod().Name.Replace(".ctor", "/new").Replace(".cctor", "/static"));
                    Console.Write("] > ");
                }
                else
                    Console.Write("] > ");

                if (Console.CursorLeft + Message.Length == Console.BufferWidth)
                    Console.Write(Message);
                else
                    Console.WriteLine(Message);

            }

            this.Update();
        }

        #region LoggingLevel's (static)

        /// <summary>
        /// Emergency
        /// </summary>
        /// <param name="message">Message to be sent to console</param>
        public static void Emergency(string message)
        {
            GetInstance().Message(Enum.LoggingLevel.Emergency, message);
        }

        /// <summary>
        /// Alert
        /// </summary>
        /// <param name="message">Message to be sent to console</param>
        public static void Alert(string message)
        {
            GetInstance().Message(Enum.LoggingLevel.Alert, message);
        }

        /// <summary>
        /// Critical
        /// </summary>
        /// <param name="message">Message to be sent to console</param>
        public static void Critical(string message)
        {
            GetInstance().Message(Enum.LoggingLevel.Critical, message);
        }

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="message">Message to be sent to console</param>
        public static void Error(string message)
        {
            GetInstance().Message(Enum.LoggingLevel.Error, message);
        }

        /// <summary>
        /// Warning
        /// </summary>
        /// <param name="message">Message to be sent to console</param>
        public static void Warning(string message)
        {
            GetInstance().Message(Enum.LoggingLevel.Warning, message);
        }

        /// <summary>
        /// Notice
        /// </summary>
        /// <param name="message">Message to be sent to console</param>
        public static void Notice(string message)
        {
            GetInstance().Message(Enum.LoggingLevel.Notice, message);
        }

        /// <summary>
        /// Informational
        /// </summary>
        /// <param name="message">Message to be sent to console</param>
        public static void Informational(string message)
        {
            GetInstance().Message(Enum.LoggingLevel.Informational, message);
        }

        /// <summary>
        /// Debug
        /// </summary>
        /// <param name="message">Message to be sent to console</param>
        public static void Debug(string message)
        {
            GetInstance().Message(Enum.LoggingLevel.Debug, message);
        }

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Callback...
        /// </summary>
        public static event Action<string, Enum.LoggingLevel> OnMessage;

        #endregion

    }
}
