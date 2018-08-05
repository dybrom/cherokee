using Cherokee.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Cherokee.API.Reports
{

    public class ProjectHistoryModel
    {
        public ProjectHistoryModel()
        {
            Employees = new List<ProjectHistoryEmployee>();
            Total = new List<ProjectHistoryTotal>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? TotalHours { get; set; }
        //public List<AnnualReportEmployees> Employees { get; set; }
        public List<ProjectHistoryEmployee> Employees { get; set; }
        public List<ProjectHistoryTotal> Total { get; set; }
    }

    public class ProjectHistoryTotal
    {
        public ProjectHistoryTotal()
        {
            MonthlyHours = new List<MonthlyHours>();
            MonthlyHours.Add(new MonthlyHours()
            {
                Month = "January"
            });
            MonthlyHours.Add(new MonthlyHours()
            {
                Month = "February"
            });
            MonthlyHours.Add(new MonthlyHours()
            {
                Month = "March"
            });
            MonthlyHours.Add(new MonthlyHours()
            {
                Month = "April"
            });
            MonthlyHours.Add(new MonthlyHours()
            {
                Month = "May"
            });
            MonthlyHours.Add(new MonthlyHours()
            {
                Month = "June"
            });
            MonthlyHours.Add(new MonthlyHours()
            {
                Month = "July"
            });
            MonthlyHours.Add(new MonthlyHours()
            {
                Month = "August"
            });
            MonthlyHours.Add(new MonthlyHours()
            {
                Month = "September"
            });
            MonthlyHours.Add(new MonthlyHours()
            {
                Month = "October"
            });
            MonthlyHours.Add(new MonthlyHours()
            {
                Month = "November"
            });
            MonthlyHours.Add(new MonthlyHours()
            {
                Month = "December"
            });

        }
        public int Year { get; set; }
        public decimal? TotalHours { get; set; }
        public List<MonthlyHours> MonthlyHours { get; set; }
    }
    public class ProjectHistoryEmployee
    {
        public ProjectHistoryEmployee()
        {
            Sums = new List<ProjectHistoryTotal>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProjectHistoryTotal> Sums { get; set; }
        public decimal? TotalHours { get; set; }
    }
    public class MonthlyHours
    {
        public string Month { get; set; }
        public decimal? Hours { get; set; }
    }
}