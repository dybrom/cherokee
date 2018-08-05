using Cherokee.API.Models;
using Cherokee.API.Reports.ReportModels;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Cherokee.NotificationsService
{
    public partial class TimeKeeperNotification : ServiceBase
    {
        System.Timers.Timer delayTime;
        int count;
        public TimeKeeperNotification()
        {
            InitializeComponent();
            delayTime = new System.Timers.Timer();
            delayTime.Elapsed += new ElapsedEventHandler(WorkProcess);
            delayTime.Elapsed += new ElapsedEventHandler(WorkProcessForInvoice);
        }

        protected override void OnStart(string[] args)
        {
            Console.WriteLine("Service has started!");
            delayTime.Enabled = true;
        }

        protected override void OnStop()
        {
            Console.WriteLine("Service stopped!");
            delayTime.Enabled = false;
        }

        public void WorkProcess(object sender, ElapsedEventArgs e)
        {
            var conString = "mongodb://localhost:27017";
            MongoClient client = new MongoClient(conString);
            //povezivanje sa bazom
            var dbBase = client.GetDatabase("Services");
            // povezivanje sa kolekcijom
            var messages = dbBase.GetCollection<EmailEmployeeModel>("Emails");
            // pokupiti sve iz  baze ( filter empty -> sve sto nije empty :D )
            List<EmailEmployeeModel> contents = messages.Find(Builders<EmailEmployeeModel>.Filter.Empty).ToList();
            if (contents.Count() > 0)
            {
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
                foreach (var content in contents)
                {
                    //slanje maila
                    content.Name = "Notification for " + content.Email;
                    content.Email = "cherokeetimekeeper@gmail.com";
                    MailMessage mailMessage = new MailMessage("cherokeetimekeeper@gmail.com", content.Email)
                    {
                        Body = content.MailToSend,
                        Subject = content.Email
                    };


                    smtpClient.Send(mailMessage);
                    //Email.Send(content.ReceiverMailAddress, content.MailSubject);
                    //brisemo kad posaljemo
                    mailMessage = null;
                    messages.DeleteOne(Builders<EmailEmployeeModel>.Filter.Eq("_id", content.Id));
                    Thread.Sleep(2500);

                }
            }
        }

        public void WorkProcessForInvoice(object sender, ElapsedEventArgs e)
        {
            var conString = "mongodb://localhost:27017";
            MongoClient client = new MongoClient(conString);
            //povezivanje sa bazom
            var dbBase = client.GetDatabase("Billings");
            // povezivanje sa kolekcijom
            var messages = dbBase.GetCollection<EmailCustomerModel>("InvoiceSendBuffer");
            // pokupiti sve iz  baze ( filter empty -> sve sto nije empty :D )
            List<EmailCustomerModel> invoices = messages.Find(_ => true).ToList();
            if (invoices.Count() >= 0)
            {
                SmtpClient smtpClient = new SmtpClient
                {
                    Port = 587,
                    Host = "smtp.gmail.com",
                    EnableSsl = true,
                    Timeout = 10000 + (invoices.Count() * 10000),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential("cherokeetimekeeper@gmail.com", "sulejmanc4")
                };

                //StringBuilder BodyContent = new StringBuilder();

                foreach (var invoice in invoices)
                {
                    //foreach (var role in invoice.Roles)
                    //{
                    //    if(role.Status != "Sent")
                    //    {
                    //        BodyContent.Append(role.Description + " | " + role.Quantity + " hours | " + role.SubTotal + "$" + " <br> " + "\n\r");   
                    //    }
                    //    else { continue; }
                    //}

                    //slanje maila
                    invoice.Name = "Notification for " + invoice.Name;
                    invoice.Email = "cherokeetimekeeper@gmail.com";
                    MailMessage mailMessage = new MailMessage("cherokeetimekeeper@gmail.com", invoice.Email)
                    {
                        Body = invoice.MailToSend,
                        //Body = BodyContent.ToString(),
                        Subject = invoice.Email
                    };

                    try
                    {
                        smtpClient.Send(mailMessage);
                        mailMessage = null;
                        messages.DeleteOne(Builders<EmailCustomerModel>.Filter.Eq("_id", invoice.Id));
                    }
                    catch { }
                    
                    //Email.Send(content.ReceiverMailAddress, content.MailSubject);
                    //brisemo kad posaljemo
                    //if(invoice.Status == "Canceled")

                    Thread.Sleep(2500);

                }
            }
        }
        //kraj workprocessforinvoices


    }
}
