using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Cherokee.API.Controllers;
using Cherokee.API.Models;
using System.Web.Http.Results;

namespace Cherokee.Test
{
    [TestClass]
    public class GetEngagementById
    {
        [TestMethod]
        public void ControllerGetEngagementByValidId()
        {
            var controller = new MembersController();

            var response = controller.GetById(1);
            var result = (OkNegotiatedContentResult<MemberModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }
        [TestMethod]
        public void ControllerGetEngagementByInvalidId()
        {
            var controller = new MembersController();

            var response = controller.GetById(1337);
            var result = (NotFoundResult)response;
            Assert.IsNotNull(result);

        }
    }
}
