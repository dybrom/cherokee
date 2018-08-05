using Cherokee.API.Controllers;
using Cherokee.API.Models;
using Cherokee.DAL.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace Cherokee.Test
{
    [TestClass]
    public class PostEngagement
    {
        [TestMethod]
        public void ControllerPostEngagementWithValidData()
        {

            var controller = new MembersController();

            var response = controller.Post(new Engagement()
            {
                Hours = 20,
                Employee = new Employee() { Id = 1 },
                Team = new Team() { Id = "A" },
                Role = new Role() { Id = "SD" }
            });
            //var response = controller.Get("G");
            var result = (OkNegotiatedContentResult<MemberModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        //[ExpectedException(typeof(InvalidCastException))]
        public void ControllerPostEngagementWithInvalidData()
        {
            var controller = new MembersController();


            var response = controller.Post(new Engagement()
            {
                Employee = new Employee() { Id = 1 }
            });
            var result = (BadRequestErrorMessageResult)response;
            Assert.IsNotNull(result);


        }
    }
}
