using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Parity.DS
{
    public class Client : Base.Thread.Lockable
    {

        public Manager @Manager { get; private set; }
        public MySqlConnection Connection { get; private set; }
        public long IdleTime { get; internal set; }

        public override bool Take()
        {
            if (Base.App.Time.IsOlder(this.IdleTime, 28800 * 1000))
                this.Reconnect();
            return base.Take();
        }
        public CommandWrapper Command(string command)
        {
            return new CommandWrapper(this, command);
        }

        protected void Reconnect()
        {
            this.Connection.Close();
            this.Connection.Open();
        }
        public Client(Manager manager)
        {
            this.@Manager = manager;
            this.Connection = this.@Manager.CreateConnection();
            this.IdleTime = Base.App.Time.Get();
            this.Reconnect();
        }

    }
}
