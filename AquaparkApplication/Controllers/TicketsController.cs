using System;
using System.Collections.Generic;
using System.Linq;
using AquaparkApplication.Models.Dtos;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AquaparkApplication.Controllers
{
    [EnableCors("AllowMyOrigin")]
    public class TicketsController : Controller
    {
        private AquaparkDbContext _dbContext;

        public TicketsController(AquaparkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [AcceptVerbs("GET")]
        [ActionName("GetAllTickets")]
        public JsonResult GetAllTickets()
        {
            List<TicketDto> ticketDtos = new List<TicketDto>();
            try
            {
                ticketDtos = _dbContext.Tickets.Select(i => new TicketDto()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Price = i.Price,
                    StartHour = i.StartHour,
                    EndHour = i.EndHour,
                    Days = i.Days,
                    Months = i.Months,
                    Zone = new ZoneWithAttractionsInformationDto()
                        {
                            ZoneId = i.Zone.Id,
                            Name = i.Zone.Name,
                            Attractions = _dbContext.Attractions.Where(j => j.Zone.Id == i.Zone.Id).
                                Select(j =>
                                new AttractionPrimaryInformationDto()
                                {
                                    AttractionId = j.Id,
                                    Name = j.Name
                                })
                        }

                }).ToList();
            }
            catch (Exception)
            {
                return new JsonResult(ticketDtos);
            }

            return new JsonResult(ticketDtos);
        }
    }
}
