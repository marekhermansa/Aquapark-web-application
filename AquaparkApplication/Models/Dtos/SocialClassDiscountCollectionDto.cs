using System.Collections.Generic;
using Newtonsoft.Json;

namespace AquaparkApplication.Models.Dtos
{
    public class SocialClassDiscountCollectionDto
    {
        [JsonProperty("userToken")]
        public string UserToken { get; set; }

        [JsonProperty("socialClassDiscounts")]
        public IEnumerable<SocialClassDiscount> SocialClassDiscounts { get; set; }
    }
}