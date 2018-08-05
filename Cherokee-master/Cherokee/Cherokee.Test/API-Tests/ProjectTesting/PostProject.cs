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
    public class PostProject
    {
        [TestMethod]
        public void ControllerPostProjectWithValidData()
        {

            var controller = new ProjectsController();

            var response = controller.Post(new Project()
            {
                Name = "POst Project Test",
                Description = "Something about the Test project",
                Team = new Team() { Id = "A"},
                Customer = new Customer() { Id = 1}
            });
            //var response = controller.Get("G");
            var result = (OkNegotiatedContentResult<ProjectModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void ControllerPostProjectWithInvalidData()
        {
            var controller = new ProjectsController();

            var response = controller.Post(new Project()
            {
                Description = "Something about the Test project"

            });
            var result = (BadRequestErrorMessageResult)response;
            Assert.IsNotNull(result);
        }
    }
}

