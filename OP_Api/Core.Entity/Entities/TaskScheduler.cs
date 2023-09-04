using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class TaskScheduler : EntitySimple
    {
        public TaskScheduler() { }
        public TaskScheduler(string code,string note, double netTime)
        {
            this.ActionTIme = DateTime.Now;
            this.NextTime = netTime;
            this.Code = this.Name = code;
            this.Note = note;

        }

        public DateTime ActionTIme { get; set; }
        public double NextTime { get; set; }
        public string Note { get; set; }
    }
}
