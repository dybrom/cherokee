using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cherokee.API.Reports.ReportModels
{
    public class PersonalReportProjectModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? Hours { get; set; }
    }
}