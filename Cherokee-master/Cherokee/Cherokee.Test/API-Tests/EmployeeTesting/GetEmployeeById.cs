using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using Cherokee.API.Controllers;
using Cherokee.API.Models;
using System.Web.Http.Results;

namespace Cherokee.Test
{
    [TestClass]
    public class GetEmployeeById
    {
        [TestMethod]
        public void ControllerGetEmployeeByValidId()
        {
            var controller = new EmployeesController();

            var response = controller.GetById(1);
            var result = (OkNegotiatedContentResult<EmployeeModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }
        [TestMethod]
        public void ControllerGetEmployeeByInvalidId()
        {
            var controller = new EmployeesController();

            var response = controller.GetById(1337);
            var result = (NotFoundResult)response;
            Assert.IsNotNull(result);

        }
    }
}
