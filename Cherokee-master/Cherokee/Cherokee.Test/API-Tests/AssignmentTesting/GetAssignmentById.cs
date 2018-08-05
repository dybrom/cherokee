//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using Cherokee.API.Controllers;
//using Cherokee.API.Models;
//using System.Web.Http.Results;

//namespace Cherokee.Test
//{
//    [TestClass]
//    public class GetAssignmentById
//    {
//        [TestMethod]
//        public void ControllerGetAssignmentByValidId()
//        {
//            var controller = new AssignmentsController();

//            var response = controller.GetById(1);
//            var result = (OkNegotiatedContentResult<AssignmentModel>)response;

//            Assert.IsNotNull(result);
//            Assert.IsNotNull(result.Content);
//        }
//        [TestMethod]
//        public void ControllerGetAssignmentByInvalidId()
//        {
//            var controller = new AssignmentsController();

//            var response = controller.GetById(1337);
//            var result = (NotFoundResult)response;
//            Assert.IsNotNull(result);

//        }
//    }
//}
