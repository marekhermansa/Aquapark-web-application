using System;
using System.Collections.Generic;
using System.Linq;
using AquaparkApplication.Models;
using AquaparkApplication.Models.Dtos;
using AquaparkSystemApi.Exceptions;
using AquaparkSystemApi.Models.Dtos;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace AquaparkApplication.Controllers
{
    [EnableCors("AllowMyOrigin")]
    public class DiscountsController : Controller
    {
        private AquaparkDbContext _dbContext;

        public DiscountsController(AquaparkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [AcceptVerbs("POST")]
        [ActionName("AddSocialClassDiscounts")]
        public IEnumerable<SocialClassDiscountDto> AddSocialClassDiscounts([FromBody]SocialClassDiscountCollectionDto socialClassDiscountCollectionDto)
        {
            var socialClassDiscounts = socialClassDiscountCollectionDto.SocialClassDiscounts;

            try
            {
                if (AquaparkSystemApi.Security.Security.UserTokens.Any(i => i.Value == socialClassDiscountCollectionDto.UserToken))
                {
                    var userId = AquaparkSystemApi.Security.Security.UserTokens.FirstOrDefault(i => i.Value == socialClassDiscountCollectionDto.UserToken).Key;

                    var user = _dbContext.Users.FirstOrDefault(i => i.Id == userId);
                    if (user == null)
                    {
                        throw new UserNotFoundException("There is no user with given data.");
                    }

                    // remove scd

                    var dbSocialClassDiscounts = _dbContext.SocialClassDiscounts;
                    var dbPositions = _dbContext.Positions.Include(p => p.SocialClassDiscount);

                    var socialClassDiscountsToRemove = new List<SocialClassDiscount>();
                    var positionsToUpdate = new List<Position>();

                    dbSocialClassDiscounts.ToList()
                        .ForEach(s =>
                        {
                            var dbSocialClassDiscountId = s.Id;

                            var matchedDiscounts = socialClassDiscounts
                           .ToList()
                           .SingleOrDefault(d => d.Id == dbSocialClassDiscountId);

                            if (matchedDiscounts == null)
                            {
                                socialClassDiscountsToRemove.Add(s);

                                // make ticket id null in postiions table
                                var pToUpdate = dbPositions.Where(p => p.SocialClassDiscount.Id == s.Id).ToList();
                                pToUpdate.ForEach(p =>
                                {
                                    p.SocialClassDiscount = null;
                                    positionsToUpdate.Add(p);
                                });
                            }
                        });

                    _dbContext.UpdateRange(positionsToUpdate);
                    _dbContext.SaveChanges();

                    _dbContext.RemoveRange(socialClassDiscountsToRemove);
                    _dbContext.SaveChanges();

                    // add scd

                    socialClassDiscounts
                        .ToList()
                        .ForEach(s =>
                        {
                            var matchedDiscounts = dbSocialClassDiscounts.ToList()
                                .SingleOrDefault(d => d.Id == s.Id);
                            if (matchedDiscounts == null)
                            {
                                var newsocialClassDiscount = new SocialClassDiscount();

                                var newDiscount = new SocialClassDiscount()
                                {
                                    Id = s.Id,
                                    Value = s.Value,
                                    SocialClassName = s.SocialClassName
                                };
                                _dbContext.SocialClassDiscounts.Add(newDiscount);
                            }
                        });
                    _dbContext.SaveChanges();

                    // update scd

                    socialClassDiscounts
                        .ToList()
                        .ForEach(s =>
                        {
                            var matchedDiscount = dbSocialClassDiscounts.ToList()
                                .SingleOrDefault(d => d.Id == s.Id);
                            if (matchedDiscount != null)
                            {
                                matchedDiscount.Id = s.Id;
                                matchedDiscount.Value = s.Value;
                                matchedDiscount.SocialClassName = s.SocialClassName;

                                _dbContext.SocialClassDiscounts.Update(matchedDiscount);
                            }
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
                return this.GetAllSocialClassDiscounts();
            }

            return this.GetAllSocialClassDiscounts();
        }

        [AcceptVerbs("POST")]
        [ActionName("AddPeriodicDiscounts")]
        public IEnumerable<PeriodicDiscountDto> AddPeriodicDiscounts([FromBody]PeriodicDiscountCollectionDto periodicDiscountCollectionDto)
        {
            var periodicDiscounts = periodicDiscountCollectionDto.PeriodicDiscounts;

            try
            {
                if (AquaparkSystemApi.Security.Security.UserTokens.Any(i => i.Value == periodicDiscountCollectionDto.UserToken))
                {
                    var userId = AquaparkSystemApi.Security.Security.UserTokens.FirstOrDefault(i => i.Value == periodicDiscountCollectionDto.UserToken).Key;

                    var user = _dbContext.Users.FirstOrDefault(i => i.Id == userId);
                    if (user == null)
                    {
                        throw new UserNotFoundException("There is no user with given data.");
                    }

                    _dbContext.Tickets.ToList().ForEach(x => x.PeriodicDiscount = null);
                    _dbContext.Positions.ToList().ForEach(x => x.PeriodicDiscount = null);
                    _dbContext.SaveChanges();

                    _dbContext.PeriodicDiscounts.RemoveRange(_dbContext.PeriodicDiscounts);
                    _dbContext.PeriodicDiscounts.AddRange(periodicDiscounts);
                    _dbContext.SaveChanges();
                }
                else
                {
                    throw new Exception("User identification failed.");
                }
            }
            catch (Exception e)
            {
                return this.GetAllPeriodicDiscount();
            }

            return this.GetAllPeriodicDiscount();
        }

        [AcceptVerbs("GET")]
        [ActionName("GetAllPeriodicDiscount")]
        public IEnumerable<PeriodicDiscountDto> GetAllPeriodicDiscount()
        {
            List<PeriodicDiscountDto> periodicDiscountDtos = new List<PeriodicDiscountDto>();
            try
            {
                periodicDiscountDtos = _dbContext.PeriodicDiscounts.Select(i => new PeriodicDiscountDto()
                {
                    Id = i.Id,
                    StartTimeDate = i.StartTime,
                    FinishTimeDate = i.FinishTime,
                    Value = i.Value
                }).ToList();
            }
            catch (Exception)
            {
                return periodicDiscountDtos;
            }

            return periodicDiscountDtos;
        }

        [AcceptVerbs("GET")]
        [ActionName("GetAllSocialClassDiscounts")]
        public IEnumerable<SocialClassDiscountDto> GetAllSocialClassDiscounts()
        {
            List<SocialClassDiscountDto> socialClassDiscountDtos = new List<SocialClassDiscountDto>();
            try
            {
                socialClassDiscountDtos = _dbContext.SocialClassDiscounts.Select(i => new SocialClassDiscountDto()
                {
                    Id = i.Id,
                    SocialClassName = i.SocialClassName,
                    Value = i.Value
                }).OrderBy(i => i.Value).ToList();
            }
            catch (Exception)
            {
                return socialClassDiscountDtos;
            }

            return socialClassDiscountDtos;
        }
    }
}
