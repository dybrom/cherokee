using Cherokee.API.Controllers.Helper;
using Cherokee.API.Models;
using Cherokee.API.Reports.ReportModels;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;

namespace Cherokee.API.Controllers
{

    [RoutePrefix("api/invoice")]
    public class InvoiceController : BaseController
    {
        //[Route("")]
        //public IHttpActionResult Get(int year, int month)
        //{
        //    return Ok(TimeUnit.GetInvoiceReport(year, month, TimeFactory));
        //}


        /// <summary>
        /// Insert employee
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns></returns>
        //public IHttpActionResult Post([FromBody] ProjectInvoiceModel model)
        [Route("")]
        public IHttpActionResult PostInvoice([FromBody] EmailCustomerModel invoice)
        {
            var conString = "mongodb://localhost:27017";

            var client = new MongoClient(conString);

            var DB = client.GetDatabase("Billings");
            var collection = DB.GetCollection<EmailCustomerModel>("Invoices");

            if (invoice.Status == "Canceled")
            {
                collection.DeleteOne(Builders<EmailCustomerModel>.Filter.Eq("InvoiceNumber", invoice.InvoiceNumber));
                return Ok("Invoice canceled");
            }

            StringBuilder BodyContent = new StringBuilder();
            decimal? TotalBill = 0.00m;
            int billStatusCounter = 0;
            List<RoleInvoiceModel> Roless = new List<RoleInvoiceModel>();
            foreach (var role in invoice.Roles)
            {
                if (role.Status != "Not included" && role.Status != "Canceled")
                {
                    BodyContent.Append(role.Description.TrimEnd() + "(" + role.Quantity + " hours), SubTotal: " + role.SubTotal + "$" + "\n\r");
                    TotalBill += role.SubTotal;
                    billStatusCounter++;

                    Roless.Add(role);
                }
                else
                {
                    continue;
                }
            }

            string MailToSend = "Dear " + invoice.Name + " we are sending You invoice for "
                   + invoice.ProjectName + " for " + invoice.InvoiceDate + ". InvoiceNumber: " + invoice.InvoiceNumber + "\n\r" + BodyContent.ToString() + "\n\r" + "Total: " + TotalBill + " $";

            if (billStatusCounter == 0)
            {
                invoice.Status = "Nothing Sent";
            }
            else if (billStatusCounter == invoice.Roles.Count())
            {
                invoice.Status = "Full bill sent";
            }
            else if (billStatusCounter > 0 && billStatusCounter < invoice.Roles.Count())
            {
                invoice.Status = "Partially sent";
            }

            EmailCustomerModel invoiceToAdd = new EmailCustomerModel()
            {
                Name = invoice.Name,
                Email = invoice.Email,
                Roles = invoice.Roles,
                InvoiceDate = invoice.InvoiceDate,
                MailToSend = MailToSend,
                ProjectName = invoice.ProjectName,
                InvoiceNumber = invoice.InvoiceNumber,
                SentOrCancelDate = DateTime.Now.ToString(),
                Status = invoice.Status,
                Amount = invoice.Amount
            };

            

            collection.DeleteOne(Builders<EmailCustomerModel>.Filter.Eq("InvoiceNumber", invoice.InvoiceNumber));
            collection.InsertOne(invoiceToAdd);


            //pocetak send buffer-a
            var invoiceSendBuffer = DB.GetCollection<EmailCustomerModel>("InvoiceSendBuffer");

            invoiceToAdd.Roles = Roless;
            invoiceSendBuffer.InsertOne(invoiceToAdd);

            //StringBuilder invSendAppender = new StringBuilder();

            //foreach (var role in invoice.Roles)
            //{
            //    if (role.Status == "Sent")
            //    {
            //        string[] split = role.Description.TrimEnd().Split(' ');
            //        foreach (string s in split)
            //        {
            //            invSendAppender.Append(s.Substring(0, 1).ToUpper());
            //        }

            //        invoiceToAdd.InvoiceNumber = invoiceToAdd.InvoiceNumber + "-" + invSendAppender;

            //        //backupcollection.InsertOne(invoiceToAdd);
            //    }
            //    else { continue; }
            //}

            //invoiceToAdd.SentOrCancelDate = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString() + " at " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString();

            //invoiceSendBuffer.InsertOne(invoiceToAdd);
            //kraj send buffer-a


            //var DBackup = client.GetDatabase("BillingsBackup");
            var backupcollection = DB.GetCollection<EmailCustomerModel>("InvoicesBackup");

            //int exist = (int)backupcollection.Find(x => x.InvoiceNumber == invoice.InvoiceNumber).Count();

            StringBuilder invNumAppender = new StringBuilder();
            //if (exist == 0)
            //{

            foreach (var role in invoice.Roles)
            {
                if (role.Status == "Sent" || role.Status == "Canceled")
                {
                    string[] split = role.Description.TrimEnd().Split(' ');
                    foreach (string s in split)
                    {
                        invNumAppender.Append(s.Substring(0, 1).ToUpper());
                    }

                    invoiceToAdd.InvoiceNumber = invoiceToAdd.InvoiceNumber + "-" + invNumAppender;

                    //backupcollection.InsertOne(invoiceToAdd);
                }
                else { continue; }
            }

            invoiceToAdd.SentOrCancelDate = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString() + " at " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString();

            backupcollection.InsertOne(invoiceToAdd);
            //}

            return Ok(invoice);
        }



        //var nadji = collection.Find(Builders<EmailCustomerModel>.Filter.Where(yyyy => yyyy.InvoiceNumber == model.InvoiceNumber)).ToString();

        //var filter = "{ \"InvoiceNumber\" : \"36-TE-A-184/18\" }";

        //int exist = (int)collection.Find(x => x.InvoiceNumber == model.InvoiceNumber).Count();
        //if (exist == 0)
        //{

        //RoleInvoiceModel Roles = new RoleInvoiceModel();
        //Roles = iteracija(model.Roles),
        //List<RoleInvoiceModel> iteracija(List<RoleInvoiceModel> roles)
        //{
        //    foreach (var role in roles)
        //    {

        //    }
        //    return roles;
        //}
        //if (exist == 0)
        //{ }
        //var xy = collection.Find( x => x.InvoiceNumber == model.InvoiceNumber).ToListAsync();
        //var xy = collection.Find();
        //collection.DeleteOne(xy.Id);

        //var yz = xy.InvoiceNumber.ToString();
        //var filter = Builders<EmailCustomerModel>.Filter.Eq(yz, model.InvoiceNumber);
        //collection.ReplaceOne(model.InvoiceNumber, invoiceToAdd);


        //      collection.UpdateOne(
        //xy.Id,
        //invoiceToAdd,
        //new UpdateOptions {IsUpsert = true}
        //);}




    }
}