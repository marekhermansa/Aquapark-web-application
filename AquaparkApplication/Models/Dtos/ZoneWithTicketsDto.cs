using System.Collections.Generic;

namespace AquaparkApplication.Models.Dtos
{
    public class ZoneWithTicketsDto
    {
        public int ZoneId { get; set; }
        public string ZoneName { get; set; }
        public IEnumerable<TicketWithPeriodDiscountDto> TicketTypes { get; set; }
    }
}