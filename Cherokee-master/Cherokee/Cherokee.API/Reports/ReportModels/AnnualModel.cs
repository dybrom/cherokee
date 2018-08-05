using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cherokee.API.Reports.Models
{
    public class AnnualModel
    {
        public AnnualModel()
        {
            MonthlyHours = new decimal[12];
        }
        public string ProjectName { get; set; }
        public decimal[] MonthlyHours { get; set; }
        public decimal TotalHours { get; set; }
        public int NumOfEmployees { get; set; }
    }
}