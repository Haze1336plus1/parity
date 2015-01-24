using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.GameDetails.WRBin
{
    public class BranchContainer
    {

        public Branch.BattleClass[] Classes { get; private set; }

        public Branch.BattleClass this[Base.Enum.BattleClass @battleClass]
        { 
           get { return this.Classes[(int)@battleClass]; }
        }

        public BranchContainer(System.Xml.XmlDocument branchDocument)
        {
            this.Classes = new Branch.BattleClass[5];
            foreach (Base.Enum.BattleClass @battleClass in Enum.GetValues(typeof(Base.Enum.BattleClass)))
            {
                string battleClassRegion = @battleClass.ToString().ToUpper();
                this.Classes[(int)@battleClass] = new Branch.BattleClass(branchDocument["Document"][battleClassRegion]);
            }
        }

    }
}
