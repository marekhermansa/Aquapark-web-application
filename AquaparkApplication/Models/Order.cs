using System;
using System.Collections.Generic;

namespace AquaparkApplication.Models
{
    public sealed class Order
    {
        public Order()
        {
            Positions = new HashSet<Position>();
        }
        public int Id { get; set; }
        public DateTime DateOfOrder { get; set; }
        public ICollection<Position> Positions { get; set; }
        public UserData UserData { get; set; }
        public User User { get; set; }

    }
}