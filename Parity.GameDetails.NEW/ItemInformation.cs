using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.GameDetails
{
    public class ItemInformation
    {

        public string ItemCode { get; private set; }
        public Base.Enum.Item.Type Type { get; private set; }
        public Base.Enum.Item.Resource Resource { get; private set; }
        public Base.Enum.Item.Character Character { get; private set; }
        public Base.Enum.Item.Etc Etc { get; private set; }
        public Base.Enum.Item.Weapon Weapon { get; private set; }
        public Base.Enum.Item.Equipment Equipment { get; private set; }
        public Base.Enum.Item.EquipmentWeapon EquipmentWeapon { get; private set; }
        public Base.Enum.DamageType DamageType { get; private set; }

        public ItemInformation(string ItemCode)
        {
            this.ItemCode = ItemCode;
            this.Type = (Base.Enum.Item.Type)(ItemCode[0] - 'A');
            this.Resource = (Base.Enum.Item.Resource)(ItemCode[1] - 'A');
            this.Character = (Base.Enum.Item.Character)(ItemCode[1] - 'A');
            this.Etc = (Base.Enum.Item.Etc)(ItemCode[1] - 'A');
            // Weapon
            {
                if (char.IsDigit(ItemCode[1]))
                {
                    byte itemCodeNum = byte.Parse(ItemCode[1].ToString());
                    if (itemCodeNum == 0)
                        this.Weapon = Base.Enum.Item.Weapon.Destructive_Model_Weapon;
                    else if (itemCodeNum == 1)
                        this.Weapon = Base.Enum.Item.Weapon.Slot6thChange;
                    else if (itemCodeNum == 2)
                        this.Weapon = Base.Enum.Item.Weapon.Dagger_Throw; // custom one
                    else if (itemCodeNum == 4)
                        this.Weapon = Base.Enum.Item.Weapon.Common_8th;
                    else if (itemCodeNum == 5)
                        this.Weapon = Base.Enum.Item.Weapon.Engineer_8th;
                }
                else
                    this.Weapon = (Base.Enum.Item.Weapon)(ItemCode[1] - 'A');
            }

            this.Equipment = (Base.Enum.Item.Equipment)(ItemCode[1] - 'A');
            this.EquipmentWeapon = (Base.Enum.Item.EquipmentWeapon)(ItemCode[1] - 'A');

            this.DamageType = Base.Enum.DamageType.Surface;

            if (this.Equipment == Base.Enum.Item.Equipment.Helicopter ||
                this.Equipment == Base.Enum.Item.Equipment.Airplane)
            {
                this.DamageType = Base.Enum.DamageType.Air;
            }
            else if (this.Equipment == Base.Enum.Item.Equipment.NavalTransport || // naval = marine / sea
                this.Equipment == Base.Enum.Item.Equipment.NavalAttack)
            {
                this.DamageType = Base.Enum.DamageType.Ship;
            }
            else if (this.Equipment == Base.Enum.Item.Equipment.Destructive_Model)
            {
                this.DamageType = Base.Enum.DamageType.Personal; // derp?
            }
        }

        public override string ToString()
        {
            string returnValue = (this.ItemCode + ": " + this.Type.ToString() + " - ");
            if (this.Type == Base.Enum.Item.Type.Resource) returnValue += this.Resource.ToString();
            if (this.Type == Base.Enum.Item.Type.Character) returnValue += this.Character.ToString();
            if (this.Type == Base.Enum.Item.Type.Etc) returnValue += this.Etc.ToString();
            if (this.Type == Base.Enum.Item.Type.Weapon) returnValue += this.Weapon.ToString();
            if (this.Type == Base.Enum.Item.Type.Equipment) returnValue += this.Equipment.ToString();
            if (this.Type == Base.Enum.Item.Type.EquipmentWeapon) returnValue += this.EquipmentWeapon.ToString();
            return returnValue;
        }

    }
}
