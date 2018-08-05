using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cherokee.API.Reports.Models
{
    public class MonthlyItem
    {
        public MonthlyItem(int listSize)
        {
            Hours = new decimal[listSize];
        }

        public string Employee { get; set; }
        public decimal Total { get; set; }
        public decimal[] Hours { get; set; }
    }
}