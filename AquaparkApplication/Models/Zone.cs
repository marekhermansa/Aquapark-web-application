using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AquaparkApplication.Models
{
    public class Zone
    {
        public int Id { get; set; }
        [StringLength(30)]
        public string Name { get; set; }
        public double MaxAmountOfPeople { get; set; }
        public virtual ICollection<Attraction> Attractions { get; set; }
    }
}