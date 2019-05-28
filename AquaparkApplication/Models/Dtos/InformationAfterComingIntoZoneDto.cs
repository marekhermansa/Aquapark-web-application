namespace AquaparkApplication.Models.Dtos
{
    public class InformationAfterComingIntoZoneDto
    {
        public bool Success { get; set; }
        public string Status { get; set; }
        public int? ZoneId { get; set; }
        public int? PositionId { get; set; }

    }
}