using System;
using System.Collections.Generic;
using System.Linq;
using AquaparkApplication.Models;
using AquaparkApplication.Models.Dtos;
using AquaparkSystemApi.Exceptions;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

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
        public IEnumerable<ZoneWithTicketsDto> AddZonesWithTickets(ZoneWithTicketsCollectionDto zoneWithTicketsCollectionDto)
        {
            var zonesWithTickets = zoneWithTicketsCollectionDto.ZonesWithTicketsDto;

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


                    var tickets = new List<Ticket>();
                    zoneWithTicketsCollectionDto
                        .ZonesWithTicketsDto
                        .ToList()
                        .ForEach(z =>
                        {
                            // remove
                            var ticketsToRemove = new List<Ticket>();
                            _dbContext.Tickets.Where(t => t.Zone.Id == z.ZoneId).ToList().ForEach(t =>
                            {
                                if (z.TicketTypes.ToList().SingleOrDefault(x => x.TicketTypeId == t.Id) == null)
                                {
                                    ticketsToRemove.Add(t);
                                }
                            });
                            _dbContext.Tickets.RemoveRange(ticketsToRemove);

                            // add or modify
                            z.TicketTypes
                            .ToList()
                            .ForEach(tt =>
                            {
                                if (_dbContext.Tickets.Where(t => t.Id == tt.TicketTypeId).FirstOrDefault() == null)
                                {
                                    tickets.Add(new Ticket()
                                    {
                                        Id = tt.TicketTypeId,
                                        Name = tt.TicketTypeName,
                                        Price = tt.Price,
                                        Zone = z != null ? _dbContext.Zones.SingleOrDefault(zz => zz.Id == z.ZoneId) : null,
                                        PeriodicDiscount = tt.PeriodDiscount != null ? _dbContext.PeriodicDiscounts.SingleOrDefault(p => p.Id == tt.PeriodDiscount.Id) : null,
                                        StartHour = tt.StartHour,
                                        EndHour = tt.EndHour,
                                        Days = tt.Days,
                                        Months = tt.Months,
                                    });
                                }
                                else
                                {
                                    var ticket = _dbContext.Tickets.SingleOrDefault(t => t.Id == tt.TicketTypeId);
                                    if (ticket != null)
                                    {
                                        ticket.Id = tt.TicketTypeId;
                                        if (ticket.Name != null)
                                        {
                                            ticket.Name = tt.TicketTypeName;
                                        }
                                        ticket.Price = tt.Price;
                                        if (ticket.Zone != null)
                                        {
                                            ticket.Zone = new Zone()
                                            {
                                                Id = z.ZoneId,
                                                Name = z.ZoneName
                                            };
                                        }
                                        if (ticket.PeriodicDiscount != null)
                                        {
                                            ticket.PeriodicDiscount = new PeriodicDiscount()
                                            {
                                                Id = tt.PeriodDiscount.Id,
                                                StartTime = tt.PeriodDiscount.StartTimeDate,
                                                FinishTime = tt.PeriodDiscount.FinishTimeDate,
                                                Value = tt.PeriodDiscount.Value
                                            };
                                        }
                                        ticket.StartHour = tt.StartHour;
                                        ticket.EndHour = tt.EndHour;
                                        ticket.Days = tt.Days;
                                        ticket.Months = tt.Months;

                                        _dbContext.SaveChanges();
                                    }
                                }
                            });
                        }
                        );
                    _dbContext.Tickets.AddRange(tickets);
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
                    AmountOfPeople = _dbContext.ZoneHistories.Count(j=> j.Zone.Id == i.Id && j.FinishTime == null),
                    OccupancyRatio = _dbContext.ZoneHistories.Count(j => j.Zone.Id == i.Id && j.FinishTime == null)/(double)i.MaxAmountOfPeople,
                    Attractions = _dbContext.Attractions.
                        Where(j => j.Zone.Id == i.Id).
                        Select(j => new AttractionPrimaryInformationDto()
                        {
                            AttractionId = j.Id,
                            Name = j.Name,
                            AmountOfPeople = _dbContext.AttractionHistories.Count(k => k.Attraction.Id == j.Id && k.FinishTime == null),
                            OccupancyRatio = _dbContext.AttractionHistories.Count(k => k.Attraction.Id == j.Id && k.FinishTime == null)/(double)j.MaxAmountOfPeople
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