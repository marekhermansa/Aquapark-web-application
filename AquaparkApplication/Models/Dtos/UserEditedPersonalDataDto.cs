namespace AquaparkApplication.Models.Dtos
{
    public class UserEditedPersonalDataDto
    {
        public string UserToken { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public bool Success { get; set; }
        public string Status { get; set; }
    }
}