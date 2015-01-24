using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.GameDetails.Map.Object
{
    public class Entity : Base
    {

        public string Code { get; private set; }
        public string Target { get; private set; }
        public int SpawnInterval { get; private set; }

        public Entity(int X, int Y, int Z, string Code, string Target, int SpawnInterval)
            : base(X, Y, Z)
        {
            this.Code = Code;
            this.Target = Target;
            this.SpawnInterval = SpawnInterval;
        }

    }
}
