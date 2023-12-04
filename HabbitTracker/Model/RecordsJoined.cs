using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabbitTracker.Model
{
    public class RecordsJoined
    {
        public int RecordId { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
    }
}
