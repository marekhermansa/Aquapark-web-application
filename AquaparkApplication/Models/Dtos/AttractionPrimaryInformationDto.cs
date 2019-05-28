namespace AquaparkApplication.Models.Dtos
{
    public class AttractionPrimaryInformationDto
    {
        public int AttractionId { get; set; }
        public string Name { get; set; }
        public int AmountOfPeople { get; set; }
        public double OccupancyRatio { get; set; }
    }
}