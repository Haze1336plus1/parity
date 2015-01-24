using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.DS
{
    public class Item
    {

        public virtual int Id { get; set; }
        public virtual string Code { get; set; }
        public virtual DateTime BoughtAt { get; set; }
        public virtual int Duration { get; set; }

        public Item(string code)
        {
            this.Id = -1;
            this.Code = code;
        }
        public Item(int id, string code, DateTime boughtAt, int duration)
        {
            this.Id = id;
            this.Code = code;
            this.BoughtAt = boughtAt;
            this.Duration = duration;
        }
        public Item(System.Data.DataRow itemRow)
        {
            this.Id = (int)itemRow["id"];
            this.Code = (string)itemRow["code"];
            this.BoughtAt = (DateTime)itemRow["purchase"];
            this.Duration = (int)itemRow["duration"];
        }

        public string ExpireDate
        {
            get
            {
                return this.BoughtAt.AddDays(this.Duration).ToString("yyMMddHH");
            }
        }
        public bool IsExpired
        {
            get
            {
                return this.BoughtAt.AddDays(this.Duration) < DateTime.Now;
            }
        }

    }
}
