using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using Cherokee.API.Controllers;
using Cherokee.API.Models;
using System.Web.Http.Results;
using Cherokee.DAL.Entities;

namespace Cherokee.Test
{
    [TestClass]
    public class DeleteEngagementById
    {
        [TestMethod]
        public void ControllerDeleteEngagementWithValidID()
        {
            var controller = new MembersController();
            var response = controller.Delete(2);
            var result = (OkResult)response;

            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void ControllerDeleteEngagementWithInvalidID()
        {
            var controller = new MembersController();

            var response = controller.Delete(1337);
            var result = (NotFoundResult)response;
            Assert.IsNotNull(result);

        }
    }
}
