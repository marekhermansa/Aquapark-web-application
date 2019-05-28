using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AquaparkApplication.Models
{
    public class User
    {
        public int Id { get; set; }
        [StringLength(30)]
        public string Email { get; set; }
        [StringLength(40)]
        public string Password { get; set; }
        public Guid UserGuid { get; set; }
        [StringLength(30)]
        public string Name { get; set; }
        [StringLength(30)]
        public string Surname { get; set; }
        public bool IsAdmin { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}