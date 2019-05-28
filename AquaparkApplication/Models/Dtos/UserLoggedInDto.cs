﻿namespace AquaparkApplication.Models.Dtos
{
    public class UserLoggedInDto
    {
        public string UserToken { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool Success { get; set; }
        public string Status { get; set; }
        public bool IsAdmin { get; set; }
    }
} 