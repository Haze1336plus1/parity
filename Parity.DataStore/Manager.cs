using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Parity.DS
{
    public class Manager
    {

        protected List<Client> ClientList { get; private set; }
        public Configuration @Configuration { get; private set; }
        public string ConnectionString { get; private set; }

        public Client GetClient()
        {
            foreach (Client iClient in this.ClientList)
            {
                if (!iClient.IsTaken && iClient.Take())
                    return iClient;
            }
            return new Client(this); // create a new temporary session
        }

        public Manager(Configuration @configuration)
        {
            this.Configuration = @configuration;
            this.ClientList = new List<Client>();

            this.ConnectionString =
                "Server={0};Port={1};Database={2};Username={3};Password={4}".Process(
                    this.Configuration.Host,
                    this.Configuration.Port,
                    this.Configuration.Database,
                    this.Configuration.Username,
                    this.Configuration.Password
                );

            int poolSize = this.Configuration.PoolSize;
            if (poolSize < 1) poolSize = 1;
            for (int iIndex = 0; iIndex < poolSize; iIndex++)
                this.ClientList.Add(new Client(this));
        }

        public MySqlConnection CreateConnection()
        {
            return new MySqlConnection(this.ConnectionString);
        }

    }
}
