using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.GameDetails.Map
{
    public class Details
    {

        public int ID { get; private set; }
        public string Name { get; private set; }

        public ChannelRestriction Channel { get; private set; }
        public GameModeRestriction GameMode { get; private set; }

        public Base.Enum.Premium Premium { get; private set; }

        public double ExperienceUp { get; private set; }
        public bool IsNew { get; private set; }
        public bool IsEvent { get; private set; }

        public Object.Bomb[] ObjectBombs { get; private set; }
        public Object.Entity[] ObjectEntities { get; private set; }
        public Object.Flag[] ObjectFlags { get; private set; }

        public void UpdateEntities(Object.EntityDetail[] EntityDetails)
        {
            this.ObjectEntities = EntityDetails;
        }

        public Details(int ID, string Name, ChannelRestriction Channel, GameModeRestriction GameMode, Base.Enum.Premium Premium, double ExperienceUp, bool IsNew, bool IsEvent, Object.Base[] MapObjects)
        {
            this.ID = ID;
            this.Name = Name;
            this.Channel = Channel;
            this.GameMode = GameMode;
            this.Premium = Premium;
            this.ExperienceUp = ExperienceUp;
            this.IsNew = IsNew;
            this.IsEvent = IsEvent;
            this.ObjectBombs = (from Object.Base iObject in MapObjects where iObject is Object.Bomb select iObject as Object.Bomb).ToArray();
            this.ObjectEntities = (from Object.Base iObject in MapObjects where iObject is Object.Entity select iObject as Object.Entity).ToArray();
            this.ObjectFlags = (from Object.Base iObject in MapObjects where iObject is Object.Flag select iObject as Object.Flag).ToArray();
        }

    }
}
