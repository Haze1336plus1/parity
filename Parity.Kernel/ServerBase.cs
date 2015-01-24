using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Kernel
{
    public abstract class ServerBase
    {

        public DS.Manager DSManager { get; private set; }
        public DS.Model DSModel { get; private set; }

        protected ServerBase(DS.Configuration databaseConfiguration)
        {
            this.DSManager = new DS.Manager(databaseConfiguration);
            this.DSModel = new DS.Model(this.DSManager);
            this.DSModel.SetGameDatabase(databaseConfiguration.GameDatabase);
        }

        public virtual void Start()
        {
            // nothing special yet
        }

    }
}
