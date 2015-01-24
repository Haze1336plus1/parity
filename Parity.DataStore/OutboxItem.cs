using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.DS
{
    public class OutboxItem
    {

        public virtual int Id { get; set; }
        public virtual string Code { get; set; }
        public virtual DateTime BoughtAt { get; set; }
        public virtual int Duration { get; set; }
        public virtual string From { get; set; }

        public OutboxItem(string code)
        {
            this.Id = -1;
            this.Code = code;
        }
        public OutboxItem(int id, string code, DateTime boughtAt, int duration, string from)
        {
            this.Id = id;
            this.Code = code;
            this.BoughtAt = boughtAt;
            this.Duration = duration;
            this.From = from;
        }
        public OutboxItem(System.Data.DataRow itemRow)
        {
            this.Id = (int)itemRow["id"];
            this.Code = (string)itemRow["code"];
            this.BoughtAt = (DateTime)itemRow["purchase"];
            this.Duration = (int)itemRow["duration"];
            this.From = (string)itemRow["from"];
        }
    }
}
