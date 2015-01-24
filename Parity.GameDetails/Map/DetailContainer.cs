using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.GameDetails.Map
{
    public class DetailContainer
    {

        public Enum MapEnumeration { get; private set; }
        public bool IsDefined(int MapID)
        {
            return Enum.IsDefined(this.MapEnumeration.GetType(), MapID);
        }
        public int GetID(string MapName)
        {
            return (int)Enum.Parse(this.MapEnumeration.GetType(), MapName);
        }
        public Map.Details GetMapDetails(int MapID)
        {
            return (from iMapDetails in this.MapDetails where iMapDetails.ID == MapID select iMapDetails).FirstOrDefault();
        }
        public Map.Details[] MapDetails { get; private set; }

        public DetailContainer(Map.Details[] MapDetails)
        {
            this.MapDetails = MapDetails;

            System.Reflection.Emit.EnumBuilder iEnumBuilder =
                AppDomain.CurrentDomain.DefineDynamicAssembly(new System.Reflection.AssemblyName() { Name = "EnumAsm" }, System.Reflection.Emit.AssemblyBuilderAccess.Run)
                .DefineDynamicModule("EnumModule")
                .DefineEnum("eMapEnumeration", System.Reflection.TypeAttributes.Public, typeof(System.Int32));

            foreach (Map.Details iMapDetail in this.MapDetails)
                iEnumBuilder.DefineLiteral(iMapDetail.Name.Replace(' ', '_'), iMapDetail.ID);

            Type iEnumType = iEnumBuilder.CreateType();
            this.MapEnumeration = (Enum)Activator.CreateInstance(iEnumType);
        }

    }
}
