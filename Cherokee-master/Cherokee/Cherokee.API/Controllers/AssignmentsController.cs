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
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace Cherokee.API.Controllers
{
    [RoutePrefix("api/assignments")]
    public class AssignmentsController : BaseController
    {
        /// <summary>
        /// Get all assignments
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult Get(int page = 0, int pageSize = 5, int sort = 0, string filter = "")
        {
            ISorting<Assignment, DetailModel> assignmentHelper = new AssignmentsSorting();
            var query = TimeUnit.Assignments.Get();

            query = assignmentHelper.Filter(query, filter);
            query = assignmentHelper.Sort(query, sort);
            var list = assignmentHelper.Paginate(query, pageSize, page);


            int totalItems = TimeUnit.Employees.Get().Count();
            var header = new Header(pageSize, assignmentHelper.TotalPages, page, sort, totalItems);
            HttpContext.Current.Response.AddHeader("Pagination", JsonConvert.SerializeObject(header));

            Utility.Log("Get all data from EMPLOYEES table", "INFO");
            return Ok(list);
        }
        /// <summary>
        /// Get assignment by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        public IHttpActionResult GetById(int id)
        {
            Assignment assignment = TimeUnit.Assignments.Get(id);
            if (assignment == null)
            {
                Utility.Log($"Failed to get data for ASSIGNMENT with ID = {id}", "ERROR");
                return NotFound();
            }
            else
            {
                Utility.Log($"Get data for ASSIGNMENT with ID = {id}", "INFO");
                return Ok(TimeFactory.Create(assignment));
            }
        }
        /// <summary>
        /// Insert assignment
        /// </summary>
        /// <param name="assignment"></param>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult Post([FromBody] Assignment assignment)
        {
            try
            {
                assignment.Day = TimeUnit.Days.Get(assignment.Day.Id);
                assignment.Project = TimeUnit.Projects.Get(assignment.Project.Id);
                TimeUnit.Assignments.Insert(assignment);
                TimeUnit.Save();
                Utility.Log($"Insert data for ASSIGNMENT {assignment.Description}", "INFO");
                return Ok(TimeFactory.Create(assignment));
            }
            catch (Exception ex)
            {
                Utility.Log(ex.Message, "ERROR", ex);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Update assignment by ID
        /// </summary>
        /// <param name="assignment"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        public IHttpActionResult Put([FromBody] Assignment assignment, int id)
        {
            try
            {
                Assignment oldAssignment = TimeUnit.Assignments.Get(id);
                if (oldAssignment == null) return NotFound();

                assignment = FillAssignmentWithOldData(assignment, oldAssignment);

                TimeUnit.Assignments.Update(assignment, id);
                TimeUnit.Save();
                Utility.Log($"Update data for ASSIGNMENT with ID = {id} ", "INFO");
                return Ok(TimeFactory.Create(assignment));
            }
            catch (Exception ex)
            {
                Utility.Log(ex.Message, "ERROR", ex);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Delete assignment by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                Assignment assignment = TimeUnit.Assignments.Get(id);
                if (assignment == null) return NotFound();
                TimeUnit.Assignments.Delete(assignment);
                TimeUnit.Save();
                Utility.Log($"Delete data for ASSIGNMENT with ID = {id}", "INFO");
                return Ok();
            }
            catch (Exception ex)
            {
                Utility.Log(ex.Message, "ERROR", ex);
                return BadRequest(ex.Message);
            }
        }
        private Assignment FillAssignmentWithOldData(Assignment newAssignment, Assignment oldAssignment)
        {
            newAssignment.Id = oldAssignment.Id;
            newAssignment.CreatedBy = oldAssignment.CreatedBy;
            newAssignment.CreatedOn = oldAssignment.CreatedOn;

            if (newAssignment.Day == null && oldAssignment.Day != null)
                newAssignment.Day = TimeUnit.Days.Get(oldAssignment.Day.Id);

            if (newAssignment.DayId == 0)
                newAssignment.DayId = newAssignment.Day.Id;

            if (newAssignment.Project == null && oldAssignment.Project != null)
                newAssignment.Project = TimeUnit.Projects.Get(oldAssignment.Project.Id);

            if (newAssignment.ProjectId == 0)
                newAssignment.ProjectId = newAssignment.Project.Id;

            if (newAssignment.Description == null && oldAssignment.Description != null)
                newAssignment.Description = oldAssignment.Description;

            if (newAssignment.Hours == null && oldAssignment.Hours != null)
                newAssignment.Hours = oldAssignment.Hours;


            return newAssignment;
        }
    }
}
