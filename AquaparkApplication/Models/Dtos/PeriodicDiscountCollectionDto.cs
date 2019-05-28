using System.Collections.Generic;
using Newtonsoft.Json;

namespace AquaparkApplication.Models.Dtos
{
    public class PeriodicDiscountCollectionDto
    {
        [JsonProperty("userToken")]
        public string UserToken { get; set; }

        [JsonProperty("periodicDiscounts")]
        public IEnumerable<PeriodicDiscount> PeriodicDiscounts { get; set; }
    }
}