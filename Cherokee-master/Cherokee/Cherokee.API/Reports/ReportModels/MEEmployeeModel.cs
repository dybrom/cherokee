using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cherokee.API.Reports.ReportModels
{
    public class MEEmployeeModel
    {
        public MEEmployeeModel()
        {
            MissingEntries = new List<MEModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int TotalMissingEntries { get; set; }
        public List<MEModel> MissingEntries { get; set; }


    }
}