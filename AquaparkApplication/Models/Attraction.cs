using System.ComponentModel.DataAnnotations;

namespace AquaparkApplication.Models
{
    public class Attraction
    {
        public int Id { get; set; }
        [StringLength(30)]
        public string Name { get; set; }
        public double MaxAmountOfPeople { get; set; }
        public Zone Zone { get; set; }
    }
}