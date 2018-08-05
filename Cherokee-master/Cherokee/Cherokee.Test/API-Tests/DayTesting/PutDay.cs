//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;

//using Cherokee.API.Controllers;
//using Cherokee.API.Models;
//using System.Web.Http.Results;
//using Cherokee.DAL.Entities;

//namespace Cherokee.Test
//{
//    [TestClass]
//    public class PutDay
//    {
//        [TestMethod]
//        public void ControllerPutDayValid()
//        {
//            var controller = new DaysController();
//            controller.Post(new Day()
//            {
//                Date = DateTime.Now,
//                Hours = 20,
//                Comment = "Day for put test ",
//                Employee = new Employee() { Id = 1 },
//                Category = new Category() { Id = 1 }
//            });

//            var response = controller.Put(new Day()
//            {
//                Date = DateTime.Now,
//                Hours = 35,
//                Comment = "Day for put updated",
//                Employee = new Employee() { Id = 1 },
//                Category = new Category() { Id = 1 }
//            }, 1);
//            var result = (OkNegotiatedContentResult<DayModel>)response;

//            Assert.IsNotNull(result);
//            Assert.IsNotNull(result.Content);
//        }
//        [TestMethod]
//        public void ControllerPutDayInvalid()
//        {
//            var controller = new DaysController();


//            var response = controller.Put(new Day()
//            {
//                Hours = 0
//            }, 1);
//            var result = (BadRequestErrorMessageResult)response;

//            Assert.IsNotNull(result);


//        }
//    }
//}
