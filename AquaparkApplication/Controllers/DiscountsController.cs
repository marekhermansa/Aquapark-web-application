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
    public class DiscountsController : Controller
    {
        private AquaparkDbContext _dbContext;

        public DiscountsController(AquaparkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [AcceptVerbs("POST")]
        [ActionName("AddSocialClassDiscounts")]
        public IEnumerable<SocialClassDiscountDto> AddSocialClassDiscounts(SocialClassDiscountCollectionDto socialClassDiscountCollectionDto)
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

                    //_dbContext.Positions.ToList().ForEach(x => x.SocialClassDiscount = null);

                    //_dbContext.SaveChanges();

                    //_dbContext.SocialClassDiscounts.RemoveRange(_dbContext.SocialClassDiscounts);
                    //_dbContext.SocialClassDiscounts.AddRange(socialClassDiscounts);
                    //_dbContext.SaveChanges();

                    //==================

                    // remove
                    var discountsToRemove = new List<SocialClassDiscount>();
                    _dbContext.SocialClassDiscounts.ToList().ForEach(s =>
                    {
                        if (socialClassDiscounts.ToList().SingleOrDefault(x => x.Id == s.Id) == null)
                        {
                            discountsToRemove.Add(s);

                            var position = _dbContext.Positions.ToList().SingleOrDefault(x => x.Id == s.Id);
                            if (position != null)
                            {
                                position.SocialClassDiscount = null;
                                _dbContext.SaveChanges();
                            }
                        }
                    });
                    _dbContext.SocialClassDiscounts.RemoveRange(discountsToRemove);



                    _dbContext.SaveChanges();

                    var discountsToAdd = new List<SocialClassDiscount>();
                    socialClassDiscountCollectionDto.SocialClassDiscounts.ToList().ForEach(s =>
                    {
                        // add
                        if (_dbContext.SocialClassDiscounts.ToList().SingleOrDefault(x => x.Id == s.Id) == null)
                        {
                            discountsToAdd.Add(s);
                        }
                        // modify
                        else
                        {
                            var discount = _dbContext.SocialClassDiscounts.SingleOrDefault(x => x.Id == s.Id);
                            if (discount != null)
                            {
                                discount.Id = s.Id;
                                discount.SocialClassName = s.SocialClassName;
                                discount.Value = s.Value;
                                _dbContext.SaveChanges();
                            }
                        }
                    });
                    _dbContext.SocialClassDiscounts.AddRange(discountsToAdd);
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
        public IEnumerable<PeriodicDiscountDto> AddPeriodicDiscounts(PeriodicDiscountCollectionDto periodicDiscountCollectionDto)
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
            catch (Exception)
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
