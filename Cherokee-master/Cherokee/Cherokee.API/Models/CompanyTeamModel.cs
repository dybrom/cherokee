using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cherokee.API.Models
{
    public class CompanyTeamModel
    {
        public string Name { get; set; }
        public decimal? OvertimeHours { get; set; }
        public int MissingEntries { get; set; }
        public decimal? TotalHours { get; set; }
        public double Utilization { get; set; }
        public double? TotalPossibleHours { get; set; }
        public int PTO { get; set; }
        //public decimal?  PTOHours { get; set; }
    }
}