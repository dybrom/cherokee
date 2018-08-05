using Cherokee.API.Controllers;
using Cherokee.API.Models;
using Cherokee.DAL.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace Cherokee.Test
{
    [TestClass]
    public class PostCategory
    {
        [TestMethod]
        public void ControllerPostCategoryWithValidData()
        {

            var controller = new CategoriesController();

            var response = controller.Post(new Category()
            {
                Description = "Post category test"
            });

            var result = (OkNegotiatedContentResult<CategoryModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void ControllerPostCategoryWithInvalidId()
        {
            var controller = new CategoriesController();

            var response = controller.Post(new Category()
            {
                CreatedBy = 1,
                CreatedOn = new DateTime(2015,02,02)
            });


            var result = (BadRequestErrorMessageResult)response;

            Assert.IsNotNull(result);


        }
    }
}
