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
    public class PostEmployee
    {
        [TestMethod]
        public void ControllerPostEmployeeWithValidData()
        {
            var Rolecontroller = new RolesController();
            Rolecontroller.Post(new Role()
            {
                Id = "T",
                Name = "Test"
            });
            
            var controller = new EmployeesController();

            var response = controller.Post(new Employee()
            {
                FirstName = "Post",
                LastName = "Test",
                Position = new Role { Id = "SD" }
            });
            //var response = controller.Get("G");
            var result = (OkNegotiatedContentResult<EmployeeModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        //[ExpectedException(typeof(InvalidCastException))]
        public void ControllerPostEmployeeByInvalidId()
        {
            var controller = new EmployeesController();

            var response = controller.Post(new Employee()
            {
                FirstName = "Firstname",
                Position = new Role { Id = "SD"}

            });


            var result = (BadRequestErrorMessageResult)response;

            Assert.IsNotNull(result);


        }
    }
}
