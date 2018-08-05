using Cherokee.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cherokee.API.Reports.ReportModels
{
    public class CompanyReport
    {
        public CompanyReport()
        {
            Teams = new List<CompanyTeamModel>();
            Projects = new List<CompanyProjectModel>();
        }
        public int Year { get; set; }
        public int Month { get; set; }
        public int NumberOfEmployees { get; set; }
        public int NumberOfProjects { get; set; }
        public decimal? TotalHours { get; set; }
        public int MaxPossibleHours { get; set; }
        public double Utilization { get; set; }

        public decimal PMUtilization { get; set; }
        public int PMCount { get; set; }
        public decimal DevUtilization { get; set; }
        public int DevCount { get; set; }
        public decimal UIUXUtilization { get; set; }
        public int UIUXCount { get; set; }
        public decimal QAUtilization { get; set; }
        public int QACount { get; set; }

        public List<CompanyTeamModel> Teams { get; set; }
        public List<CompanyProjectModel> Projects { get; set; }


    }
}