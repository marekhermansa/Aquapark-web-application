using System.Collections.Generic;
using AquaparkApplication.Models.Dtos;
using Newtonsoft.Json;

namespace AquaparkApplication.Models.PassedParameters
{
    public class NewOrder
    {
        [JsonProperty("userToken")]
        public string UserToken { get; set; }

        [JsonProperty("userData")]
        public UserDataDto UserData { get; set; }
        [JsonProperty("ticketsWithClassDiscounts")]
        public IEnumerable<NewTicketWithClassDiscount> TicketsWithClassDiscounts { get; set; }

    }
}