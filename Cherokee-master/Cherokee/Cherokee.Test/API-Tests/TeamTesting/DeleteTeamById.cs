using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Cherokee.API.Controllers;
using Cherokee.API.Models;
using System.Web.Http.Results;
using Cherokee.DAL.Entities;

namespace Cherokee.Test
{
    [TestClass]
    public class DeleteTeamById
    {
        [TestMethod]
        public void ControllerDeleteTeamWithValidID()
        {
            var controller = new TeamsController();

            controller.Post(new Team()
            {
                Name = "DeleteTeam",
                Id = "D",
                Image = "D",
                Description = "Delete Team"
            });

            var response = controller.Delete("D");
            var result = (OkResult)response;

            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void ControllerDeleteTeamWithInvalidID()
        {
            var controller = new TeamsController();

            var response = controller.Delete("|||");
            var result = (NotFoundResult)response;
            Assert.IsNotNull(result);

        }
    }
}
