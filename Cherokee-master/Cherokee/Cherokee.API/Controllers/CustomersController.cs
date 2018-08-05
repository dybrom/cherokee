using Cherokee.DAL.Entities;
using System;
using System.Collections.Generic;
using Cherokee.DAL.Repositories;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cherokee.DAL;
using Cherokee.API.Models;
using Cherokee.API.Controllers.Helper;
using System.Web;
using Newtonsoft.Json;

namespace Cherokee.API.Controllers
{
    [RoutePrefix("api/customers")]
    public class CustomersController : BaseController
    {
        /// <summary>
        /// Get all customers
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult Get(int page = 0, int pageSize = 10, int sort = 0, string filter = "")
        {
            ISorting<Customer, CustomerModel> customerHelper = new CustomersSorting();
            var query = TimeUnit.Customers.Get();

            query = customerHelper.Filter(query, filter);
            query = customerHelper.Sort(query, sort);
            var list = customerHelper.Paginate(query, pageSize, page);


            int totalItems = TimeUnit.Customers.Get().Count();
            var header = new Header(pageSize, customerHelper.TotalPages, page, sort, totalItems);
            HttpContext.Current.Response
                                       .AddHeader("Pagination", JsonConvert.SerializeObject(header));

            Utility.Log("Get all data from EMPLOYEES table", "INFO");
            return Ok(list);
        }
        /// <summary>
        /// Get customer by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        public IHttpActionResult GetById(int id)
        {
            Customer customer = TimeUnit.Customers.Get(id);

            if (customer == null)
            {
                Utility.Log($"Failed to get data for customer with ID = {id}", "ERROR");
                return NotFound();
            }
            else
            {
                Utility.Log($"Get data for customer with ID = {id}", "INFO");
                return Ok(TimeFactory.Create(customer));
            }
        }
        /// <summary>
        /// Insert customer
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult Post([FromBody] Customer customer)
        {
            try
            {
                TimeUnit.Customers.Insert(customer);
                TimeUnit.Save();
                Utility.Log($"Insert data for customer with ID = {customer.Id}", "INFO");
                return Ok(TimeFactory.Create(customer));
            }
            catch (Exception ex)
            {
                Utility.Log(ex.Message, "ERROR", ex);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Update customer by ID
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        public IHttpActionResult Put([FromBody] Customer customer, int id)
        {
            try
            {
                Customer oldCustomer = TimeUnit.Customers.Get(id);
                if (oldCustomer == null) return NotFound();

                customer = FillCustomerWithOldData(customer, oldCustomer);
                TimeUnit.Customers.Update(customer, id);
                TimeUnit.Save();
                Utility.Log($"Update data for customer with ID = {id}", "INFO");
                return Ok(TimeFactory.Create(customer));
            }
            catch (Exception ex)
            {
                Utility.Log(ex.Message, "ERROR", ex);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Delete customer by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                Customer customer = TimeUnit.Customers.Get(id);
                if (customer == null) return NotFound();
                foreach(var proj in customer.Projects)
                {
                    TimeUnit.Projects.Delete(proj.Id);
                    break;
                }
                TimeUnit.Customers.Delete(customer);
                TimeUnit.Save();
                Utility.Log($"Delete data for customer with ID = {id}", "INFO");
                return Ok();
            }
            catch (Exception ex)
            {
                Utility.Log(ex.Message, "ERROR", ex);
                return BadRequest(ex.Message);
            }
        }

        private Customer FillCustomerWithOldData(Customer newCustomer, Customer oldCustomer)
        {
            newCustomer.Id = oldCustomer.Id;
            newCustomer.CreatedBy = oldCustomer.CreatedBy;
            newCustomer.CreatedOn = oldCustomer.CreatedOn;

            if (newCustomer.Address == null && oldCustomer.Address != null)
                newCustomer.Address = oldCustomer.Address;

            if (newCustomer.Address.City == null && oldCustomer.Address.City != null)
                newCustomer.Address.City = oldCustomer.Address.City;

            if (newCustomer.Address.Street == null && oldCustomer.Address.Street != null)
                newCustomer.Address.Street = oldCustomer.Address.Street;

            if (newCustomer.Address.ZipCode == null && oldCustomer.Address.ZipCode != null)
                newCustomer.Address.ZipCode = oldCustomer.Address.ZipCode;

            if (newCustomer.Projects.Count == 0 && oldCustomer.Projects.Count != 0)
                newCustomer.Projects = oldCustomer.Projects;

            if (newCustomer.Contact == null && oldCustomer.Contact != null)
                newCustomer.Contact = oldCustomer.Contact;

            if (newCustomer.Email == null && oldCustomer.Email != null)
                newCustomer.Email = oldCustomer.Email;

            if (newCustomer.Image == null && oldCustomer.Image != null)
                newCustomer.Image = oldCustomer.Image;

            if (newCustomer.Monogram == null && oldCustomer.Monogram != null)
                newCustomer.Monogram = oldCustomer.Monogram;

            if (newCustomer.Name == null && oldCustomer.Name != null)
                newCustomer.Name = oldCustomer.Name;

            if (newCustomer.Phone == null && oldCustomer.Phone != null)
                newCustomer.Phone = oldCustomer.Phone;

            if (newCustomer.Status == null && oldCustomer.Status != null)
                newCustomer.Status = oldCustomer.Status;

            return newCustomer;
        }

    }
}