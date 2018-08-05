//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using Cherokee.API.Controllers;
//using Cherokee.API.Models;
//using System.Web.Http.Results;

//namespace Cherokee.Test
//{
//    [TestClass]
//    public class GetDayById
//    {
//        [TestMethod]
//        public void ControllerGetDayByValidId()
//        {
//            var controller = new DaysController();

//            var response = controller.GetById(1);
//            var result = (OkNegotiatedContentResult<DayModel>)response;

//            Assert.IsNotNull(result);
//            Assert.IsNotNull(result.Content);
//        }
//        [TestMethod]
//        public void ControllerGetDayByInvalidId()
//        {
//            var controller = new DaysController();

//            var response = controller.GetById(1337);
//            var result = (NotFoundResult)response;
//            Assert.IsNotNull(result);

//        }
//    }
//}
