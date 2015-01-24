using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.GameDetails.Map.Object
{
    public class EntityDetail : Entity
    {

        public int MaxHealth { get; private set; }
        public Parity.Base.Layout.VehicleSeat[] Seats { get; private set; }

        public EntityDetail(int X, int Y, int Z, string Code, string Target, int SpawnInterval, int MaxHealth, Parity.Base.Layout.VehicleSeat[] Seats)
            : base(X, Y, Z, Code, Target, SpawnInterval)
        {
            this.Seats = Seats;
            this.MaxHealth = MaxHealth;
        }

    }
}
