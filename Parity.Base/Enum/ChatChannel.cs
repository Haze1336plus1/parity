using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Base.Enum
{
    public enum ChatChannel : byte
    {

        WhisperSelf,
        Notice,
        // 2?
        LobbyChannel = 3,
        RoomAll,
        RoomTeam,
        Whisper,
        // 7?
        LobbyAll = 8,
        Clan

    }
}
