using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cherokee.API.Reports.Models
{
    public class MonthlyModel
    {
        public MonthlyModel()
        {
            Projects = new List<ProjectItem>();
            Items = new List<MonthlyItem>();
        }
        public int Year { get; set; }
        public int Month { get; set; }
        public List<ProjectItem> Projects { get; set; }
        public decimal Total { get; set; }
        public List<MonthlyItem> Items { get; set; }

    }
}