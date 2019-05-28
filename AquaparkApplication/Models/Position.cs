using System.Collections.Generic;

namespace AquaparkApplication.Models
{
    public class Position
    {

        public int Id { get; set; }
        public Ticket Ticket { get; set; }
        public PeriodicDiscount PeriodicDiscount { get; set; }
        public SocialClassDiscount SocialClassDiscount { get; set; }
        public bool CanBeUsed { get; set; }
        public virtual ICollection<AttractionHistory> AttractionHistories { get; set; }
        public virtual ICollection<ZoneHistory> ZoneHistories { get; set; }
    }
}