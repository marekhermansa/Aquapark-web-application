using System;
using System.Linq;
using AquaparkApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AquaparkApplication
{
    public class AquaparkDbContext : DbContext
    {
        public AquaparkDbContext(DbContextOptions<AquaparkDbContext> options)
            : base(options)
        { }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Attraction> Attractions { get; set; }
        public DbSet<AttractionHistory> AttractionHistories { get; set; }
        public DbSet<PeriodicDiscount> PeriodicDiscounts { get; set; }
        public DbSet<SocialClassDiscount> SocialClassDiscounts { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<ZoneHistory> ZoneHistories { get; set; }
        public DbSet<UserData> UsersData { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            Zone zone1 = new Zone()
            {
                Id = 1,
                Name = "Strefa saun",
                MaxAmountOfPeople = 35.0
            };
            Zone zone2 = new Zone()
            {
                Id = 2,
                Name = "Strefa basenów",
                MaxAmountOfPeople = 100.0
            };
            Zone zone3 = new Zone()
            {
                Id = 3,
                Name = "Strefa spa",
                MaxAmountOfPeople = 20.0
            };
            builder.Entity<Zone>().HasData(zone1, zone2, zone3);
                
                
                
           builder.Entity<Attraction>().HasData(
                new
                {
                    Id = 1,
                    Name = "Sauna 1",
                    ZoneId = 1,
                    MaxAmountOfPeople = 5.0
                },
                new
                {
                    Id = 2,
                    Name = "Sauna 2",
                    ZoneId = 1,
                    MaxAmountOfPeople = 5.0
                },
                new
                {
                    Id = 3,
                    Name = "Sauna 3",
                    ZoneId = 1,
                    MaxAmountOfPeople = 5.0
                },
                new
                {
                    Id = 4,
                    Name = "Sauna 4",
                    ZoneId = 1,
                    MaxAmountOfPeople = 7.0
                },
                new
                {
                    Id = 5,
                    Name = "Sauna 5",
                    ZoneId = 1,
                    MaxAmountOfPeople = 3.0
                },
                new
                {
                    Id = 6,
                    Name = "Sauna 6",
                    ZoneId = 1,
                    MaxAmountOfPeople = 2.0
                },
                new
                {
                    Id = 7,
                    Name = "Sauna 7",
                    ZoneId = 1,
                    MaxAmountOfPeople = 8.0
                },
                new
                {
                    Id = 8,
                    Name = "Basen 1",
                    ZoneId = 2,
                    MaxAmountOfPeople = 25.0
                },
                new
                {
                    Id = 9,
                    Name = "Basen 2",
                    ZoneId = 2,
                    MaxAmountOfPeople = 25.0
                },
                new
                {
                    Id = 10,
                    Name = "Basen 3",
                    ZoneId = 2,
                    MaxAmountOfPeople = 30.0
                },
                new
                {
                    Id = 11,
                    Name = "Basen 4",
                    ZoneId = 2,
                    MaxAmountOfPeople = 20.0
                },
                new 
                {
                    Id = 12,
                    Name = "Spa 1",
                    ZoneId = 3,
                    MaxAmountOfPeople = 5.0
                },
                new
                {
                    Id = 13,
                    Name = "Spa 2",
                    ZoneId = 3,
                    MaxAmountOfPeople = 5.0
                },
                new{
                    Id = 14,
                    Name = "Spa 3",
                    ZoneId = 3,
                    MaxAmountOfPeople = 10.0
                }
            );

           PeriodicDiscount periodicDiscount1 = new PeriodicDiscount()
           {
               Id = 1,
               FinishTime = new DateTime(2019, 5, 29),
               StartTime = new DateTime(2019, 5, 1),
               Value = 0.80M
           };
            builder.Entity<PeriodicDiscount>().HasData(periodicDiscount1);

            SocialClassDiscount classDiscount1 = new SocialClassDiscount()
            {
                Id = 1,
                SocialClassName = "Emeryt 50%",
                Value = 0.50M
            };
            SocialClassDiscount classDiscount2 = new SocialClassDiscount()
            {
                Id = 2,
                SocialClassName = "Student 80%",
                Value = 0.20M
            };
            SocialClassDiscount classDiscount3 = new SocialClassDiscount()
            {
                Id = 3,
                SocialClassName = "Weteran 25%",
                Value = 0.75M
            };
            SocialClassDiscount classDiscount4 = new SocialClassDiscount()
            {
                Id = 4,
                SocialClassName = "Dziecko 10%",
                Value = 0.90M
            };
            SocialClassDiscount classDiscount5 = new SocialClassDiscount()
            {
                Id = 5,
                SocialClassName = "Normalny 100%",
                Value = 0.00M
            };
            builder.Entity<SocialClassDiscount>().HasData(classDiscount1, classDiscount2, classDiscount3, classDiscount4, classDiscount5);

            
            builder.Entity<Ticket>().HasData(
            new
            {
                Id = 1,
                Name = "Basen - Bilet poranny 6:00-12:00",
                Price = 30.00M,
                ZoneId = 2,
                StartHour = 6.00,
                EndHour = 12.00,
                Days = 1,
                Months = 0
            },
            new
            {
                Id = 2,
                Name = "Basen - Bilet poranny 12:00-18:00",
                Price = 35.00M,
                ZoneId = 2,
                StartHour = 12.00,
                EndHour = 18.00,
                Days = 1,
                Months = 0
            },
            new
            {
                Id = 3,
                Name = "Basen - Bilet poranny 18:00-24:00",
                Price = 40.00M,
                ZoneId = 2,
                StartHour = 18.00,
                EndHour = 24.00,
                Days = 1,
                Months = 0
            },
            new
            {
                Id = 4,
                Name = "Basen - Bilet całodniowy",
                Price = 60.00M,
                ZoneId = 2,
                StartHour = 0.00,
                EndHour = 24.00,
                Days = 1,
                Months = 0
            },
            new
            {
                Id = 5,
                Name = "Sauna - Bilet całodniowy",
                Price = 80.00M,
                ZoneId = 1,
                StartHour = 0.00,
                EndHour = 24.0,
                Days = 1,
                Months = 0
            },
             new 
            {
                Id = 6,
                Name = "Spa - Bilet całodniowy",
                Price = 200.00M,
                ZoneId = 3,
                StartHour = 0.00,
                EndHour = 24.00,
                Days = 1,
                Months = 0
            });
        }
    }
}
