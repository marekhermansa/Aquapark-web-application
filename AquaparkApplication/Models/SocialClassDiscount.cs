using System.ComponentModel.DataAnnotations;

namespace AquaparkApplication.Models
{
    public class SocialClassDiscount
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        [StringLength(30)]
        public string SocialClassName { get; set; }
    }
}