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
    public class EntriesController : Controller
    {
        private AquaparkDbContext _dbContext;

        public EntriesController(AquaparkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [AcceptVerbs("POST")]
        [ActionName("ComeInToZone")]
        public InformationAfterComingIntoZoneDto ComeInToZone(ZoneEntry zoneEntry)
        {
            InformationAfterComingIntoZoneDto result = new InformationAfterComingIntoZoneDto()
            {
                Status = "Something went wrong.",
                Success = false
            };
            try
            {
                bool success = false;
                string status = "";
                double currentHour = DateTime.Now.Hour;
                double currentMinute = DateTime.Now.Minute;
                if (currentMinute > 0)
                    currentHour += 1;
                currentHour %= 24;

                if (AquaparkSystemApi.Security.Security.UserTokens.Any(i => i.Value == zoneEntry.UserToken) || zoneEntry.UserToken == string.Empty || zoneEntry.UserToken == "a")
                {
                    List<Position> positionsAvailableForZone = new List<Position>();
                    int userId = -1;
                    if (zoneEntry.UserToken != string.Empty && zoneEntry.UserToken != "a")
                    {
                        userId = AquaparkSystemApi.Security.Security.UserTokens.FirstOrDefault(i => i.Value == zoneEntry.UserToken).Key;
                        var user = _dbContext.Users.FirstOrDefault(i => i.Id == userId);
                        if (user == null)
                        {
                            throw new UserNotFoundException("There is no user with given data.");
                        }

                        positionsAvailableForZone = _dbContext.Orders.Where(i => i.User.Id == userId).SelectMany(i =>
                            i.Positions.Where(j => j.Ticket.Zone.Id == zoneEntry.ZoneId && j.CanBeUsed &&
                                                   j.Ticket.StartHour <= currentHour &&
                                                   j.Ticket.EndHour >= currentHour)).ToList();
                    }
                    else
                    {
                        positionsAvailableForZone = _dbContext.Orders.Where(i => i.UserData.Email == zoneEntry.Email)
                            .SelectMany(
                                i => i.Positions.Where(j => j.Ticket.Zone.Id == zoneEntry.ZoneId && j.CanBeUsed && 
                                                            j.Ticket.StartHour <= currentHour && j.Ticket.EndHour >= currentHour))
                            .ToList();
                    }


                    if (positionsAvailableForZone.Any(i =>
                        _dbContext.ZoneHistories.Count(j => j.FinishTime == null && j.Id == i.Id) > 0))
                    {

                        throw new Exception("This operation is not allowed. You are already in different zone!");
                    }
                    else if (positionsAvailableForZone.Count == 0)
                    {
                        throw new Exception("You don't have required ticket.");
                    }

                    Position position = positionsAvailableForZone.FirstOrDefault();
                    Zone zoneToEnter = _dbContext.Zones.FirstOrDefault(i => i.Id == zoneEntry.ZoneId);
                    _dbContext.ZoneHistories.Add(new ZoneHistory()
                    {
                        Position = position,
                        StartTime = DateTime.Now,
                        Zone = zoneToEnter
                    });
                    position.CanBeUsed = false;

                    _dbContext.SaveChanges();
                    status = zoneToEnter.Name;
                    success = true;
                    result.Success = success;
                    result.Status = status;
                    result.ZoneId = zoneToEnter.Id;
                    result.PositionId = position.Id;
                }
                else
                {
                    throw new Exception("User identification failed.");
                }
            }
            catch (Exception ex)
            {
                result.Status = ex.Message;
            }

            return result;
        }

        [AcceptVerbs("POST")]
        [ActionName("ComeOutOfZone")]
        public InformationAfterComingOutOfZoneDto ComeOutOfZone(ZoneEntry zoneEntry)
        {
            InformationAfterComingOutOfZoneDto result = new InformationAfterComingOutOfZoneDto()
            {
                Status = "Something went wrong.",
                Success = false
            };
            try
            {

                if (AquaparkSystemApi.Security.Security.UserTokens.Any(i => i.Value == zoneEntry.UserToken) || zoneEntry.UserToken == string.Empty || zoneEntry.UserToken == "a")
                {
                    ZoneHistory positionAvailableForZone;
                    int userId = -1;
                    if (zoneEntry.UserToken != string.Empty && zoneEntry.UserToken != "a")
                    {
                        userId = AquaparkSystemApi.Security.Security.UserTokens.FirstOrDefault(i => i.Value == zoneEntry.UserToken).Key;
                        var user = _dbContext.Users.FirstOrDefault(i => i.Id == userId);
                        if (user == null)
                        {
                            throw new UserNotFoundException("There is no user with given data.");
                        }

                        positionAvailableForZone = _dbContext.Orders.Where(i => i.User.Id == userId)
                            .SelectMany(
                                i => i.Positions.Where(j => j.Ticket.Zone.Id == zoneEntry.ZoneId && !j.CanBeUsed)).SelectMany(i => i.ZoneHistories.Where(k => k.FinishTime == null))
                            .Include(j => j.Position.Ticket)
                            .FirstOrDefault();
                    }
                    else
                    {
                        positionAvailableForZone = _dbContext.Orders.Where(i => i.UserData.Email == zoneEntry.Email)
                            .SelectMany(
                                i => i.Positions.Where(j => j.Ticket.Zone.Id == zoneEntry.ZoneId && !j.CanBeUsed )).SelectMany(i => i.ZoneHistories.Where(k => k.FinishTime == null))
                            .Include(j=> j.Position.Ticket)
                            .FirstOrDefault();
                    }
                    positionAvailableForZone.FinishTime = DateTime.Now;
                    result.Status = "Success!";
                    result.Success = true;

                    // sprawdź czy pozycja jest już zużyta i jeśli tak to wyzeruj ją na false (czyli zostaw) lub jeśli 
                    // będzie mogła być jeszcze użyta to zmień ją na true
                    double currentHour = DateTime.Now.Hour;
                    double currentMinute = DateTime.Now.Minute;
                    if (currentMinute > 0)
                        currentHour += 1;
                    var position = positionAvailableForZone.Position;
                    if (position.Ticket.EndHour > currentHour)
                    {
                        position.CanBeUsed = true;
                    }

                    /// przeszukaj również wszystkie atrakcje i je też pozamykaj?? (dodatkowe zabezpieczenie po prostu)

                    //_dbContext.ZoneHistories.Add(new ZoneHistory()
                    //{
                    //    Position = positionAvailableForZone,
                    //    StartTime = DateTime.Now,
                    //    Zone = _dbContext.Zones.FirstOrDefault(i => i.Id == zoneEntry.ZoneId)
                    //});
                    //positionAvailableForZone.CanBeUsed = false;

                    _dbContext.SaveChanges();
                }
                else
                {
                    throw new Exception("User identification failed.");
                }
            }
            catch (Exception ex)
            {

                result.Status = ex.Message;
                result.Success = false;
            }

            return result;
        }

        [AcceptVerbs("POST")]
        [ActionName("ComeInToAttraction")]
        public void ComeInToAttraction(AttractionEntry attractionEntry)
        {
            InformationAfterComingIntoZoneDto result = new InformationAfterComingIntoZoneDto()
            {
                Status = "Something went wrong.",
                Success = false
            };
            try
            {
                bool success = false;
                string status = "";
                double currentHour = DateTime.Now.Hour;
                double currentMinute = DateTime.Now.Minute;
                if (currentMinute > 0)
                    currentHour += 1;
                currentHour %= 24;

                if (AquaparkSystemApi.Security.Security.UserTokens.Any(i => i.Value == attractionEntry.UserToken)
                    || attractionEntry.UserToken == string.Empty || attractionEntry.UserToken == "a")
                {
                    List<Position> positionsAvailableForZone = new List<Position>();
                    int userId = -1;
                    if (attractionEntry.UserToken != string.Empty && attractionEntry.UserToken != "a")
                    {
                        userId = AquaparkSystemApi.Security.Security.UserTokens.FirstOrDefault(i => i.Value == attractionEntry.UserToken).Key;
                        var user = _dbContext.Users.FirstOrDefault(i => i.Id == userId);
                        if (user == null)
                        {
                            throw new UserNotFoundException("There is no user with given data.");
                        }

                        //positionsAvailableForZone = _dbContext.Orders.Where(i => i.User.Id == userId).SelectMany(i =>
                        //    i.Positions.Where(j => j.Ticket.Zone.Id == attractionEntry.ZoneEntry.ZoneId && !j.CanBeUsed &&
                        //                           j.Ticket.StartHour <= currentHour &&
                        //                           j.Ticket.EndHour >= currentHour)).ToList();
                    }
                    else
                    {
                        //positionsAvailableForZone = _dbContext.Orders.Where(i => i.UserData.Email == attractionEntry.ZoneEntry.Email)
                        //    .SelectMany(
                        //        i => i.Positions.Where(j => j.Ticket.Zone.Id == attractionEntry.ZoneEntry.ZoneId && !j.CanBeUsed &&
                        //                                    j.Ticket.StartHour <= currentHour && j.Ticket.EndHour >= currentHour))
                        //    .ToList();
                    }

                    AttractionHistory attractionHistory = new AttractionHistory()
                    {
                        Position = _dbContext.Positions.FirstOrDefault(i => i.Id == attractionEntry.PositionId),
                        Attraction = _dbContext.Attractions.Include(i => i.Zone).FirstOrDefault(i => i.Id == attractionEntry.AttractionId),
                        StartTime = DateTime.Now
                    };
                    _dbContext.AttractionHistories.Add(attractionHistory);
                    _dbContext.SaveChanges();

                    status = attractionHistory.Attraction.Name;
                    success = true;
                    result.Success = success;
                    result.Status = status;
                    result.ZoneId = attractionHistory.Attraction.Zone.Id;
                    result.PositionId = attractionHistory.Position.Id;
                }
                else
                {
                    throw new Exception("User identification failed.");
                }
            }
            catch (Exception ex)
            {
                result.Status = ex.Message;
            }
        }

        //[AcceptVerbs("POST")]
        //[ActionName("ComeInToAttraction")]
        //public void ComeInToAttraction(ZoneEntry zoneEntry)
        //{

        //    try
        //    {
        //        double currentHour = DateTime.Now.Hour;
        //        double currentMinute = DateTime.Now.Minute;
        //        if (currentMinute > 0)
        //            currentHour += 1;
        //        currentHour %= 24;

        //        if (Security.Security.UserTokens.Any(i => i.Value == zoneEntry.UserToken) || zoneEntry.UserToken == string.Empty || 
        //            attractionEntry.ZoneEntry.UserToken == "a")
        //        {
        //            Position positionAvailableForZone;
        //            int userId = -1;
        //            if (zoneEntry.UserToken != string.Empty)
        //            {
        //                userId = Security.Security.UserTokens.FirstOrDefault(i => i.Value == zoneEntry.UserToken).Key;
        //                var user = _dbContext.Users.FirstOrDefault(i => i.Id == userId);
        //                if (user == null)
        //                {
        //                    throw new UserNotFoundException("There is no user with given data.");
        //                }

        //                positionAvailableForZone = _dbContext.Orders.Where(i => i.User.Id == userId).
        //                    SelectMany(i => i.Positions.Where(j => j.Ticket.Zone.Id == zoneEntry.ZoneId && j.CanBeUsed &&
        //                                                           j.Ticket.StartHour <= currentHour && j.Ticket.EndHour >= currentHour))
        //                    .FirstOrDefault();
        //            }
        //            else
        //            {
        //                positionAvailableForZone = _dbContext.Orders.Where(i => i.UserData.Email == zoneEntry.Email)
        //                    .SelectMany(
        //                        i => i.Positions.Where(j => j.Ticket.Zone.Id == zoneEntry.ZoneId && j.CanBeUsed &&
        //                                                    j.Ticket.StartHour <= currentHour && j.Ticket.EndHour >= currentHour))
        //                    .FirstOrDefault();
        //            }


        //            _dbContext.ZoneHistories.Add(new ZoneHistory()
        //            {
        //                Position = positionAvailableForZone,
        //                StartTime = DateTime.Now,
        //                Zone = _dbContext.Zones.FirstOrDefault(i => i.Id == zoneEntry.ZoneId)
        //            });
        //            positionAvailableForZone.CanBeUsed = false;

        //            _dbContext.SaveChanges();
        //        }
        //        else
        //        {
        //            throw new Exception("User identification failed.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }


        //}
    }
}
