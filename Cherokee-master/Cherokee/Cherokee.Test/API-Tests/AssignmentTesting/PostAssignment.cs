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
//    public class PostAssignment
//    {
//        [TestMethod]
//        public void ControllerPostAssignmentWithValidData()
//        {

//            var controller = new AssignmentsController();

//            var response = controller.Post(new Assignment()
//            {
//                Description = "Post assignment test",
//                Project = new Project() {Id = 1},
//                Day = new Day() {Id = 1}
//            });

//            var result = (OkNegotiatedContentResult<AssignmentModel>)response;

//            Assert.IsNotNull(result);
//            Assert.IsNotNull(result.Content);
//        }

//        [TestMethod]
//        public void ControllerPostAssignmentWithInvalidId()
//        {
//            var controller = new AssignmentsController();

//            var response = controller.Post(new Assignment()
//            {
//                Project = new Project() { Id = 1 },
//                Day = new Day() { Id = 1 }
//            });


//            var result = (BadRequestErrorMessageResult)response;

//            Assert.IsNotNull(result);


//        }
//    }
//}
