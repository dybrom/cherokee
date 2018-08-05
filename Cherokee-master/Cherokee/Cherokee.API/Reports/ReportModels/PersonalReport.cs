using Cherokee.API.Models;
using Cherokee.API.Reports.ReportModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cherokee.API.Controllers.Helper.Reports
{
    public class PersonalReport
    {
        public EmployeeModel Employee { get; set; }
        public List<PersonalReportDay> Days { get; set; }
        public decimal? OvertimeHours { get; set; }
        public int MissingEntries { get; set; }
        public int WorkingDays { get; set; }
        public double PercentageOfWorkingDays { get; set; }
        public int VacationDays { get; set; }
        public int BusinessAbscenceDays { get; set; }
        public int PublicHolidays { get; set; }
        public int SickLeaveDays { get; set; }
        public int ReligiousDays { get; set; }
        public int OtherAbscenceDays { get; set; }
        public decimal TotalHours { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal? TotalHoursInYear { get; set; }
        public decimal? TotalPossibleHours { get; set; }
        public List<PersonalReportProjectModel> Projects { get; set; }
    }
}