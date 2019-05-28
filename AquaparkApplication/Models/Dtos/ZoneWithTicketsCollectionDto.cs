using System.Collections.Generic;
using Newtonsoft.Json;

namespace AquaparkApplication.Models.Dtos
{
    public class ZoneWithTicketsCollectionDto
    {
        [JsonProperty("userToken")]
        public string UserToken { get; set; }

        [JsonProperty("zonesWithTicketsDto")]
        public IEnumerable<ZoneWithTicketsDto> ZonesWithTicketsDto { get; set; }
    }
}