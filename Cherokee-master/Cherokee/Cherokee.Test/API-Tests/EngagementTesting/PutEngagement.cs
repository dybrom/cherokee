using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Cherokee.API.Controllers;
using Cherokee.API.Models;
using System.Web.Http.Results;
using Cherokee.DAL.Entities;

namespace Cherokee.Test
{
    [TestClass]
    public class PutEngagement
    {
        [TestMethod]
        public void ControllerPutEngagementValid()
        {
            var controller = new MembersController();


            var response = controller.Put(new Engagement()
            {
                Hours = 30,
                Employee = new Employee() { Id = 3 },
                Team = new Team() { Id = "A" },
                Role = new Role() { Id = "SD" }
            }, 1);

            var result = (OkNegotiatedContentResult<MemberModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }
        [TestMethod]
        public void ControllerPutEngagementInvalid()
        {
            var controller = new MembersController();
            var response = controller.Put(new Engagement()
            {

                Hours = 0
            }, 1);

            var result = (BadRequestErrorMessageResult)response;

            Assert.IsNotNull(result);


        }
    }
}
