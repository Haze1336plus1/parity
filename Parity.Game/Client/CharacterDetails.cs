using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Game.Client
{
    public class CharacterDetails
    {

        public readonly Client Owner;
        protected List<DS.Item> _characters;
        public DS.Item[] Items { get { return this._characters.ToArray(); } }
        public DS.Item[] Expired { get; private set; }
        public DS.Item[][] Equipment { get; private set; }
        public static readonly DS.Item EmptySlot;

        #region Character

        static CharacterDetails()
        {
            CharacterDetails.EmptySlot = new DS.Item("^");
        }

        protected void ChangedCharacter(Base.Enum.BattleClass battleClass)
        {
            for (int i = 1; i < 26; i++)
                this.Equipment[(int)battleClass][i] = null;
        }

        /// <summary>
        /// Equip an existing item to battle class on a given slot
        /// </summary>
        /// <param name="item">Item to be equipped</param>
        /// <param name="battleClass">Class to equip item to</param>
        /// <param name="slot">Slot to equip item to</param>
        public void Equip(DS.Item item, Base.Enum.BattleClass battleClass, byte slot)
        {
            // if this item is already equipped in the class, ignore that
            if (this.Equipment[(int)battleClass].Where(x => x != null && x.Code == item.Code).Count() > 0)
                return;
            this.Equipment[(int)battleClass][slot] = item;
            if ((Base.Enum.Item.Character)slot == Base.Enum.Item.Character.Flatform)
                this.ChangedCharacter(battleClass);
        }

        /// <summary>
        /// Unequip the itemcode from Battleclass and set default if necessary
        /// </summary>
        /// <param name="item">Item code to be unequipped</param>
        /// <param name="battleClass">Class to unequip item from</param>
        /// <param name="default">Set default or not</param>
        public void Unequip(string item, Base.Enum.BattleClass battleClass, bool @default = false)
        {
            for (byte i = 0; i < 26; i++)
            {
                DS.Item eqItem = this.Equipment[(int)battleClass][i];
                if (eqItem != null && eqItem.Code == item)
                {
                    if (@default || i == 0)
                        this.Equipment[(int)battleClass][i] = Modules.Defaults.GetCharacter(battleClass, i);
                    else
                        this.Equipment[(int)battleClass][i] = null;

                    if (i == 0) // 0 is Flatform
                        this.ChangedCharacter(battleClass);
                }
            }
        }

        /// <summary>
        /// Crate new InventoryDetails instance and loads it
        /// </summary>
        /// <param name="owner">Reference to Owner Client</param>
        public CharacterDetails(Client owner)
        {
            this.Owner = owner;
            this.LoadItems();
            this.Equipment = new DS.Item[5][];
            for (int i = 0; i < this.Equipment.Length; i++)
                this.Equipment[i] = new DS.Item[26];
            this.LoadEquipment();
        }

        /// <summary>
        /// Apply changes to Database
        /// </summary>
        public void Apply()
        {
            using (DS.Client dbc = QA.GetDBClient())
            {
                var exec = new Action<Base.Enum.BattleClass>((Base.Enum.BattleClass battleClass) =>
                {
                    DS.Item[] eq = this.Equipment[(int)battleClass];
                    var cmd = dbc
                        .Command("UPDATE `" + QA.Core.Config.GameConfig.Database + "`.`character` AS e SET e.slot1 = @slot1, e.slot2 = @slot2, e.slot3 = @slot3, e.slot4 = @slot4, e.slot5 = @slot5, e.slot6 = @slot6, e.slot7 = @slot7, e.slot8 = @slot8, e.slot9 = @slot9, e.slot10 = @slot10, e.slot11 = @slot11, e.slot12 = @slot12, e.slot13 = @slot13, e.slot14 = @slot14, e.slot15 = @slot15, e.slot16 = @slot16, e.slot17 = @slot17, e.slot18 = @slot18, e.slot19 = @slot19, e.slot20 = @slot20, e.slot21 = @slot21, e.slot22 = @slot22, e.slot23 = @slot23, e.slot24 = @slot24, e.slot25 = @slot25, e.slot26 = @slot26 WHERE e.class = @class AND e.account_id = @account_id LIMIT 1;");

                    for (int i = 1; i <= 26; i++)
                        cmd.SetParameter("slot" + i.ToString(), this.GetDatabaseId(battleClass, eq[i - 1]));

                    cmd.SetParameter("class", (int)battleClass)
                        .SetParameter("account_id", this.Owner.Session.Account.Id)
                        .Execute();
                });

                using (var trans = dbc.Connection.BeginTransaction())
                {

                    for (Base.Enum.BattleClass battleClass = Base.Enum.BattleClass.Engineer; battleClass <= Base.Enum.BattleClass.Antitank; battleClass++)
                        exec(battleClass);

                    trans.Commit();
                }

            }
        }

        #endregion

        #region Equipment

        /// <summary>
        /// Loads the Equipment from Session
        /// </summary>
        protected void LoadEquipment()
        {
            for (byte i = 0; i < 5; i++)
            {
                DS.Character eq = this.Owner.Session.Account.Character.ElementAt(i);
                for (byte j = 0; j < 26; j++)
                {
                    DS.Item citem = (from DS.Item iitem in this.Items where iitem.Id == eq.Slots[j] select iitem).FirstOrDefault();
                    this.Equipment[i][j] = Base.Decision.NotNull(citem, Modules.Defaults.GetCharacter((Base.Enum.BattleClass)i, j));
                }
            }
        }

        /// <summary>
        /// Get index of item in inventory
        /// </summary>
        /// <param name="item">Item to be looked for</param>
        /// <returns>Index of item in _items</returns>
        public int GetEquipmentId(DS.Item item)
        {
            return this._characters.IndexOf(item);
        }

        /// <summary>
        /// Get id of item ready to be inserted
        /// </summary>
        /// <param name="battleClass">Battle Class to lookup default equipment</param>
        /// <param name="item">Item that's set on that class</param>
        /// <returns>-1 for empty, 0 for default, > 0 for item id reference</returns>
        protected int GetDatabaseId(Base.Enum.BattleClass battleClass, DS.Item item)
        {
            if (item != null && item.Id > 0)
                return item.Id;
            else if (item == null) // obv...
                return -1;
            else if (Modules.Defaults.IsDefaultCharacter(battleClass, item))
                return 0;
            return -1;
        }

        /// <summary>
        /// Remove the Item from local list and drop it from database
        /// </summary>
        /// <param name="item">Item to be deleted</param>
        public void Delete(DS.Item item)
        {
            this.Unequip(item.Code, Base.Enum.BattleClass.Engineer, true);
            this.Unequip(item.Code, Base.Enum.BattleClass.Medic, true);
            this.Unequip(item.Code, Base.Enum.BattleClass.Patrol, true);
            this.Unequip(item.Code, Base.Enum.BattleClass.Assult, true);
            this.Unequip(item.Code, Base.Enum.BattleClass.Antitank, true);

            this._characters.Remove(item);
            using (DS.Client dbc = QA.GetDBClient())
            {
                dbc
                    .Command("DELETE FROM `" + QA.Core.Config.GameConfig.Database + "`.`items` WHERE `id` = @obId LIMIT 1;")
                    .SetParameter("obId", item.Id)
                    .Execute();
            }
        }

        #endregion

        #region Serialization

        /// <summary>
        /// Serializes Equipment into warrock-readable string
        /// </summary>
        /// <returns>Warrock-readable equipment string</returns>
        public string GetEquipment(Base.Enum.BattleClass battleClass)
        {
            string eq = string.Empty;
            for (int j = 0; j < 26; j++)
            {
                DS.Item eqs = this.Equipment[(int)battleClass][j];
                if (eqs == null)
                    eq += InventoryDetails.EmptySlot.Code;
                else
                {
                    int eqsId = this.GetEquipmentId(eqs);
                    if (eqsId == -1)
                        eq += eqs.Code;
                    else
                        eq += "I" + eqsId.ToString().PadLeft(3, '0');
                }
                eq += ",";
            }
            eq = eq.Substring(0, eq.Length - 1);

            return eq;
        }

        /// <summary>
        /// Serializes items into warrock-readable string
        /// </summary>
        /// <returns>Warrock-readable inventory string</returns>
        public override string ToString()
        {
            string[] eq = Enumerable.Repeat("^", 32).ToArray();
            Func<DS.Item, string> appendSlot = new Func<DS.Item, string>((DS.Item i) =>
            {
                if (i == null)
                    return InventoryDetails.EmptySlot.Code;
                else
                    return String.Format("{0}-3-0-{1}-0-0", i.Code, i.ExpireDate);
            });

            int l = this._characters.Count > eq.Length ? eq.Length : this._characters.Count;
            for (int i = 0; i < l; i++)
                eq[i] = appendSlot(this._characters[i]);

            return String.Join(",", eq);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Checks if item is in the inventory
        /// </summary>
        /// <param name="code">ItemCode of item to be checked</param>
        /// <returns>True if item exists, false if not</returns>
        public bool HasItem(string code)
        {
            return this.GetItem(code) != null;
        }

        /// <summary>
        /// Searches for Item in inventory
        /// </summary>
        /// <param name="code">ItemCode of item to be searched for</param>
        /// <returns>Found item or null</returns>
        public DS.Item GetItem(string code)
        {
            return (from DS.Item i in this._characters where i.Code == code select i).FirstOrDefault();
        }

        #endregion

        #region Create

        /// <summary>
        /// Create an Inventory Item if not exists or updates existing item (+duration) and saves it in the database
        /// </summary>
        /// <param name="itemCode">ItemCode of Item to be created/updated</param>
        /// <param name="duration">Duration if Item in days</param>
        public void Create(string itemCode, int duration)
        {
            DS.Item existing = this.GetItem(itemCode);
            if (existing != null)
            {
                existing.Duration += duration;
                using (DS.Client dbc = QA.GetDBClient())
                {
                    dbc
                        .Command("UPDATE `" + QA.Core.Config.GameConfig.Database + "`.`items` SET `duration` = @duration WHERE `id` = @id")
                        .SetParameter("duration", existing.Duration)
                        .SetParameter("id", existing.Id)
                        .Execute();
                }
            }
            else
            {
                DS.Item newItem = QA.GetDBModel().CreateItem(this.Owner.Session.Account, itemCode, duration);
                this._characters.Add(newItem);
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// Load items form database and split expired items
        /// </summary>
        protected void LoadItems()
        {
            System.Data.DataTable table;
            using (DS.Client dbc = QA.GetDBClient())
            {
                table = dbc
                    .Command("SELECT * FROM `" + QA.Core.Config.GameConfig.Database + "`.`items` WHERE `account_id` = @account_id AND `code` LIKE 'B%';")
                    .SetParameter("account_id", this.Owner.Session.Account.Id)
                    .ReadTable();

                this._characters = (from System.Data.DataRow row in table.Rows select new DS.Item(row)).ToList();
                this.Expired = (from DS.Item item in this._characters where item.IsExpired select item).ToArray();

                using (var trans = dbc.Connection.BeginTransaction())
                {

                    this.Expired.ForEach((DS.Item exp) =>
                    {
                        dbc
                            .Command("DELETE FROM `" + QA.Core.Config.GameConfig.Database + "`.`items` WHERE `id` = @itemId;")
                            .SetParameter("itemId", exp.Id)
                            .Execute();
                    });

                    trans.Commit();
                }

            }

            this._characters.RemoveAll(x => this.Expired.Contains(x));
        }

        #endregion

    }
}
