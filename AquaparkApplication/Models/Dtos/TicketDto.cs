namespace AquaparkApplication.Models.Dtos
{
    public class TicketDto
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public double StartHour { get; set; }
        public double EndHour { get; set; }
        public int Days { get; set; }
        public int Months { get; set; }
        public ZoneWithAttractionsInformationDto Zone { get; set; }
    }
}