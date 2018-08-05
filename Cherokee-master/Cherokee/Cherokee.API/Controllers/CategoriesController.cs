using Cherokee.API.Controllers;
using Cherokee.DAL;
using Cherokee.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Cherokee.API.Controllers
{
    [RoutePrefix("api/categories")]
    public class CategoriesController : BaseController
    {
        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult Get()
        {
            var list = TimeUnit.Categories.Get().ToList()
                               .Select(c => TimeFactory.Create(c)).ToList();
            Utility.Log($"Get all data from CATEGORIES table", "INFO");
            return Ok(list);
        }
        /// <summary>
        /// Get category by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        public IHttpActionResult GetById(int id)
        {
            Category category = TimeUnit.Categories.Get(id);

            if (category == null)
            {
                Utility.Log($"Failed to get data for category with ID = {id}", "ERROR");
                return NotFound();
            }
            else
            {
                Utility.Log($"Get data for category with ID = {id}", "INFO");
                return Ok(TimeFactory.Create(category));
            }
        }
        /// <summary>
        /// Insert category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult Post([FromBody] Category category)
        {
            try
            {
                TimeUnit.Categories.Insert(category);
                TimeUnit.Save();
                Utility.Log($"Insert data for category {category.Description}", "INFO");
                return Ok(TimeFactory.Create(category));
            }
            catch (Exception ex)
            {
                Utility.Log(ex.Message, "ERROR", ex);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Update category by ID
        /// </summary>
        /// <param name="category"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        public IHttpActionResult Put([FromBody] Category category, int id)
        {
            try
            {
                Category oldCategory = TimeUnit.Categories.Get(id);
                if (oldCategory == null) return NotFound();
                category = FillCategoryWithOldData(category, oldCategory);
                TimeUnit.Categories.Update(category, id);
                TimeUnit.Save();
                Utility.Log($"Update data for category with ID = {id}", "INFO");
                return Ok(TimeFactory.Create(category));
            }
            catch (Exception ex)
            {
                Utility.Log(ex.Message, "ERROR", ex);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Delete category by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                Category category = TimeUnit.Categories.Get(id);
                if (category == null) return NotFound();
                TimeUnit.Categories.Delete(category);
                TimeUnit.Save();
                Utility.Log($"Delete data for category with ID = {id}", "INFO");
                return Ok();
            }
            catch (Exception ex)
            {
                Utility.Log(ex.Message, "ERROR", ex);
                return BadRequest(ex.Message);
            }
        }
        private Category FillCategoryWithOldData(Category newCategory, Category oldCategory)
        {
            newCategory.Id = oldCategory.Id;
            newCategory.CreatedBy = oldCategory.CreatedBy;
            newCategory.CreatedOn = oldCategory.CreatedOn;

            if (newCategory.Days.Count == 0 && oldCategory.Days.Count != 0)
                newCategory.Days = oldCategory.Days;

            if (newCategory.Description == null && oldCategory.Description != null)
                newCategory.Description = oldCategory.Description;

            return newCategory;
        }
    }
}