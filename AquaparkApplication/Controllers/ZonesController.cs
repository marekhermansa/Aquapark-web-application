using System;
using System.Collections.Generic;
using System.Linq;
using AquaparkApplication.Models;
using AquaparkApplication.Models.Dtos;
using AquaparkSystemApi.Exceptions;
using AquaparkSystemApi.Models.Dtos;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AquaparkApplication.Controllers
{
    [EnableCors("AllowMyOrigin")]
    public class ZonesController : Controller
    {
        private AquaparkDbContext _dbContext;

        public ZonesController(AquaparkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [AcceptVerbs("POST")]
        [ActionName("AddZonesWithTickets")]
        public IEnumerable<ZoneWithTicketsDto> AddZonesWithTickets([FromBody]ZoneWithTicketsCollectionDto zoneWithTicketsCollectionDto)
        {
            try
            {
                if (AquaparkSystemApi.Security.Security.UserTokens.Any(i => i.Value == zoneWithTicketsCollectionDto.UserToken))
                {
                    var userId = AquaparkSystemApi.Security.Security.UserTokens.FirstOrDefault(i => i.Value == zoneWithTicketsCollectionDto.UserToken).Key;

                    var user = _dbContext.Users.FirstOrDefault(i => i.Id == userId);
                    if (user == null)
                    {
                        throw new UserNotFoundException("There is no user with given data.");
                    }

                    var zones = _dbContext.Zones;
                    var tickets = _dbContext.Tickets
                        .Include(t => t.Zone)
                        .Include(t => t.PeriodicDiscount);
                    var periodicDiscounts = _dbContext.PeriodicDiscounts;
                    var positions = _dbContext.Positions;


                    // remove tickets 

                    tickets = _dbContext.Tickets
                        .Include(t => t.Zone)
                        .Include(t => t.PeriodicDiscount);
                    var ticketsToRemove = new List<Ticket>();
                    var periodicDiscountsToRemove = new List<PeriodicDiscount>();
                    var positionsToUpdate = new List<Position>();

                    tickets.ToList()
                        .ForEach(t =>
                        {
                            var databaseZoneId = t.Zone.Id;
                            var databaseTicketId = t.Id;

                            var matchedTickets = zoneWithTicketsCollectionDto
                            .ZonesWithTicketsDto
                            .ToList()
                            .SingleOrDefault(z => z.ZoneId == databaseZoneId)
                            .TicketTypes
                            .Where(tt => tt.TicketTypeId == databaseTicketId);

                            if (matchedTickets.ToList().Count == 0)
                            {
                                ticketsToRemove.Add(t);

                                // make ticket id null in postiions table
                                var pToUpdate = positions.Where(p=>p.Ticket.Id == t.Id).ToList();
                                pToUpdate.ForEach(p=> 
                                {
                                    p.Ticket = null;
                                    positionsToUpdate.Add(p);
                                });

                                // remove periodic discount 
                                periodicDiscounts.ToList().ForEach(p=>
                                {
                                    if(p.Id == t.PeriodicDiscount.Id)
                                    {
                                        periodicDiscountsToRemove.Add(p);
                                    }
                                });
                            }
                        });

                    _dbContext.UpdateRange(positionsToUpdate);
                    _dbContext.SaveChanges();

                    _dbContext.RemoveRange(ticketsToRemove);
                    _dbContext.SaveChanges();

                    _dbContext.RemoveRange(periodicDiscountsToRemove);
                    _dbContext.SaveChanges();

                    // add tickets

                    zoneWithTicketsCollectionDto.ZonesWithTicketsDto
                        .ToList()
                        .ForEach(z =>
                        {
                            var ticketTypes = z.TicketTypes;
                            ticketTypes.ToList()
                            .ForEach(t =>
                            {
                                var matchedTickets = tickets.ToList()
                                    .SingleOrDefault(tt => tt.Id == t.TicketTypeId);
                                if (matchedTickets == null)
                                {
                                    var newPeriodicDiscount = new PeriodicDiscount();
                                    
                                    if (t.PeriodDiscount != null)
                                    {
                                        newPeriodicDiscount = new PeriodicDiscount()
                                        {
                                            StartTime = t.PeriodDiscount.StartTimeDate,
                                            FinishTime = t.PeriodDiscount.FinishTimeDate,
                                            Value = t.PeriodDiscount.Value
                                        };
                                    }

                                    var newTicket = new Ticket()
                                    {
                                        Name = t.TicketTypeName,
                                        Price = t.Price,
                                        Zone = _dbContext.Zones.SingleOrDefault(zz => zz.Id == z.ZoneId),
                                        PeriodicDiscount = t.PeriodDiscount == null ? null : newPeriodicDiscount, // TODO
                                        StartHour = t.StartHour,
                                        EndHour = t.EndHour,
                                        Days = t.Days,
                                        Months = t.Months
                                    };
                                    _dbContext.Tickets.Add(newTicket);
                                }
                            });
                        });
                    _dbContext.SaveChanges();


                    // update tickets

                    zoneWithTicketsCollectionDto.ZonesWithTicketsDto
                        .ToList()
                        .ForEach(z =>
                        {
                            var ticketTypes = z.TicketTypes;
                            ticketTypes.ToList()
                            .ForEach(t =>
                            {
                                var matchedTicket = tickets.ToList()
                                    .SingleOrDefault(tt => tt.Id == t.TicketTypeId);
                                if (matchedTicket != null)
                                {
                                    matchedTicket.Name = t.TicketTypeName;
                                    matchedTicket.Price = t.Price;
                                    matchedTicket.Zone = _dbContext.Zones.SingleOrDefault(zz => zz.Id == z.ZoneId);
                                    matchedTicket.StartHour = t.StartHour;
                                    matchedTicket.EndHour = t.EndHour;
                                    matchedTicket.Days = t.Days;
                                    matchedTicket.Months = t.Months;

                                    // modify periodic discount
                                    if (t.PeriodDiscount != null)
                                    {
                                        var matchedPeriodicDiscount = periodicDiscounts.ToList().SingleOrDefault(p => p.Id == t.PeriodDiscount.Id);

                                        // add
                                        if (matchedPeriodicDiscount == null)
                                        {
                                            matchedTicket.PeriodicDiscount = new PeriodicDiscount()
                                            {
                                                StartTime = t.PeriodDiscount.StartTimeDate,
                                                FinishTime = t.PeriodDiscount.FinishTimeDate,
                                                Value = t.PeriodDiscount.Value
                                            };
                                        }
                                        // modify
                                        else
                                        {
                                            matchedTicket.PeriodicDiscount.StartTime = t.PeriodDiscount.StartTimeDate;
                                            matchedTicket.PeriodicDiscount.FinishTime = t.PeriodDiscount.FinishTimeDate;
                                            matchedTicket.PeriodicDiscount.Value = t.PeriodDiscount.Value;
                                        }
                                    }
                                    else
                                    {
                                        if (matchedTicket.PeriodicDiscount != null)
                                        {
                                            var periodicDiscountToDelete = periodicDiscounts.SingleOrDefault(p => p.Id == matchedTicket.PeriodicDiscount.Id);
                                            _dbContext.Remove(periodicDiscountToDelete);
                                        }
                                        else
                                        {
                                            matchedTicket.PeriodicDiscount = null;
                                        }
                                    }

                                    _dbContext.Tickets.Update(matchedTicket);
                                }
                            });
                        });
                    _dbContext.SaveChanges();
                }
                else
                {
                    throw new Exception("User identification failed.");
                }
            }
            catch (Exception e)
            {
                return this.GetAllZonesWithTickets();
            }

            return this.GetAllZonesWithTickets();
        }

        [AcceptVerbs("GET")]
        [ActionName("GetAllZones")]
        public IEnumerable<ZonePrimaryInformationDto> GetAllZones()
        {
            List<ZonePrimaryInformationDto> zones = new List<ZonePrimaryInformationDto>();
            try
            {
                zones = _dbContext.Zones.Select(i => new ZonePrimaryInformationDto()
                {
                    ZoneId = i.Id,
                    Name = i.Name
                }).ToList();
            }
            catch (Exception)
            {
                return zones;
            }

            return zones;
        }

        [AcceptVerbs("GET")]
        [ActionName("GetAllZonesWithAttractions")]
        public IEnumerable<ZoneWithAttractionsInformationDto> GetAllZonesWithAttractions()
        {
            List<ZoneWithAttractionsInformationDto> zones = new List<ZoneWithAttractionsInformationDto>();
            try
            {
                zones = _dbContext.Zones.Select(i => new ZoneWithAttractionsInformationDto()
                {
                    ZoneId = i.Id,
                    Name = i.Name,
                    AmountOfPeople = _dbContext.ZoneHistories.Count(j => j.Zone.Id == i.Id && j.FinishTime == null),
                    OccupancyRatio = _dbContext.ZoneHistories.Count(j => j.Zone.Id == i.Id && j.FinishTime == null) / (double)i.MaxAmountOfPeople,
                    Attractions = _dbContext.Attractions.
                        Where(j => j.Zone.Id == i.Id).
                        Select(j => new AttractionPrimaryInformationDto()
                        {
                            AttractionId = j.Id,
                            Name = j.Name,
                            AmountOfPeople = _dbContext.AttractionHistories.Count(k => k.Attraction.Id == j.Id && k.FinishTime == null),
                            OccupancyRatio = _dbContext.AttractionHistories.Count(k => k.Attraction.Id == j.Id && k.FinishTime == null) / (double)j.MaxAmountOfPeople
                        })
                }).ToList();
            }
            catch (Exception w)
            {
                return zones;
            }

            return zones;
        }

        [AcceptVerbs("GET")]
        [ActionName("GetAllZonesWithTickets")]
        public IEnumerable<ZoneWithTicketsDto> GetAllZonesWithTickets()
        {
            List<ZoneWithTicketsDto> ticketDtos = new List<ZoneWithTicketsDto>();
            try
            {
                ticketDtos = _dbContext.Zones.Select(i => new ZoneWithTicketsDto()
                {
                    ZoneId = i.Id,
                    ZoneName = i.Name,
                    TicketTypes = _dbContext.Tickets.Where(j => j.Zone.Id == i.Id).
                        Select(j =>
                            new TicketWithPeriodDiscountDto()
                            {
                                TicketTypeId = j.Id,
                                Price = j.Price,
                                TicketTypeName = j.Name,
                                StartHour = j.StartHour,
                                EndHour = j.EndHour,
                                Days = j.Days,
                                Months = j.Months,
                                PeriodDiscount = j.PeriodicDiscount.StartTime != null ? new PeriodicDiscountDto()
                                {
                                    FinishTimeDate = j.PeriodicDiscount.FinishTime,
                                    Id = j.PeriodicDiscount.Id,
                                    StartTimeDate = j.PeriodicDiscount.StartTime,
                                    Value = j.PeriodicDiscount.Value
                                } : null
                            })

                }).ToList();
            }
            catch (Exception)
            {
                return ticketDtos;
            }

            return ticketDtos;
        }
    }
}