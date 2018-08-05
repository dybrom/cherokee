using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cherokee.API.Controllers.Helper.Reports
{
    public class PersonalReportDay
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public string Type { get; set; }
        public decimal? Hours { get; set; }
        public decimal? OvertimeHours { get; set; }
        public string Comment { get; set; }
        public int Ordinal { get; set; }
        public virtual List<PersonalReportTask> Tasks { get; set; }

    }
}