using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using Cherokee.API.Controllers;
using Cherokee.API.Models;
using System.Web.Http.Results;
using Cherokee.DAL.Entities;

namespace Cherokee.Test
{
    [TestClass]
    public class DeleteRoleById
    {
        [TestMethod]
        public void ControllerDeleteRoleWithValidID()
        {
            var controller = new RolesController();

            controller.Post(new Role()
            {
                Id = "DEL",
                Name = "Role to be deleted"
            });

            var response = controller.Delete("DEL");
            var result = (OkResult)response;

            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void ControllerDeleteRoleWithInvalidID()
        {
            var controller = new RolesController();

            var response = controller.Delete("|||");
            var result = (NotFoundResult)response;
            Assert.IsNotNull(result);

        }
    }
}
