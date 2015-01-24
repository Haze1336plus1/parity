using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.DS
{
    public class Model
    {

        public Manager DSManager { get; private set; }
        public string GameDatabase { get; private set; }

        public void SetGameDatabase(string gameDatabase)
        {
            this.GameDatabase = gameDatabase;
        }
        public Model(Manager dataSourceManager)
        {
            this.DSManager = dataSourceManager;
            this.SetGameDatabase(this.DSManager.Configuration.Database);
        }

        public bool IsMacAddressBanned(string macAddress)
        {
            bool isBanned = false;
            using (Client selectionClient = this.DSManager.GetClient())
            {
                isBanned = int.Parse(selectionClient
                    .Command("SELECT COUNT(*) FROM `mac_bans` AS mb WHERE mb.mac = @macAddress LIMIT 0,1;")
                    .SetParameter("@macAddress", macAddress)
                    .ExecuteScalar().ToString()) == 1;
            }
            return isBanned;
        }

        public Account Account(int accountId, bool full = true)
        {
            Account retAcc = new Account(this, accountId, full);
            if (retAcc.Id == 0)
                return null;
            return retAcc;
        }

        public Account Account(string username, bool full = true)
        {
            Account retAccount = null;
            using (Client selectionClient = this.DSManager.GetClient())
            {
                int? accountId = (int?)selectionClient
                    .Command("SELECT a.account_id FROM `accounts` AS a WHERE a.username = @username")
                    .SetParameter("@username", username)
                    .ExecuteScalar();

                if (accountId != null)
                    retAccount = this.Account((int)accountId, full);
            }
            return retAccount;
        }

        public void UpdateSession(Base.Layout.AccountSession updatedSession)
        {
            using (Client updateClient = this.DSManager.GetClient())
            {
                updateClient
                    .Command("UPDATE `accounts` AS a SET a.session = @sessionId WHERE a.account_id = @accountId")
                    .SetParameter("@sessionId", updatedSession.SessionKey)
                    .SetParameter("@accountId", updatedSession.AccountId)
                    .Execute();
            }
        }

        public void GoOnline(Account currentAccount, int serverId)
        {
            using (Client updateClient = this.DSManager.GetClient())
            {
                updateClient
                     .Command("UPDATE `accounts` AS a SET a.online = @server_id, a.session = NULL WHERE a.account_id = @accountId")
                     .SetParameter("@server_id", serverId)
                     .SetParameter("@accountId", currentAccount.Id)
                     .Execute();
            }
            currentAccount.Online = true;
            currentAccount.Session = 0;
        }

        public Item CreateItem(Account owner, string code, int duration)
        {
            int itemId = 0;
            DateTime now = DateTime.Now;
            using (Client creationClient = this.DSManager.GetClient())
            {
                itemId = int.Parse(
                   creationClient
                       .Command("INSERT INTO `" + this.GameDatabase + "`.`items` (`account_id`, `code`, `purchase`, `duration`) VALUES (@accountId, @code, @purchase, @duration); SELECT LAST_INSERT_ID();")
                       .SetParameter("@accountId", owner.Id)
                       .SetParameter("@code", code)
                       .SetParameter("@purchase", now.ToString("yyyy-MM-dd HH:mm:ss"))
                       .SetParameter("@duration", duration)
                       .ExecuteScalar()
                       .ToString());
            }
            return new Item(itemId, code, DateTime.Now, duration);
        }

        public OutboxItem CreateOutboxItem(Account owner, string code, int duration, string from)
        {
            int itemId = 0;
            DateTime now = DateTime.Now;
            using (Client creationClient = this.DSManager.GetClient())
            {
                itemId = int.Parse(
                    creationClient
                        .Command("INSERT INTO `outbox` (`account_id`, `code`, `purchase`, `duration`, `from`) VALUES (@accountId, @code, @purchase, @duration, @from); SELECT LAST_INSERT_ID();")
                        .SetParameter("@accountId", owner.Id)
                        .SetParameter("@code", code)
                        .SetParameter("@purchase", now.ToString("yyyy-MM-dd HH:mm:ss"))
                        .SetParameter("@duration", duration)
                        .SetParameter("@from", from)
                        .ExecuteScalar()
                        .ToString());
            }
            return new OutboxItem(itemId, code, DateTime.Now, duration, from);
        }

    }
}
