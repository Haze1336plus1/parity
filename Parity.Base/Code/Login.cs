using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Base.Code
{
    public enum Login
    {

        Success = 1,

        RequestNickname = 72000,
        Unregistered_User = 72010,
        Invalid_Password = 72020,
        Already_Logged_In = 72030,
        Verify_Email_Address = 73040,
        BannedWithTime = 73020,
        Normal_Procedure = 73030,
        Banned = 73050,
        EnterID = 74010,
        EnterPassword = 74020,
        EnterCharacterName = 74030,
        ClientVersionMissmatch = 70301,
        FailedAccountValidation = 70201,
        AccountNotActive = 70202

    }
}
