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
    public class GetProjects
    {
        [TestInitialize]
        public void Initialize()
        {
            HttpContext.Current = new HttpContext(
            new HttpRequest("", "http://tempuri.org", ""),
            new HttpResponse(new StringWriter()));
        }
        [TestMethod]
        public void ControllerGetProjects()
        {
            var controller = new ProjectsController();

            var response = controller.Get();
            var result = (OkNegotiatedContentResult<List<ProjectModel>>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

    }
}
