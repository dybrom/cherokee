//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;

//using Cherokee.API.Controllers;
//using Cherokee.API.Models;
//using System.Web.Http.Results;
//using Cherokee.DAL.Entities;

//namespace Cherokee.Test
//{
//    [TestClass]
//    public class PutAssignment
//    {
//        [TestMethod]
//        public void ControllerPutAssignmentValid()
//        {
//            var controller = new AssignmentsController();
//            var response = controller.Put(new Assignment()
//            {
//                Description = "Put test",
//                Project = new Project() { Id = 1},
//                Day = new Day() { Id = 1}
//            }, 1);
//            var result = (OkNegotiatedContentResult<AssignmentModel>)response;

//            Assert.IsNotNull(result);
//            Assert.IsNotNull(result.Content);
//        }
//        [TestMethod]
//        public void ControllerPutAssignmentInvalid()
//        {
//            var controller = new AssignmentsController();

//            var response = controller.Put(new Assignment()
//            {
//                Description = ""
//            }, 1);
//            var result = (BadRequestErrorMessageResult)response;

//            Assert.IsNotNull(result);


//        }
//    }
//}
