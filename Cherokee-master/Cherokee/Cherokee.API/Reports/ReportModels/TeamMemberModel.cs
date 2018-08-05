using Cherokee.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cherokee.API.Reports.ReportModels
{
    public class TeamMemberModel
    {
        public BaseModel Employee { get; set; }
        public decimal? TotalHours { get; set; }
        public DayStatisticModel Days { get; set; }
        public int MaxPossibleHours { get; set; }
        public int PTO { get; set; }

    }
}