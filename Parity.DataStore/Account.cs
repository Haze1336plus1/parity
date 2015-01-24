using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.DS
{
    public class Account
    {

        public virtual int Id { get; set; }
        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
        public virtual string Nickname { get; set; }
        public virtual string EMail { get; set; }
        public virtual bool Online { get; set; }
        public virtual int Session { get; set; }
        public virtual string MacAddress { get; set; }
        public virtual Base.Enum.AuthLevel AuthLevel { get; set; }

        public virtual Detail Details { get; set; }
        public virtual ISet<Equipment> Equipment { get; set; }
        public virtual ISet<Character> Character { get; set; }

        public Account(Model dbModel, int accountId, bool full = true)
        {
            using (Client selectionClient = dbModel.DSManager.GetClient())
            {
                System.Data.DataTable
                    accountTable = null,
                    detailTable = null,
                    equipmentTable = null,
                    characterTable = null;

                // Account
                accountTable = selectionClient
                    .Command("SELECT * FROM `accounts` AS a WHERE a.account_id = @accountId")
                    .SetParameter("@accountId", accountId)
                    .ReadTable();

                if(accountTable.Rows.Count == 0)
                {
                    this.Id = -1;
                    return;
                }

                accountId = (int)accountTable.Rows[0]["account_id"];

                // Details
                detailTable = selectionClient
                    .Command("SELECT * FROM `" + dbModel.GameDatabase + "`.`details` AS d WHERE d.id = @accountId")
                    .SetParameter("@accountId", accountId)
                    .ReadTable();

                if (full)
                {
                    // Equipment
                    equipmentTable = selectionClient
                        .Command("SELECT * FROM `" + dbModel.GameDatabase + "`.`equipment` AS d WHERE d.account_id = @accountId")
                        .SetParameter("@accountId", accountId)
                        .ReadTable();
                    // Character
                    characterTable = selectionClient
                        .Command("SELECT * FROM `" + dbModel.GameDatabase + "`.`character` AS d WHERE d.account_id = @accountId")
                        .SetParameter("@accountId", accountId)
                        .ReadTable();
                }

                if (accountTable.Rows.Count == 1 &&
                    detailTable.Rows.Count == 1)
                {
                    System.Data.DataRow accountRow = accountTable.Rows[0];
                    System.Data.DataRow detailRow = detailTable.Rows[0];
                    this.Id = (int)accountRow["account_id"];
                    this.Username = (string)accountRow["username"];
                    this.Password = (string)accountRow["password"];
                    this.Nickname = (string)accountRow["nickname"];
                    this.EMail = (string)accountRow["email"];
                    this.Online = (bool)accountRow["online"];
                    this.Session = accountRow.IsNull("session") ? 0 : (int)accountRow["session"];
                    this.MacAddress = (string)accountRow["mac_address"];
                    this.AuthLevel = (Base.Enum.AuthLevel)accountRow["authlevel"];
                    this.Details = new Detail(detailRow);

                    if (full)
                    {
                        this.Equipment = new HashSet<Equipment>(from System.Data.DataRow eTR in equipmentTable.Rows select new Equipment(eTR));
                        this.Character = new HashSet<Character>(from System.Data.DataRow cTR in characterTable.Rows select new Character(cTR));
                    }
                    else
                    {
                        this.Equipment = new HashSet<Equipment>();
                        this.Character = new HashSet<Character>();
                    }
                }
            }
        }

        /// <summary>
        /// Create a new Session and update Database
        /// </summary>
        /// <returns>The just created Session</returns>
        public Base.Layout.AccountSession CreateSession(Model databaseModel)
        {
            if(this.Online)
                return default(Base.Layout.AccountSession);
            Base.Layout.AccountSession newSession = Base.Layout.AccountSession.CreateSession(this.Id);
            this.Session = newSession.SessionKey;
            databaseModel.UpdateSession(newSession);
            return newSession;
        }

    }
}
