using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Cherokee.API.Controllers;
using Cherokee.API.Models;
using System.Web.Http.Results;
using System.Collections.Generic;

namespace Cherokee.Test
{
    [TestClass]
    public class GetTeams
    {
        [TestMethod]
        public void ControllerGetTeams()
        {
            var controller = new TeamsController();

            var response = controller.Get();
            var result = (OkNegotiatedContentResult<List<TeamModel>>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }
       
    }
}
