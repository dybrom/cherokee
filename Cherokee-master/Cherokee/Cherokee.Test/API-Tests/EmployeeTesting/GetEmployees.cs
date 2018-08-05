using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using Cherokee.API.Controllers;
using Cherokee.API.Models;
using System.Web.Http.Results;
using System.Web;
using System.IO;
using System.Collections.Generic;

namespace Cherokee.Test
{
    [TestClass]
    public class GetEmployees
    {
        [TestInitialize]
        public void Initialize()
        {
            HttpContext.Current = new HttpContext(
            new HttpRequest("", "http://tempuri.org", ""),
            new HttpResponse(new StringWriter()));
        }
        [TestMethod]
        public void ControllerGetEmployees()
        {
            var controller = new EmployeesController();

            var response = controller.Get();
            var result = (OkNegotiatedContentResult<List<EmployeeModel>>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }
       
    }
}
