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
    public class PostTeam
    {
        [TestMethod]
        public void ControllerPostTeamWithValidData()
        {
            
            var controller = new TeamsController();

            var response = controller.Post(new Team()
            {
                Name = "Gama",
                Id = "G",
                Image = "G",
                Description = "Gama Team"
            });
            //var response = controller.Get("G");
            var result = (OkNegotiatedContentResult<TeamModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
       //[ExpectedException(typeof(InvalidCastException))]
        public void ControllerPostTeamWithInvalidId()
        {
            var controller = new TeamsController();

            var response = controller.Post(new Team()
            {
                Name = "lala",
                Image = "blabla",
                Description = "Gama Team",
               
            });

            
            var result = (BadRequestErrorMessageResult)response;

            Assert.IsNotNull(result);
          

        }
    }
}
