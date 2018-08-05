//using Cherokee.API.Controllers;
//using Cherokee.API.Models;
//using Cherokee.DAL.Entities;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web.Http.Results;

//namespace Cherokee.Test
//{
//    [TestClass]
//    public class PostDay
//    {
//        [TestMethod]
//        public void ControllerPostDayWithValidData()
//        {

//            var controller = new DaysController();

//            var response = controller.Post(new Day() {
//                Date = DateTime.Now,
//                Hours = 20,
//                Comment = "Post test Day",
//                Employee = new Employee() { Id = 1},
//                Category = new Category() { Id = 1}
//            });
//            var result = (OkNegotiatedContentResult<DayModel>)response;

//            Assert.IsNotNull(result);
//            Assert.IsNotNull(result.Content);
//        }

//        [TestMethod]
//        public void ControllerPostDaytWithInvalidData()
//        {
//            var controller = new DaysController();

//            var response = controller.Post(new Day()
//            {
//                Date = DateTime.Now,
//                Hours = 20,
//                Comment = "Post test Day",

//            });

//            var result = (BadRequestErrorMessageResult)response;
//            Assert.IsNotNull(result);
//        }
//    }
//}

