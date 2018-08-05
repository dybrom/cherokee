using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using Cherokee.API.Controllers;
using Cherokee.API.Models;
using System.Web.Http.Results;

namespace Cherokee.Test
{
    [TestClass]
    public class GetRoleById
    {
        [TestMethod]
        public void ControllerGetRoleByValidId()
        {
            var controller = new RolesController();

            var response = controller.GetById("UX");
            var result = (OkNegotiatedContentResult<RoleModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }
        [TestMethod]
        public void ControllerGetRoleByInvalidId()
        {
            var controller = new RolesController();

            var response = controller.GetById("|||");
            var result = (NotFoundResult)response;
            Assert.IsNotNull(result);

        }
    }
}
