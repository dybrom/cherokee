using Cherokee.API.Controllers.Helper;
using Cherokee.API.Models;
using Cherokee.DAL;
using Cherokee.DAL.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using Thinktecture.IdentityModel.WebApi;
//za employee treba stavit engagement : team, role , employee, hours
namespace Cherokee.API.Controllers
{
    //[TimeAuth("Administrator")]
    [RoutePrefix("api/employees")]
    public class EmployeesController : BaseController
    {
        

        /// <summary>
        /// Get employees by page, page size with sort and filter
        /// </summary>
        /// <returns></returns>

        // api/employees?page=3
        // api/employees?page=1&pagesize=20
        // API/EMPLOYEES/PAGE?PAGE=1&SORT=2
        // api/employees/page/1&sort=1
        [Route("")]
        public IHttpActionResult Get(int page = 0, int pageSize = 10, int sort = 0, string filter = "")
        {
            ISorting<Employee, EmployeeModel> employeeHelper = new EmployeesSorting();

            var query = TimeUnit.Employees.Get();

            query = employeeHelper.Filter(query, filter);
            query = employeeHelper.Sort(query, sort);
            var list = employeeHelper.Paginate(query, pageSize, page);

            int totalItems = TimeUnit.Employees.Get().Count();
            var header = new Header(pageSize, employeeHelper.TotalPages, page, sort, totalItems);
            HttpContext.Current.Response.AddHeader("Pagination", JsonConvert.SerializeObject(header));

            Utility.Log($"Get all data from EMPLOYEES table, page = {page}, pageSize = {pageSize}, " +
                $"sort = {sort}, filter = {filter}", "INFO");
            return Ok(list);
          


        }
        [Route("all")]
        public IHttpActionResult GetAll()
        {
            var list = TimeUnit.Employees.Get().ToList().Select(e => TimeFactory.Create(e)).ToList();
            return Ok(list);
        }

        /// <summary>
        /// Get employee by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
       // [ScopeAuthorize("read")]
        [Route("{id}")]
        public IHttpActionResult GetById(int id)
        {
            //Employee employee = TimeUnit.Employees.Get(id);
            //if (employee == null)
            //{
            //    Utility.Log($"Failed to get data for employee with ID = {id}", "ERROR");
            //    return NotFound();
            //}
            //else
            //{

            //    Utility.Log($"Get data for employee  with ID = {id} ", "INFO");
            //    return Ok(TimeFactory.Create(employee));
            //}
            //var claimsPrincipal = User as ClaimsPrincipal;
            //string username = claimsPrincipal.FindFirst
            //("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            Employee employee = TimeUnit.Employees.Get(x => x.Id == id).FirstOrDefault();
            if (employee == null)
            {
                Utility.Log($"Failed to get data for employee with ID = {id}", "ERROR");
                return NotFound();
            }
            else
            {
                
                Utility.Log($"Get data for employee  with ID = {id} ", "INFO");
                return Ok(TimeFactory.Create(employee));
            }

        }
        /// <summary>
        /// Insert employee
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult Post([FromBody] Employee employee)
        {
            try
            {
                employee.Position = TimeUnit.Roles.Get(employee.Position.Id);
                employee.Image = employee.ConvertAndSave();
                TimeUnit.Employees.Insert(employee);
                TimeUnit.Save();
                Utility.Log($"Insert data for employee {employee.FullName}", "INFO");
                return Ok(TimeFactory.Create(employee));
            }
            catch (Exception ex)
            {
                Utility.Log(ex.Message, "ERROR", ex);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Update employee by ID
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        public IHttpActionResult Put([FromBody] Employee employee, int id)
        {
            try
            {
                Employee oldEmployee = TimeUnit.Employees.Get(id);
                if (oldEmployee == null) return NotFound();

                employee = FillEmployeeWithOldData(employee, oldEmployee);

                employee.Image = employee.ConvertAndSave();
                TimeUnit.Employees.Update(employee, id);
                TimeUnit.Save();
                Utility.Log($"Update data for employee {employee.FullName}", "INFO");
                return Ok(TimeFactory.Create(employee));
            }
            catch (Exception ex)
            {
                Utility.Log(ex.Message, "ERROR", ex);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Delete employee by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
       [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                Employee employee = TimeUnit.Employees.Get(id);
                if (employee == null) return NotFound();
                //CalendarController c = new CalendarController();
                //foreach (var day in employee.Days)
                //{
                //    c.(day.Id);
                //}
                foreach (var eng in employee.Engagements)
                {
                    TimeUnit.Engagements.Delete(eng.Id);
                    break;
                }
                foreach (var day in employee.Days)
                {
                    TimeUnit.Days.Delete(day.Id);
                    break;
                }

                TimeUnit.Employees.Delete(employee);
                TimeUnit.Save();
                Utility.Log($"Delete data for employee with ID = {id}", "INFO");
                return Ok();
            }
            catch (Exception ex)
            {
                Utility.Log(ex.Message, "ERROR", ex);
                return BadRequest(ex.Message);
            }
        }
        private Employee FillEmployeeWithOldData(Employee newEmployee, Employee oldEmployee)
        {
            newEmployee.Id = oldEmployee.Id;
            newEmployee.CreatedBy = oldEmployee.CreatedBy;
            newEmployee.CreatedOn = oldEmployee.CreatedOn;


            if (newEmployee.Days.Count == 0 && oldEmployee.Days.Count != 0)
                newEmployee.Days = oldEmployee.Days;

            if (newEmployee.Engagements.Count == 0 && oldEmployee.Engagements.Count != 0)
                newEmployee.Engagements = oldEmployee.Engagements;

            if (newEmployee.Position == null && oldEmployee.Position != null)
                newEmployee.Position = TimeUnit.Roles.Get(oldEmployee.Position.Id);

            if (newEmployee.PositionId == null)
                newEmployee.PositionId = newEmployee.Position.Id;

            //if (newEmployee.Image == null && oldEmployee.Image != null)
                //newEmployee.Image = oldEmployee.Image;

            if (newEmployee.FirstName == null && oldEmployee.FirstName != null)
                newEmployee.FirstName = oldEmployee.FirstName;

            if (newEmployee.LastName == null && oldEmployee.LastName != null)
                newEmployee.LastName = oldEmployee.LastName;

            if (newEmployee.Phone == null && oldEmployee.Phone != null)
                newEmployee.Phone = oldEmployee.Phone;

            if (newEmployee.Salary == null && oldEmployee.Salary != null)
                newEmployee.Salary = oldEmployee.Salary;

            if (newEmployee.Status == null && oldEmployee.Status != null)
                newEmployee.Status = oldEmployee.Status;

            if (newEmployee.Email == null && oldEmployee.Email != null)
                newEmployee.Email = oldEmployee.Email;

            if (newEmployee.BeginDate == null && oldEmployee.BeginDate != null)
                newEmployee.BeginDate = oldEmployee.BeginDate;

            if (newEmployee.BirthDate == null && oldEmployee.BirthDate != null)
                newEmployee.BirthDate = oldEmployee.BirthDate;

            if (newEmployee.EndDate == null && oldEmployee.EndDate != null)
                newEmployee.EndDate = oldEmployee.EndDate;
            if (newEmployee.Password == null && oldEmployee.Password != null)
                newEmployee.Password = oldEmployee.Password;
            return newEmployee;
        }
    }
}
