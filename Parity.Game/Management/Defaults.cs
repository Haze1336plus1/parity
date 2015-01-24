using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Game.Management
{
    public class Defaults
    {

        public readonly DS.Item[][] Equipment;
        public readonly DS.Item[][] Character;

        public Defaults()
        {
            this.Equipment = new DS.Item[5][];
            this.Character = new DS.Item[5][];
            for (byte i = 0; i < 5; i++)
            {
                this.Equipment[i] = new DS.Item[9]; // ninth item is 6th slot change
                this.Character[i] = new DS.Item[26];
                for (byte j = 0; j < 8; j++)
                {
                    string itemCode = Modules.WarRock.BranchContainer.Classes[i].SlotInfo[j].Code;
                    if (!String.IsNullOrEmpty(itemCode))
                        this.Equipment[i][j] = new DS.Item(itemCode);
                }
                this.Character[i][0] = new DS.Item("BA0" + (i + 1).ToString()); // default characters
            }
        }

        public DS.Item GetEquipment(Base.Enum.BattleClass battleClass, byte slot)
        {
            return this.Equipment[(int)battleClass][slot];
        }
        public bool IsDefaultEquipment(Base.Enum.BattleClass battleClass, DS.Item item)
        {
            return this.Equipment[(int)battleClass].Contains(item);
        }
        public bool IsDefaultEquipment(Base.Enum.BattleClass battleClass, string item)
        {
            return this.Equipment[(int)battleClass].Where(x => x != null && x.Code == item).Count() > 0;
        }

        public DS.Item GetCharacter(Base.Enum.BattleClass battleClass, byte slot)
        {
            return this.Character[(int)battleClass][slot];
        }
        public bool IsDefaultCharacter(Base.Enum.BattleClass battleClass, DS.Item item)
        {
            return this.Character[(int)battleClass].Contains(item);
        }
        public bool IsDefaultCharacter(Base.Enum.BattleClass battleClass, string item)
        {
            return this.Character[(int)battleClass].Where(x => x != null && x.Code == item).Count() > 0;
        }
        
    }
}
