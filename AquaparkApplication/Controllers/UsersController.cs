using System;
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
    public class UsersController : Controller
    {
        private AquaparkDbContext _dbContext;

        public UsersController(AquaparkDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        [ActionName("RegisterUser")]
        [HttpPost]
        public JsonResult RegisterUser([FromBody]UserToRegister userToRegister)
        {
            Guid userGuid = Guid.NewGuid();
            string hashedPassword = AquaparkSystemApi.Security.Security.HashSHA1(userToRegister.Password + userGuid.ToString());
            string statusMessage = "";
            bool wasUserAdded = false;

            bool doesUserWithGivenLoginExist = _dbContext.Users.Any(i => i.Email == userToRegister.Email);
            if (!doesUserWithGivenLoginExist)
            {
                _dbContext.Users.Add(
                    new User()
                    {
                        Name = userToRegister.Name,
                        Surname = userToRegister.Surname,
                        Password = hashedPassword,
                        UserGuid = userGuid,
                        Email = userToRegister.Email,
                        IsAdmin = false,
                    });
                _dbContext.SaveChanges();
                wasUserAdded = true;
            }
            else
            {
                statusMessage = "There is a user with given login or email.";
            }

            return new JsonResult(new UserRegisteredDto()
            {
                Success = wasUserAdded,
                Status = statusMessage
            });
        }

        [AcceptVerbs("POST")]
        [ActionName("LogIn")]
        public UserLoggedInDto LogIn(UserToLogIn userToLogIn)
        {
            string userToken = "";
            string email = "";
            string name = "";
            string surname = "";
            bool success = false;
            string statusMessage = "";
            bool isAdmin = false;

            try
            {
                User user = _dbContext.Users.ToList().FirstOrDefault(i => i.Email == userToLogIn.Email);
                if (user == null)
                    throw new UserNotFoundException("There is no user with given data.");

                string hashedPassword = AquaparkSystemApi.Security.Security.HashSHA1(userToLogIn.Password + user.UserGuid);

                if (user.Password.Trim() == hashedPassword)
                {
                    string generatedToken = AquaparkSystemApi.Security.Security.UserTokens.FirstOrDefault(i => i.Key == user.Id).Value;
                    if (!string.IsNullOrEmpty(generatedToken))
                    {
                        userToken = generatedToken;
                        email = user.Email;
                        name = user.Name;
                        surname = user.Surname;
                        success = true;
                        statusMessage = "";
                        isAdmin = user.IsAdmin;
                    }
                    else if (AquaparkSystemApi.Security.Security.UserTokens.All(i => i.Key != user.Id))
                    {
                        generatedToken = AquaparkSystemApi.Security.Security.GenerateToken(user.Email);
                        AquaparkSystemApi.Security.Security.UserTokens.Add(user.Id, generatedToken);
                        userToken = generatedToken;
                        email = user.Email;
                        name = user.Name;
                        surname = user.Surname;
                        success = true;
                        statusMessage = "";
                        isAdmin = user.IsAdmin;
                    }
                }
            }
            catch (Exception e)
            {
                userToken = name = surname = "";
                success = false;
                statusMessage = e.Message;
            }

            return new UserLoggedInDto()
            {
                UserToken = userToken,
                Email = email,
                Name = name,
                Surname = surname,
                Success = success,
                Status = statusMessage,
                IsAdmin = isAdmin
            };
        }

        [AcceptVerbs("POST")]
        [ActionName("LogOut")]
        public UserLoggedOutDto LogOut(TokenOfUser user)
        {
            bool success = false;
            string statusMessage = "";
            try
            {
                bool loggedOut = AquaparkSystemApi.Security.Security.UserTokens.Remove(AquaparkSystemApi.Security.Security.UserTokens.First(i => i.Value == user.UserToken).Key);

                if (loggedOut)
                {
                    statusMessage = "User logged out successfully!";
                    success = true;
                }  
            }
            catch (Exception)
            {
                success = false;
                statusMessage = "Something went wrong :(";
            }

            return new UserLoggedOutDto()
            {
                Success = success,
                Status = statusMessage
            };
        }

        [AcceptVerbs("POST")]
        [ActionName("EditUser")]
        public UserEditedPersonalDataDto EditUser(UserToEditPersonalData editedUser)
        {
            string userToken = "";
            string name = "";
            string surname = "";
            string email = "";
            bool success = false;
            string statusMessage = "";

            try
            {
                int userId;
                if (AquaparkSystemApi.Security.Security.UserTokens.Any(i => i.Value == editedUser.UserToken))
                {
                    userId = AquaparkSystemApi.Security.Security.UserTokens.FirstOrDefault(i => i.Value == editedUser.UserToken).Key;

                    User user = _dbContext.Users.FirstOrDefault(i => i.Id == userId);
                    if (user == null)
                        throw new UserNotFoundException("There is no user with given data.");

                    _dbContext.Entry(user).State = EntityState.Modified;
                    email = user.Email = editedUser.Email;
                    name = user.Name = editedUser.Name;
                    surname = user.Surname = editedUser.Surname;
                    userToken = editedUser.UserToken;
                    success = true;
                    statusMessage = "";
                    _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("User identification failed.");
                }
                    
            }
            catch (Exception e)
            {
                userToken = name = surname = "";
                success = false;
                statusMessage = e.Message;
            }

            return new UserEditedPersonalDataDto()
            {
                UserToken = userToken,
                Name = name,
                Surname = surname,
                Email = email,
                Success = success,
                Status = statusMessage
            };
        }
    }
}
