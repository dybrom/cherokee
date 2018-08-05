using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cherokee.API.Reports.ReportModels
{
    public class RoleInvoiceModel
    {
        public string Description { get; set; }
        public decimal? Quantity { get; set; }
        public string Unit { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? SubTotal { get; set; }
        public string Status { get; set; }
    }
}