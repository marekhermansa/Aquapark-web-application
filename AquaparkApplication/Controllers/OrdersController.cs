using System;
using System.Collections.Generic;
using System.Linq;
using AquaparkApplication.Models;
using AquaparkApplication.Models.Dtos;
using AquaparkApplication.Models.PassedParameters;
using AquaparkSystemApi.Exceptions;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AquaparkApplication.Controllers
{
    [EnableCors("AllowMyOrigin")]
    public class OrdersController : Controller
    {
        private AquaparkDbContext _dbContext;

        public OrdersController(AquaparkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [AcceptVerbs("POST")]
        [ActionName("MakeNewOrder")]
        public OrderDto MakeNewOrder([FromBody]NewOrder newOrder)
        {
            bool success = false;
            string status = "Wrong token";
            OrderDto orderDto = new OrderDto()
            {
                Success = success,
                Status = status
            };

            try
            {
                int userId;
                if (newOrder.UserToken == string.Empty || AquaparkSystemApi.Security.Security.UserTokens.Any(i => i.Value == newOrder.UserToken))
                {
                    User user = null;
                    if (newOrder.UserToken != string.Empty)
                    {
                        userId = AquaparkSystemApi.Security.Security.UserTokens.FirstOrDefault(i => i.Value == newOrder.UserToken).Key;
                        user = _dbContext.Users.Include(i=> i.Orders).FirstOrDefault(i => i.Id == userId);
                        if (user == null)
                            throw new UserNotFoundException("There is no user with given data.");
                    }
                    

                    Order order = new Order()
                    {
                        DateOfOrder = DateTime.Now,
                        UserData = new UserData()
                        {
                            Email = newOrder.UserData.Email,
                            Name = newOrder.UserData.Name,
                            Surname = newOrder.UserData.Surname
                        }
                    };
                    if (user == null)
                    {
                        _dbContext.Orders.Add(order);
                    }
                    else
                    {
                        user.Orders.Add(order);
                    }

                    List<Position> positionsToOrder = new List<Position>();
                    foreach (var item in newOrder.TicketsWithClassDiscounts)
                    {
                        for (int j = 0; j < item.NumberOfTickets; j++)
                        {
                            Position position = new Position()
                            {
                                
                                SocialClassDiscount =
                                    _dbContext.SocialClassDiscounts.FirstOrDefault(i => i.Id == item.SocialClassDiscountId),
                                Ticket = _dbContext.Tickets.Include(i => i.Zone)
                                    .FirstOrDefault(i => i.Id == item.TicketTypeId),
                                PeriodicDiscount = _dbContext.PeriodicDiscounts.FirstOrDefault(i =>
                                    i.StartTime >= DateTime.Now &&
                                    i.FinishTime <= DateTime.Now),
                                CanBeUsed = true
                            };
                            positionsToOrder.Add(position);
                            order.Positions.Add(position);
                        }
                    }

                    _dbContext.SaveChanges();

                    success = true;
                    status = "";
                    orderDto.Status = status;
                    orderDto.Success = success;
                    orderDto.Tickets = positionsToOrder.Select(i => new TicketDto()
                    {
                        Id = i.Id,
                        Name = i.Ticket.Name,
                        Number = positionsToOrder.Count(j => j.Ticket.Id == i.Ticket.Id),
                        Price = i.Ticket.Price,
                        Zone = new ZoneWithAttractionsInformationDto()
                        {
                            ZoneId = i.Ticket.Zone.Id,
                            Name = i.Ticket.Zone.Name,
                            Attractions = _dbContext.Attractions.Where(j => j.Zone.Id == i.Ticket.Zone.Id).Select(j =>
                                new AttractionPrimaryInformationDto()
                                {
                                    AttractionId = j.Id,
                                    Name = j.Name
                                })
                        }
                    });
                    orderDto.OrderId = order.Id;
                }
            }
            catch (Exception ex)
            {
                orderDto.Status = ex.Message;
                orderDto.Success = false;
            }

            return orderDto;
        }
    }
}
