using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Cherokee.API.Controllers;
using Cherokee.API.Models;
using System.Web.Http.Results;
using System.Collections.Generic;

namespace Cherokee.Test
{
    [TestClass]
    public class GetCategories
    {
        [TestMethod]
        public void ControllerGetCategories()
        {
            var controller = new CategoriesController();

            var response = controller.Get();
            var result = (OkNegotiatedContentResult<List<CategoryModel>>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

    }
}
