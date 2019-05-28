using System;
using System.Collections.Generic;

namespace AquaparkApplication.Models
{
    public sealed class PeriodicDiscount
    {
        public PeriodicDiscount()
        {
            Tickets = new HashSet<Ticket>();
        }
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public decimal Value { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}