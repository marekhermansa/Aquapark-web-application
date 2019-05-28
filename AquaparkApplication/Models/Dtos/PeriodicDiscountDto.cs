using System;
using Newtonsoft.Json;

namespace AquaparkApplication.Models.Dtos
{
    public class PeriodicDiscountDto
    {
        public int Id { get; set; }
        [JsonIgnore]
        public DateTime StartTimeDate { get; set; }
        [JsonIgnore]
        public DateTime FinishTimeDate { get; set; }

        public string StartTime => StartTimeDate.ToString("yyyy-MM-dd");
        public string FinishTime => FinishTimeDate.ToString("yyyy-MM-dd");
        public decimal Value { get; set; }

    }
}