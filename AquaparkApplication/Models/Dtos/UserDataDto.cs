using System.ComponentModel.DataAnnotations;

namespace AquaparkApplication.Models.Dtos
{
    public class UserDataDto
    {
        [StringLength(30)]
        public string Email { get; set; }
        [StringLength(30)]
        public string Name { get; set; }
        [StringLength(30)]
        public string Surname { get; set; }
    }
}