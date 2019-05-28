using System;
using System.Collections.Generic;
using System.Linq;
using AquaparkApplication.Models;
using AquaparkApplication.Models.Dtos;
using AquaparkApplication.Models.PassedParameters;
using AquaparkApplication.Other;
using AquaparkSystemApi.Exceptions;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AquaparkApplication.Controllers
{
    [EnableCors("AllowMyOrigin")]
    public class AdminController : Controller
    {
        private AquaparkDbContext _dbContext;

        public AdminController(AquaparkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [AcceptVerbs("POST")]
        [ActionName("SendNewsletter")]
        public ResultInfoDto SendNewsletter(EmailPassedParameters emailPassedParameters)
        {
            string status = "";
            bool success = false;
            try
            {
                if (AquaparkSystemApi.Security.Security.UserTokens.Any(i => i.Value == emailPassedParameters.UserToken))
                {
                    
                    var userId = AquaparkSystemApi.Security.Security.UserTokens.FirstOrDefault(i => i.Value == emailPassedParameters.UserToken).Key;
                    
                    var user = _dbContext.Users.FirstOrDefault(i => i.Id == userId);
                    if (user == null)
                    {
                        throw new UserNotFoundException("There is no user with given data.");
                    }

                    List<User> users = _dbContext.Users.ToList();

                    int emailSentCounter = 0;
                    foreach (User userToSendEmail in users)
                    {
                        if (Email.SendEmail(userToSendEmail.Email, emailPassedParameters.Message, "Newsletter"))
                            emailSentCounter++;
                    }

                    success = true;
                    status = $"Wysłano do {emailSentCounter} użytkowników.";
                }
                else
                {
                    throw new Exception("User identification failed.");
                }
            }
            catch (Exception e)
            {
                success = false;
                status = e.Message;
            }

            return new ResultInfoDto()
            {
                Success = success,
                Status = status
            };
        }
    }
}