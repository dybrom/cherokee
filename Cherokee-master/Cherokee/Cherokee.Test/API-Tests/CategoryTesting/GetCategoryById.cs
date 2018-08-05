using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Cherokee.API.Controllers;
using Cherokee.API.Models;
using System.Web.Http.Results;

namespace Cherokee.Test
{
    [TestClass]
    public class GetCategoryById
    {
        [TestMethod]
        public void ControllerGetCategoryByValidId()
        {
            var controller = new CategoriesController();

            var response = controller.GetById(1);
            var result = (OkNegotiatedContentResult<CategoryModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }
        [TestMethod]
        public void ControllerGetCategoryByInvalidId()
        {
            var controller = new CategoriesController();

            var response = controller.GetById(1337);
            var result = (NotFoundResult)response;
            Assert.IsNotNull(result);

        }
    }
}
