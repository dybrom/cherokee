using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using Cherokee.API.Controllers;
using Cherokee.API.Models;
using System.Web.Http.Results;
using System.Collections.Generic;

namespace Cherokee.Test
{
    [TestClass]
    public class GetRoles
    {
        [TestMethod]
        public void ControllerGetRoles()
        {
            var controller = new RolesController();

            var response = controller.Get();
            var result = (OkNegotiatedContentResult<List<RoleModel>>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

    }
}
