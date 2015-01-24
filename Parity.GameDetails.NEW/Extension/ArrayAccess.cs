using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Collections.Generic
{
    public static class ArrayAccess
    {

        // Character
        public static Parity.GameDetails.WRBin.Items.Character Find(this Parity.GameDetails.WRBin.Items.Character[] collection, string code)
        {
            return (from character in collection where character.BasicInfo.Code == code select character).FirstOrDefault();
        }

        // Etc
        public static Parity.GameDetails.WRBin.Items.Etc Find(this Parity.GameDetails.WRBin.Items.Etc[] collection, string code)
        {
            return (from etc in collection where etc.BasicInfo.Code == code select etc).FirstOrDefault();
        }

        // Weapon
        public static Parity.GameDetails.WRBin.Items.Weapon Find(this Parity.GameDetails.WRBin.Items.Weapon[] collection, string code)
        {
            return (from weapon in collection where weapon.BasicInfo.Code == code select weapon).FirstOrDefault();
        }

    }
}
