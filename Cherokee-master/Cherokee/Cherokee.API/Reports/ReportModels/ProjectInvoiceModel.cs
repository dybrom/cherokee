using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cherokee.API.Reports.ReportModels
{
    public class ProjectInvoiceModel
    {
        public string ProjectName { get; set; }
        public string CustomerName { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal? Amount { get; set; }
        public List<RoleInvoiceModel> Roles { get; set; }
        public string CustomerEmail { get; set; }
        public string InvoiceDate { get; set; }
        public string Status { get; set; }
    }
}