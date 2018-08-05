using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using Cherokee.API.Controllers;
using Cherokee.API.Models;
using System.Web.Http.Results;
using Cherokee.DAL.Entities;

namespace Cherokee.Test
{
    [TestClass]
    public class PutCustomer
    {
        [TestMethod]
        public void ControllerPutCustomerValid()
        {
            var controller = new CustomersController();



            var response = controller.Put(new Customer()
            {
                Name = "Put customer test"
            }, 1);
            var result = (OkNegotiatedContentResult<CustomerModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }
        [TestMethod]
        public void ControllerPutCustomerInvalid()
        {
            var controller = new CustomersController();

            var response = controller.Put(new Customer()
            {
                Name = "a"
            }, 1);
            var result = (BadRequestErrorMessageResult)response;

            Assert.IsNotNull(result);


        }
    }
}
