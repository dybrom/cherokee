using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using Cherokee.API.Controllers;
using Cherokee.API.Models;
using System.Web.Http.Results;
using Cherokee.DAL.Entities;

namespace Cherokee.Test
{
    [TestClass]
    public class PutRole
    {
        [TestMethod]
        public void ControllerPutRoleValid()
        {
            var controller = new RolesController();

 

            var response = controller.Put(new Role()
            {
                Name = "PUt role success"
            }, "UX");
            var result = (OkNegotiatedContentResult<RoleModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }
        [TestMethod]
        public void ControllerPutRoleInvalid()
        {
            var controller = new RolesController();

            var response = controller.Put(new Role()
            {
                Name = "aa"
            }, "UX");
            var result = (BadRequestErrorMessageResult)response;

            Assert.IsNotNull(result);


        }
    }
}
