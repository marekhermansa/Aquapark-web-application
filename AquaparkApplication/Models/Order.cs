using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AquaparkApplication.Models
{
    public sealed class Order
    {
        public Order()
        {
            Positions = new HashSet<Position>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime DateOfOrder { get; set; }
        public ICollection<Position> Positions { get; set; }
        public UserData UserData { get; set; }
        public User User { get; set; }

    }
}