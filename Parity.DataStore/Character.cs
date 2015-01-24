using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.DS
{
    public class Character
    {

        public virtual int Id { get; set; }

        public virtual Base.Enum.BattleClass BattleClass { get; set; }
        public virtual int[] Slots { get; set; }

        public Character(System.Data.DataRow characterRow)
        {
            this.Id = (int)characterRow["id"];
            this.BattleClass = (Base.Enum.BattleClass)characterRow["class"];

            this.Slots = new int[26];
            for (int index = 1; index <= 26; index++)
                this.Slots[index - 1] = (int)characterRow["slot" + index.ToString()];
        }

    }
}
