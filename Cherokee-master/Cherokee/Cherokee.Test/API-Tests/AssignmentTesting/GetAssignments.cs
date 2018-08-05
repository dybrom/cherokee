//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using Cherokee.API.Controllers;
//using Cherokee.API.Models;
//using System.Web.Http.Results;
//using System.Collections.Generic;
//using System.Web;
//using System.IO;

//namespace Cherokee.Test
//{
//    [TestClass]
//    public class GetAssignments
//    {
//        [TestInitialize]
//        public void Initialize()
//        {
//            HttpContext.Current = new HttpContext(
//            new HttpRequest("", "http://tempuri.org", ""),
//            new HttpResponse(new StringWriter()));
//        }
//        [TestMethod]
//        public void ControllerGetAssignments()
//        {
//            var controller = new AssignmentsController();

//            var response = controller.Get();
//            var result = (OkNegotiatedContentResult<List<AssignmentModel>>)response;

//            Assert.IsNotNull(result);
//            Assert.IsNotNull(result.Content);
//        }

//    }
//}
