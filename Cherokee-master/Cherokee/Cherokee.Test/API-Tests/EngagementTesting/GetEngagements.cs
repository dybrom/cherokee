using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Cherokee.API.Controllers;
using Cherokee.API.Models;
using System.Web.Http.Results;
using System.Collections.Generic;

namespace Cherokee.Test
{
    [TestClass]
    public class GetEngagements
    {
        [TestMethod]
        public void ControllerGetEngagements()
        {
            var controller = new MembersController();

            var response = controller.Get();
            var result = (OkNegotiatedContentResult<List<MemberModel>>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);

        }

    }
}
