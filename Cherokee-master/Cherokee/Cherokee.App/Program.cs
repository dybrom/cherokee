using Cherokee.API.Models;
using Cherokee.DAL;
using Cherokee.DAL.Entities;
using Cherokee.DAL.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cherokee.App
{
    class Program
    {
        //public  class Result
        //{
        //    public Employee Employee { get; set; }
        //    public decimal TotalHours { get; set; }
        //    public decimal AverageHours { get; set; }



        //}

        static void Main(string[] args)
        {
            var conString = "mongodb://localhost:27017";
            MongoClient client = new MongoClient(conString);
            //povezivanje sa bazom
            var dbBase = client.GetDatabase("Services");
            // povezivanje sa kolekcijom
            var messages = dbBase.GetCollection<EmailEmployeeModel>("Emails");
            // pokupiti sve iz  baze ( filter empty -> sve sto nije empty :D )
            List<EmailEmployeeModel> contents = messages.Find(Builders<EmailEmployeeModel>.Filter.Empty).ToList();
            Console.WriteLine(contents.Count());
            SmtpClient smtpClient = new SmtpClient
            {
                Port = 587,
                Host = "smtp.gmail.com",
                EnableSsl = true,
                Timeout = 10000,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential("cherokeetimekeeper@gmail.com", "sulejmanc4")
            };
            int count = 0;
            foreach (var content in contents)
            {
                //slanje maila
                count++;
                content.Name = "Notification for " + content.Email;
                content.Email = "cherokeetimekeeper@gmail.com";
                MailMessage mailMessage = new MailMessage("cherokeetimekeeper@gmail.com", content.Email)
                {
                    Body = content.Name,
                    Subject = content.Email
                };
                

                smtpClient.Send(mailMessage);
                //Email.Send(content.ReceiverMailAddress, content.MailSubject);
                //brisemo kad posaljemo
                messages.DeleteOne(Builders<EmailEmployeeModel>.Filter.Eq("_id", content.Id));
                Thread.Sleep(2500);

            }
            Console.WriteLine(count);
            Console.ReadKey();
            //var conString = "mongodb://localhost:27017";

            //var client = new MongoClient(conString);

            //var DB = client.GetDatabase("TestDatabase");
            //var collection = DB.GetCollection<BsonDocument>("Student");

            ////var document = new BsonDocument
            ////{
            ////    {"Name" , "Testic" },
            ////    {"Subject", "Physics" },
            ////    {"Marks", "100" }
            ////};
            //var name = "Arif";
            //var subject = "nesto";
            //int broj = 100;
            //    var document = new BsonDocument
            //{
            //    {"Name" , name },
            //    {"Subject", subject },
            //    {"Marks",broj}
            //};
            //    collection.InsertOne(document);




            //double time;

            //using (UnitOfWork unit = new UnitOfWork())
            //{
            //    Employee emp = unit.Employees.Get(1);
            //    emp.Position = unit.Roles.Get("A");
            //    unit.Save();
            //}
            //{
            //    Console.Write("Enter team name : ");
            //    string teamName = Console.ReadLine();

            //    DateTime srcStart = DateTime.Now;
            //    var items = context.Teams
            //                       .Where(x => x.Name.Contains(teamName))
            //                       .SelectMany(e => e.Engagements).ToList();

            //    foreach(var item in items)
            //    {
            //        Console.WriteLine($"{item.Team.Name}  {item.Role.Id} {item.Employee.FirstName} | {item.Hours}");
            //    }
            //    time = Math.Round((DateTime.Now - srcStart).TotalSeconds, 3);
            //}
            //Console.WriteLine($"\n------------------------");
            ////Console.WriteLine($"\n{count} records retreived. ");
            ////Console.WriteLine($"\n{result}  records found.");
            ////Console.WriteLine($"\ntook {time} to get it done. ");
            //Console.WriteLine("\n*** press any key ***");
            //Console.ReadKey();


        }
    }
}
