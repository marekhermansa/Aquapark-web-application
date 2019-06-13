using AquaparkSystemApi.Models.Dtos;

namespace AquaparkApplication.Models.Dtos
{
    public class TicketWithPeriodDiscountDto
    {
        public int TicketTypeId { get; set; }
        public string TicketTypeName { get; set; }
        public decimal Price { get; set; }
        public double StartHour { get; set; }
        public double EndHour { get; set; }
        public int Days { get; set; }
        public int Months { get; set; }
        public PeriodicDiscountDto PeriodDiscount { get; set; }
    }
}