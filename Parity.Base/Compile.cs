using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Base
{
    public class Compile
    {

        public static readonly string DataDirectory;
        public static readonly string DataRemoteDirectory;
        public static readonly string ConfigDirectory;
        public static readonly string LogDirectory;
        public static readonly uint GameVersion;

        public static Dictionary<string, string> FileNames;
        public static Dictionary<string, object> GameDefaults;

        public static Version ExecutingVersion;

        static Compile()
        {
            Compile.DataDirectory = System.IO.Path.Combine(Environment.CurrentDirectory, @"Data\");
            Compile.DataRemoteDirectory = System.IO.Path.Combine(Compile.DataDirectory, @"Remote\");
            Compile.ConfigDirectory = System.IO.Path.Combine(Environment.CurrentDirectory, @"Config\");
            Compile.LogDirectory = System.IO.Path.Combine(Environment.CurrentDirectory, @"Log\");

            Compile.ExecutingVersion = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
            Compile.GameVersion = (uint)(1 << 16 | Compile.ExecutingVersion.Major << 15 | Compile.ExecutingVersion.Minor << 11 | Compile.ExecutingVersion.Build);

            Compile.FileNames = new Dictionary<string, string>()
            {
                // Config
                { "Auth.Config", System.IO.Path.Combine(Compile.ConfigDirectory, "authServer.xml") },
                { "Game.Config", System.IO.Path.Combine(Compile.ConfigDirectory, "gameServer.xml") },
                { "Database.Config", System.IO.Path.Combine(Compile.ConfigDirectory, "Database.xml") },
                // Bin Files
                { "Game.ItemsBin", System.IO.Path.Combine(Compile.DataDirectory, "items.bin") },
                { "Game.BranchBin", System.IO.Path.Combine(Compile.DataDirectory, "branch.bin") },
                // Xml Files
                { "Game.ItemsXml", System.IO.Path.Combine(Compile.DataDirectory, "items.xml") },
                { "Game.BranchXml", System.IO.Path.Combine(Compile.DataDirectory, "branch.xml") },
                { "Game.DetailsXml", System.IO.Path.Combine(Compile.DataDirectory, "GameDetails.xml") }
            };

            Compile.GameDefaults = new Dictionary<string, object>()
            {
                // Inventory
                { "Inventory.Limit", 32 },
                { "Inventory.ExtendMultiplier" , 8 },
                { "Inventory.MaxItemTime", 365 },
                // Character
                { "Character.Limit", 24 },
                // Lobby
                { "Lobby.RoomLimit", 255 },
                { "Lobby.SpectatorLimit", 16 },
                { "Lobby.UpdateFrequency", 5000 }, // LobbyUpdate frequency in msec
                // Game
                { "Game.EndingTime", 5000 },
                // Server
                { "Server.IOPipeName", "ppiopipe" }
            };
        }

    }
}
