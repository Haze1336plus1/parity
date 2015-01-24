using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.DS
{
    public class Equipment
    {

        public virtual int Id { get; set; }

        public virtual Base.Enum.BattleClass BattleClass { get; set; }
        public virtual int[] Slots { get; set; }
        public virtual int Slot6Change { get; set; }

        public Equipment(System.Data.DataRow equipmentRow)
        {
            this.Id = (int)equipmentRow["id"];
            this.BattleClass = (Base.Enum.BattleClass)equipmentRow["class"];

            this.Slots = new int[8];
            for (int index = 1; index <= 8; index++)
                this.Slots[index - 1] = (int)equipmentRow["slot" + index.ToString()];
            this.Slot6Change = (int)equipmentRow["slot6change"];
        }

    }
}
