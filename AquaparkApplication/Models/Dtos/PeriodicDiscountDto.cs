using System;
using Newtonsoft.Json;

namespace AquaparkSystemApi.Models.Dtos
{
    public class PeriodicDiscountDto
    {
        public int Id { get; set; }
        [JsonIgnore]
        public DateTime StartTimeDate { get; set; }
        [JsonIgnore]
        public DateTime FinishTimeDate { get; set; }

        private string startTime;
        public string StartTime
        {
            get
            {
                return StartTimeDate.ToString("yyyy-MM-dd");
            }
            set
            {
                startTime = value;
                StartTimeDate = Convert.ToDateTime(startTime);
            }
        }
        private string finishTime;
        public string FinishTime
        {
            get
            {
                return FinishTimeDate.ToString("yyyy-MM-dd");
            }
            set
            {
                finishTime = value;
                FinishTimeDate = Convert.ToDateTime(finishTime);
            }
        }
        public decimal Value { get; set; }

    }
}