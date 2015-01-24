using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Parity.DS
{
    public class CommandWrapper
    {

        public readonly Client Owner;
        public string Query { get; private set; }
        public Dictionary<string, object> Parameters { get; private set; }

        public CommandWrapper(Client owner, string query)
        {
            this.Owner = owner;
            this.Query = query;
            this.Parameters = new Dictionary<string, object>();
        }

        protected MySqlCommand MakeCommand()
        {
            MySqlCommand command = new MySqlCommand(this.Query, this.Owner.Connection);
            foreach (KeyValuePair<string, object> kp in this.Parameters)
                command.Parameters.AddWithValue(kp.Key, kp.Value);
            return command;
        }

        public CommandWrapper SetParameter(string param, object value)
        {
            this.Parameters.Add(param, value);
            return this;
        }
        public int Execute()
        {
            int retVal = 0;
            using (MySqlCommand command = this.MakeCommand())
            {
                retVal = command.ExecuteNonQuery();
            }
            this.Owner.IdleTime = Base.App.Time.Get();
            return retVal;
        }
        public object ExecuteScalar()
        {
            object retVal = null;
            using (MySqlCommand command = this.MakeCommand())
            {
                retVal = command.ExecuteScalar();
            }
            this.Owner.IdleTime = Base.App.Time.Get();
            return retVal;
        }
        public System.Data.DataTable ReadTable()
        {
            System.Data.DataTable dataTable = new System.Data.DataTable();
            using (MySqlCommand command = this.MakeCommand())
            {
                command.ExecuteNonQuery();
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                {
                    adapter.Fill(dataTable);
                }
            }
            this.Owner.IdleTime = Base.App.Time.Get();
            return dataTable;
        }
    }
}
