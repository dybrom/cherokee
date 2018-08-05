using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Cherokee.API.Controllers;
using Cherokee.API.Models;
using System.Web.Http.Results;

namespace Cherokee.Test
{
    [TestClass]
    public class GetCustomerById
    {
        [TestMethod]
        public void ControllerGetCustomerByValidId()
        {
            var controller = new CustomersController();

            var response = controller.GetById(1);
            var result = (OkNegotiatedContentResult<CustomerModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }
        [TestMethod]
        public void ControllerGetCustomerByInvalidId()
        {
            var controller = new CustomersController();

            var response = controller.GetById(1337);
            var result = (NotFoundResult)response;
            Assert.IsNotNull(result);

        }
    }
}
