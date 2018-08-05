using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using Cherokee.API.Controllers;
using Cherokee.API.Models;
using System.Web.Http.Results;
using Cherokee.DAL.Entities;

namespace Cherokee.Test
{
    [TestClass]
    public class PutEmployee
    {
        [TestMethod]
        public void ControllerPutEmployeeValid()
        {
            var controller = new EmployeesController();
            //var RoleController = new RolesController();

            var response = controller.Put(new Employee()
            {
                FirstName = "Put",
                LastName = "Test",
                Position = new Role { Id = "UX"}
            }, 1);
            var result = (OkNegotiatedContentResult<EmployeeModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }
        [TestMethod]
        public void ControllerPutEmployeeInvalid()
        {
            var controller = new EmployeesController();


            var response = controller.Put(new Employee()
            {
                FirstName = "u",
            }, 1);
            var result = (BadRequestErrorMessageResult)response;

            Assert.IsNotNull(result);


        }
    }
}
