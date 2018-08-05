using Cherokee.API.Controllers.Helper;
using Cherokee.API.Models;
using Cherokee.API.Reports.ReportModels;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cherokee.API.Controllers
{
    [RoutePrefix("api/missingentries")]
    public class MissingEntriesController : BaseController
    {
        [Route("")]
        public IHttpActionResult Get(int year, int month)
        {
            MissingEntriesModel report = BuildReport(year,month);
            return Ok(report);
        }



        [Route("")]
        public IHttpActionResult Post([FromBody] MissingEntriesModel model)
        {
            var conString = "mongodb://localhost:27017";

            var client = new MongoClient(conString);

            var DB = client.GetDatabase("Services");
            var collection = DB.GetCollection<EmailEmployeeModel>("Emails");

            //foreach (var employee in model.Employees)
            //{
            //    var document = new BsonDocument
            //{
            //    {"Name" , employee.Name },
            //    {"Email", employee.Email }

            //};
            //    var arr = new BsonArray();
            //    foreach (var entry in employee.MissingEntries)
            //    {
            //        arr.Add(new BsonDocument("Day:", entry.Day));
                    
            //    }
            //    document.Add("MissingEntries:", arr);
            //    collection.InsertOne(document);
            //}
            foreach(var employee in model.Employees)
            {
                string MailToSend = "Dear " + employee.Name + " you have missing entries for Month : "
                    + model.Month + " in Year: " + model.Year + " for days: " ;
                EmailEmployeeModel employeeToAdd = new EmailEmployeeModel() {
                    Name = employee.Name,
                    Email = employee.Email,
                    TotalMissingEntries = employee.MissingEntries.Count()
                };
                foreach(var day in employee.MissingEntries)
                {
                    MailToSend += day.Day + ", ";
                }
                employeeToAdd.MailToSend = MailToSend;
                collection.InsertOne(employeeToAdd);
            }
            return Ok(model);
        }



        public MissingEntriesModel BuildReport(int year, int month)
        {
            var date = new DateTime(year, month, DateTime.DaysInMonth(year,month));
            List<MEEmployeeModel> MEEmployees = new List<MEEmployeeModel>();
            
            int Limit = DateTime.DaysInMonth(year, month);
            var employees = TimeUnit.Employees.Get().Where(e => e.BeginDate <= date ).ToList();
            foreach(var employee in employees)
            {
                List<int> CalendarDays = new List<int>();
                int k = 0;
                if ( employee.BeginDate.Value.Year == year && employee.BeginDate.Value.Month == month  && employee.BeginDate.Value.Day > 1) k = employee.BeginDate.Value.Day - 1;
                for (int i = k; i < Limit; i++)
                {
                    DateTime DateHelper = new DateTime(year, month, i+1);
                    if (DateHelper.DayOfWeek == DayOfWeek.Saturday || DateHelper.DayOfWeek == DayOfWeek.Sunday)
                        CalendarDays.Add(2);
                    else
                    CalendarDays.Add(1);
                }

                var daysss = employee.Days
                    .Where(x=> x.Date.Value.Year == year && x.Date.Value.Month == month);

                foreach(var day in daysss)
                {
                    
                    CalendarDays[day.Date.Value.Day - 1 - k] = 0;
                }
                List<MEModel> missingDays = new List<MEModel>();

                for (int i = k; i < Limit; i++)
                {
                    if (CalendarDays[i-k] == 1)
                        missingDays.Add(new MEModel()
                        {
                            Day = i + 1
                        });
                    
                }
                if (missingDays.Count != 0)
                {

                    MEEmployeeModel employeeToAdd = new MEEmployeeModel()
                    {
                        Id = employee.Id,
                        Name = employee.FullName,
                        Email = employee.Email,
                        MissingEntries = missingDays,
                        TotalMissingEntries = missingDays.Count()
                    };
                    MEEmployees.Add(employeeToAdd);
                }

            }


            return new MissingEntriesModel()
            {
                Year = year,
                Month = month,
                Employees = MEEmployees
            };
        }
    }
   
}
