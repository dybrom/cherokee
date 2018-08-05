using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Cherokee.API.Controllers;
using Cherokee.API.Models;
using System.Web.Http.Results;

namespace Cherokee.Test
{
    [TestClass]
    public class GetTeamById
    {
        [TestMethod]
        public void ControllerGetTeamByValidId()
        {
            var controller = new TeamsController();

            var response = controller.GetById("A");
            var result = (OkNegotiatedContentResult<TeamModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }
        [TestMethod]
        public void ControllerGetTeamByInvalidId()
        {
            var controller = new TeamsController();

            var response = controller.GetById("|||");
            var result = (NotFoundResult)response;
            Assert.IsNotNull(result);

        }
    }
}
