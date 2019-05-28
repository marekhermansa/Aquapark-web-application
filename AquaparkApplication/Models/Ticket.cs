using System.ComponentModel.DataAnnotations;

namespace AquaparkApplication.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Zone Zone { get; set; }
        public PeriodicDiscount PeriodicDiscount { get; set; }
        public double StartHour { get; set; }
        public double EndHour { get; set; }
        public int Days { get; set; }
        public int Months { get; set; }
    }
}