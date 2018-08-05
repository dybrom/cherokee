using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using Cherokee.API.Controllers;
using Cherokee.API.Models;
using System.Web.Http.Results;
using Cherokee.DAL.Entities;

namespace Cherokee.Test
{
    [TestClass]
    public class PutCategory
    {
        [TestMethod]
        public void ControllerPutCategoryValid()
        {
            var controller = new CategoriesController();



            var response = controller.Put(new Category()
            {
                Description = "Put test"
            }, 1);
            var result = (OkNegotiatedContentResult<CategoryModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }
        [TestMethod]
        public void ControllerPutCategoryInvalid()
        {
            var controller = new CategoriesController();

            var response = controller.Put(new Category()
            {
                Description = "bla"
            }, 1);
            var result = (BadRequestErrorMessageResult)response;

            Assert.IsNotNull(result);


        }
    }
}
