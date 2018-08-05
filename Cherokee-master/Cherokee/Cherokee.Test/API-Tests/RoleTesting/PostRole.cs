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
    public class PostRole
    {
        [TestMethod]
        public void ControllerPostRoleWithValidData()
        {

            var controller = new RolesController();

            var response = controller.Post(new Role()
            {
                Id = "AT",
                Name = "Adi Type",
                Type = RoleType.TeamRole,
                HourlyRate = 30,
                MonthlyRate = 4500
            });
            //var response = controller.Get("G");
            var result = (OkNegotiatedContentResult<RoleModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void ControllerPostRoleWithInvalidId()
        {
            var controller = new RolesController();

            var response = controller.Post(new Role()
            {
                Id = "AT",
                Type = RoleType.TeamRole,
                HourlyRate = 30,
                MonthlyRate = 4500

            });


            var result = (BadRequestErrorMessageResult)response;

            Assert.IsNotNull(result);


        }
    }
}
