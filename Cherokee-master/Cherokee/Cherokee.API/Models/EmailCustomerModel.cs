using Cherokee.API.Reports.ReportModels;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cherokee.API.Models
{
    public class EmailCustomerModel
    {

        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ProjectName { get; set; }
        public List<RoleInvoiceModel> Roles { get; set; }
        public string InvoiceDate { get; set; }
        public string MailToSend { get; set; }
        public string InvoiceNumber { get; set; }
        public string Status { get; set; }
        public string SentOrCancelDate { get; set; }
        public decimal? Amount { get; set; }
    }
}