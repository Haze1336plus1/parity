using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Game.Client
{
    public class InventoryDetails
    {

        public readonly Client Owner;
        protected List<DS.Item> _items;
        protected List<DS.OutboxItem> _outbox;
        public DS.Item[] Items { get { return this._items.ToArray(); } }
        public DS.Item[] Expired { get; private set; }
        public DS.OutboxItem[] Outbox { get { return this._outbox.ToArray(); } }
        public DS.Item[][] Equipment { get; private set; }
        public static readonly DS.Item EmptySlot;

        #region Inventory

        static InventoryDetails()
        {
            InventoryDetails.EmptySlot = new DS.Item("^");
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
        }

        /// <summary>
        /// Unequip the itemcode from Battleclass and set default if necessary
        /// </summary>
        /// <param name="item">Item code to be unequipped</param>
        /// <param name="battleClass">Class to unequip item from</param>
        /// <param name="default">Set default or not</param>
        public void Unequip(string item, Base.Enum.BattleClass battleClass, bool @default = false)
        {
            for (byte i = 0; i < 9; i++)
            {
                DS.Item eqItem = this.Equipment[(int)battleClass][i];
                if (eqItem != null && eqItem.Code == item)
                {
                    if (@default || i == 0)
                        this.Equipment[(int)battleClass][i] = Modules.Defaults.GetEquipment(battleClass, i);
                    else
                        this.Equipment[(int)battleClass][i] = null;
                }
            }
        }

        /// <summary>
        /// Crate new InventoryDetails instance and loads it
        /// </summary>
        /// <param name="owner">Reference to Owner Client</param>
        public InventoryDetails(Client owner)
        {
            this.Owner = owner;
            this.LoadItems();
            this.UpdateOutbox();
            this.Equipment = new DS.Item[5][];
            for (int i = 0; i < this.Equipment.Length; i++)
                this.Equipment[i] = new DS.Item[9];
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
                    dbc
                        .Command("UPDATE `" + QA.Core.Config.GameConfig.Database + "`.`equipment` AS e SET e.slot1 = @slot1, e.slot2 = @slot2, e.slot3 = @slot3, e.slot4 = @slot4, e.slot5 = @slot5, e.slot6 = @slot6, e.slot7 = @slot7, e.slot8 = @slot8, e.slot6change = @slot6change WHERE e.class = @class AND e.account_id = @account_id LIMIT 1;")
                        .SetParameter("slot1", this.GetDatabaseId(battleClass, eq[0]))
                        .SetParameter("slot2", this.GetDatabaseId(battleClass, eq[1]))
                        .SetParameter("slot3", this.GetDatabaseId(battleClass, eq[2]))
                        .SetParameter("slot4", this.GetDatabaseId(battleClass, eq[3]))
                        .SetParameter("slot5", this.GetDatabaseId(battleClass, eq[4]))
                        .SetParameter("slot6", this.GetDatabaseId(battleClass, eq[5]))
                        .SetParameter("slot7", this.GetDatabaseId(battleClass, eq[6]))
                        .SetParameter("slot8", this.GetDatabaseId(battleClass, eq[7]))
                        .SetParameter("slot6change", this.GetDatabaseId(battleClass, eq[8]))
                        .SetParameter("class", (int)battleClass)
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
                DS.Equipment eq = this.Owner.Session.Account.Equipment.ElementAt(i);
                for (byte j = 0; j < 8; j++)
                {
                    DS.Item citem = (from DS.Item iitem in this.Items where iitem.Id == eq.Slots[j] select iitem).FirstOrDefault();
                    this.Equipment[i][j] = Base.Decision.NotNull(citem, Modules.Defaults.GetEquipment((Base.Enum.BattleClass)i, j));
                }

                DS.Item chgitem = (from DS.Item iitem in this.Items where iitem.Id == eq.Slot6Change select iitem).FirstOrDefault();
                this.Equipment[i][8] = Base.Decision.NotNull(chgitem, Modules.Defaults.GetEquipment((Base.Enum.BattleClass)i, 8));
            }
        }

        /// <summary>
        /// Get index of item in inventory
        /// </summary>
        /// <param name="item">Item to be looked for</param>
        /// <returns>Index of item in _items</returns>
        public int GetEquipmentId(DS.Item item)
        {
            return this._items.IndexOf(item);
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
            else if (Modules.Defaults.IsDefaultEquipment(battleClass, item))
                return 0;
            return -1;
        }

        /// <summary>
        /// Remove the OutboxItem from local list and drop it from database
        /// </summary>
        /// <param name="obItem">OutboxItem to be deleted</param>
        public void OutboxDelete(DS.OutboxItem obItem)
        {
            this._outbox.Remove(obItem);
            using (DS.Client dbc = QA.GetDBClient())
            {
                dbc
                    .Command("DELETE FROM `outbox` WHERE `id` = @obId LIMIT 1;")
                    .SetParameter("obId", obItem.Id)
                    .Execute();
            }
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

            this._items.Remove(item);
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
            for (int j = 0; j < 8; j++)
            {
                DS.Item eqs = this.Equipment[(int)battleClass][j];
                if(eqs == null)
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
            string[] eq = Enumerable.Repeat("^", this.Limit).ToArray();
            Func<DS.Item, string> appendSlot = new Func<DS.Item, string>((DS.Item i) =>
            {
                if (i == null)
                    return InventoryDetails.EmptySlot.Code;
                else
                    return String.Format("{0}-1-0-{1}-0-0-0-0-0", i.Code, i.ExpireDate);
            });

            int l = this._items.Count > eq.Length ? eq.Length : this._items.Count;
            for (int i = 0; i < l; i++)
                eq[i] = appendSlot(this._items[i]);

            return String.Join(",", eq);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Check if slot is active and useable
        /// </summary>
        /// <param name="slot">Slot (indexed) to be checked</param>
        /// <returns>True if slot is active and useable, false if not</returns>
        public bool SlotAllowed(byte slot)
        {
            if (slot < 4) return true;
            else if (slot == 4) return this.HasItem("CA01");
            else if (slot == 5) return (from iItem in this._items where iItem.Code[1] == 'S' || iItem.Code[1] == 'U' select true).FirstOrDefault() || this.HasItem("CA02");
            else if (slot == 6) return this.HasItem("CA03");
            else if (slot == 7) return this.HasItem("CA04");
            return false;
        }

        /// <summary>
        /// Serializes allowed Slots (Slot 5-8) into a warrock-readable string (B,B,B,B where B is T or F as boolean)
        /// </summary>
        public string SlotCode
        {
            get
            {
                return String.Join(",", new string[] {
                    this.SlotAllowed(4) ? "T" : "F",
                    this.SlotAllowed(5) ? "T" : "F",
                    this.SlotAllowed(6) ? "T" : "F",
                    this.SlotAllowed(7) ? "T" : "F"
                });
            }
        }

        /// <summary>
        /// Limit of items in inventory
        /// </summary>
        public int Limit
        {
            get
            {
                return (int)Base.Compile.GameDefaults["Inventory.Limit"] +
                    (this._items.Count(x => x.Code == "CB99") * // CHG_INVENEXTEND
                    (int)Base.Compile.GameDefaults["Inventory.ExtendMultiplier"]);
            }
        }

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
        /// Checks if item is in the outbox
        /// </summary>
        /// <param name="code">ItemCode of item to be checked</param>
        /// <returns>True if item exists, false if not</returns>
        public bool HasItemOutbox(string code)
        {
            return this.GetItemOutbox(code) != null;
        }

        /// <summary>
        /// Searches for Item in inventory
        /// </summary>
        /// <param name="code">ItemCode of item to be searched for</param>
        /// <returns>Found item or null</returns>
        public DS.Item GetItem(string code)
        {
            return (from DS.Item i in this._items where i.Code == code select i).FirstOrDefault();
        }

        /// <summary>
        /// Searches for Item in outbox
        /// </summary>
        /// <param name="code">ItemCode of item to be searched for</param>
        /// <returns>Found item or null</returns>
        public DS.OutboxItem GetItemOutbox(string code)
        {
            return (from DS.OutboxItem i in this._outbox where i.Code == code select i).FirstOrDefault();
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
                this._items.Add(newItem);
            }
        }

        /// <summary>
        /// Create an Outbox Item if not exists or updates existing item (+duration) and saves it in the database
        /// </summary>
        /// <param name="itemCode">ItemCode of Item to be created/updated</param>
        /// <param name="duration">Duration if Item in days</param>
        /// <param name="from">Name of existence that ordered the Item</param>
        public void CreateOutbox(string itemCode, int duration, string from)
        {
            DS.OutboxItem existing = (from iItem in this._outbox where iItem.Code == itemCode select iItem).FirstOrDefault();
            if (existing != null)
            {
                existing.Duration += duration;
                using (DS.Client dbc = QA.GetDBClient())
                {
                    dbc
                        .Command("UPDATE `outbox` SET `duration` = @duration WHERE `id` = @id")
                        .SetParameter("duration", existing.Duration)
                        .SetParameter("id", existing.Id)
                        .Execute();
                }
            }
            else
            {
                DS.OutboxItem newItem = QA.GetDBModel().CreateOutboxItem(this.Owner.Session.Account, itemCode, duration, from);
                this._outbox.Add(newItem);
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
                    .Command("SELECT * FROM`" + QA.Core.Config.GameConfig.Database + "`. `items` WHERE `account_id` = @account_id AND `code` NOT LIKE 'B%';")
                    .SetParameter("account_id", this.Owner.Session.Account.Id)
                    .ReadTable();

                this._items = (from System.Data.DataRow row in table.Rows select new DS.Item(row)).ToList();
                this.Expired = (from DS.Item item in this._items where item.IsExpired select item).ToArray();

                using (var trans = dbc.Connection.BeginTransaction())
                {

                    this.Expired.ForEach((DS.Item exp) => {
                        dbc
                            .Command("DELETE FROM `" + QA.Core.Config.GameConfig.Database + "`.`items` WHERE `id` = @itemId;")
                            .SetParameter("itemId", exp.Id)
                            .Execute();
                    });
                    
                    trans.Commit();
                }
            }

            this._items.RemoveAll(x => this.Expired.Contains(x));
        }

        /// <summary>
        /// Load outbox items from database
        /// </summary>
        public void UpdateOutbox()
        {
            System.Data.DataTable table;
            using (DS.Client dbc = QA.GetDBClient())
            {
                table = dbc
                    .Command("SELECT * FROM `outbox` WHERE `account_id` = @account_id;")
                    .SetParameter("account_id", this.Owner.Session.Account.Id)
                    .ReadTable();
            }
            this._outbox = (from System.Data.DataRow row in table.Rows select new DS.OutboxItem(row)).ToList();
        }

        #endregion

    }
}
