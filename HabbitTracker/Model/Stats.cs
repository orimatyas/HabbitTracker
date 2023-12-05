using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabbitTracker.Model
{
    public class Stats
    {
        public int MaxQuantity { get; set; }
        public int MinQuantity { get; set; }
        public int AvgQuantity { get; set; }
        public int TotalQuantity { get; set; }
        public int RecordCount { get; set; }
        public string Unit {  get; set; }

    }
}
