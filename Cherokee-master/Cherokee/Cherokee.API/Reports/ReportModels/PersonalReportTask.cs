using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cherokee.API.Controllers.Helper.Reports
{
    public class PersonalReportTask
    {
        public string Project { get; set; }
        public string Description { get; set; }
        public decimal? Hours { get; set; }
    }
}