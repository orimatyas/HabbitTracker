using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabbitTracker.Model
{
    internal class HabitRecord

    {
        public int RecordId { get; set; }
        public int HabitId { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
    }
}
