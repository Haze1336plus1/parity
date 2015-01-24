using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.DS
{
    public class Detail
    {

        public virtual int Id { get; set; }
        public virtual int Kills { get; set; }
        public virtual int Deaths { get; set; }
        public virtual int Experience { get; set; }
        public virtual int Dinar { get; set; }
        public virtual int Credits { get; set; }
        public virtual Base.Enum.Premium Premium { get; set; }
        public virtual DateTime PremiumBegin { get; set; }
        public virtual int PremiumTime { get; set; }

        public Detail(System.Data.DataRow detailRow)
        {
            this.Id = (int)detailRow["id"];
            this.Kills = (int)detailRow["kills"];
            this.Deaths = (int)detailRow["deaths"];
            this.Experience = (int)detailRow["experience"];
            this.Dinar = (int)detailRow["dinar"];
            this.Credits = (int)detailRow["credits"];
            this.Premium = (Base.Enum.Premium)detailRow["premium"];
            this.PremiumBegin = (DateTime)detailRow["premiumbegin"];
            this.PremiumTime = (int)detailRow["premiumduration"];
            if (this.PremiumRemain.TotalSeconds <= 1)
            {
                this.Premium = Base.Enum.Premium.None;
                this.PremiumBegin = DateTime.Now;
                this.PremiumTime = 0;
            }
        }

        public TimeSpan PremiumRemain
        {
            get
            {
                return this.PremiumBegin.AddDays(this.PremiumTime).Subtract(DateTime.Now);
            }
        }

        public virtual Account Owner { get; set; }

        public void Save(Model dbm)
        {
            using (var dbc = dbm.DSManager.GetClient())
            {
                dbc
                    .Command("UPDATE `" + dbm.GameDatabase + "`.`details` AS d SET d.kills = @kills, d.deaths = @deaths, d.experience = @experience, d.dinar = @dinar, d.credits = @credits, d.premium = @premium, d.premiumbegin = @premiumbegin, d.premiumduration = @premiumduration WHERE d.id = @account_id;")
                    .SetParameter("@kills", this.Kills)
                    .SetParameter("@deaths", this.Deaths)
                    .SetParameter("@experience", this.Experience)
                    .SetParameter("@dinar", this.Dinar)
                    .SetParameter("@credits", this.Credits)
                    .SetParameter("@premium", (sbyte)this.Premium)
                    .SetParameter("@premiumbegin", this.PremiumBegin)
                    .SetParameter("@premiumduration", this.PremiumTime)
                    .SetParameter("@account_id", this.Id)
                    .Execute();
            }
        }

    }
}
