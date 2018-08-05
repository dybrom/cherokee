using Cherokee.API.Reports.ReportModels;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cherokee.API.Models
{
    public class EmailEmployeeModel
    {
        public EmailEmployeeModel()
        {
        }
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int TotalMissingEntries { get; set; }
        public string MailToSend { get; set; }

    }
}