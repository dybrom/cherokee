using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using Cherokee.API.Controllers;
using Cherokee.API.Models;
using System.Web.Http.Results;
using Cherokee.DAL.Entities;

namespace Cherokee.Test
{
    [TestClass]
    public class PutProject
    {
        [TestMethod]
        public void ControllerPutProjectValid()
        {
            var controller = new ProjectsController();
            

            var response = controller.Put(new Project()
            {
                Name = "Put test",
                Team = new Team() { Id = "A" },
                Customer = new Customer() { Id = 1 }
            }, 1);
            var result = (OkNegotiatedContentResult<ProjectModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }
        [TestMethod]
        public void ControllerPutProjectInvalid()
        {
            var controller = new ProjectsController();


            var response = controller.Put(new Project()
            {
                Name = "aaa"
            }, 1);
            var result = (BadRequestErrorMessageResult)response;

            Assert.IsNotNull(result);


        }
    }
}
