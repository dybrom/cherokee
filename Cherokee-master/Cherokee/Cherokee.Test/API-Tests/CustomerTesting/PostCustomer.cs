using Cherokee.API.Controllers;
using Cherokee.API.Models;
using Cherokee.DAL.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace Cherokee.Test
{
    [TestClass]
    public class PostCustomer
    {
        [TestMethod]
        public void ControllerPostCustomerWithValidData()
        {
            var controller = new CustomersController();
            var response = controller.Post(new Customer()
            {
                Name = "First Customer",
                Address = { Street = "Ulica", ZipCode = "71000", City = "Sarajevo" }
            });
            var result = (OkNegotiatedContentResult<CustomerModel>)response;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void ControllerPostCustomerWithInvalidData()
        {
            var controller = new CustomersController();
            var response = controller.Post(new Customer()
            {
                Contact = "Contact"

            });
            var result = (BadRequestErrorMessageResult)response;
            Assert.IsNotNull(result);
        }

    }
}
