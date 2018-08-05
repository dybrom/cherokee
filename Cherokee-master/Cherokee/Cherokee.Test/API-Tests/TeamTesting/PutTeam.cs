using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Cherokee.API.Controllers;
using Cherokee.API.Models;
using System.Web.Http.Results;
using Cherokee.DAL.Entities;

namespace Cherokee.Test
{
    [TestClass]
    public class PutTeam
    {
        [TestMethod]
        public void ControllerPutTeamValid()
        {
            var controller = new TeamsController();

            controller.Post(new Team()
            {
                Name = "PutTeamTest",
                Id = "P",
                Image = "P",
                Description = "Put Team"
            });

            var response = controller.Put(new Team()
            {
                Name = "Gama1111",
                Image = "G",
                Description = "Gama Team Changed"
            }, "P");
            var result = (OkNegotiatedContentResult<TeamModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }
        [TestMethod]
        public void ControllerPutTeamInvalid()
        {
            var controller = new TeamsController();

          

            var response = controller.Put(new Team()
            {
                Image = "G",
                Description = "Gama Team Changed",
                Name = "bla"
            }, "P");
            var result = (BadRequestErrorMessageResult)response;

            Assert.IsNotNull(result);


        }
    }
}
