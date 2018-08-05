using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using Cherokee.API.Controllers;
using Cherokee.API.Models;
using System.Web.Http.Results;
using Cherokee.DAL.Entities;

namespace Cherokee.Test
{
    [TestClass]
    public class DeleteEmployeeById
    {
        [TestMethod]
        public void ControllerDeleteEmployeeWithValidID()
        {
            var controller = new EmployeesController();
            var response = controller.Delete(2);
            var result = (OkResult)response;

            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void ControllerDeleteEmployeeWithInvalidID()
        {
            var controller = new EmployeesController();

            var response = controller.Delete(1337);
            var result = (NotFoundResult)response;
            Assert.IsNotNull(result);

        }
    }
}
