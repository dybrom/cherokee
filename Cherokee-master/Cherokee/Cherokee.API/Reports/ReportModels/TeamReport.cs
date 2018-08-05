using Cherokee.API.Controllers.Helper.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cherokee.API.Reports.ReportModels
{
    public class TeamReport
    {
        public TeamReport()
        {
            Reports = new List<TeamMemberModel>();
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public int NumberOfEmployees { get; set; }
        public int NumberOfProjects { get; set; }
        public decimal? TotalHours { get; set; }
        public DayStatisticModel Dayss { get; set; }
        public List<TeamMemberModel> Reports { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int MaxPossibleHours { get; set; }
        public List<PersonalReportProjectModel> Projects { get; set; }


    }
}