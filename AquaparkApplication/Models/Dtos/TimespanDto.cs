using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AquaparkApplication.Models.Dtos
{
    public class TimespanDto
    {
        public DateTime StartTimeDate { get; set; }
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
    }
}
