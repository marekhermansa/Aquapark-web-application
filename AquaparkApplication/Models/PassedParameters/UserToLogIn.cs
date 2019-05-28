using Newtonsoft.Json;

namespace AquaparkApplication.Models.PassedParameters
{
    public class UserToLogIn
    {
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}