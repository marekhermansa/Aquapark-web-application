using System.Collections.Generic;

namespace AquaparkApplication.Models.Dtos
{
    public class ZoneWithAttractionsInformationDto
    {
        public int ZoneId { get; set; }
        public string Name { get; set; }
        public int AmountOfPeople { get; set; }
        public double OccupancyRatio { get; set; }
        public IEnumerable<AttractionPrimaryInformationDto> Attractions { get; set; }
    }
}