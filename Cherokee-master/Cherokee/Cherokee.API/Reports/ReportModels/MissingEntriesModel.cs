using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cherokee.API.Reports.ReportModels
{
    public class MissingEntriesModel
    {
        public MissingEntriesModel()
        {
            Employees = new List<MEEmployeeModel>();
        }
        public int Year { get; set; }
        public int Month { get; set; }
        public List<MEEmployeeModel> Employees { get; set; }
    }
}