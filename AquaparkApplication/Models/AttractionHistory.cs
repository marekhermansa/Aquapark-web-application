using System;
using System.ComponentModel.DataAnnotations;

namespace AquaparkApplication.Models
{
    public class AttractionHistory
    {
        public int Id { get; set; }
        [Required]
        public Attraction Attraction { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        public DateTime? FinishTime { get; set; }
        [Required]
        public Position Position { get; set; }
    }
}