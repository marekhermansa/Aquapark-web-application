using Newtonsoft.Json;

namespace AquaparkApplication.Models.PassedParameters
{
    public class UserToRegister
    {
        [JsonProperty("login")]
        public string Login { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("surname")]
        public string Surname { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }

    }
}