using Cherokee.API.Controllers;
using Cherokee.API.Controllers.Helper;
using Cherokee.API.Models;
using Cherokee.DAL;
using Cherokee.DAL.Entities;
using Cherokee.DAL.Repositories;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace Cherokee.API.Controllers
{
    [RoutePrefix("api/projects")]
    public class ProjectsController : BaseController
    {
        /// <summary>
        /// Get all projects
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult Get(int page = 0, int pageSize = 50, int sort = 0, string filter = "")
        {
            ISorting<Project, ProjectModel> projectHelper = new ProjectsSorting();
            var query = TimeUnit.Projects.Get();

            query = projectHelper.Filter(query, filter);
            query = projectHelper.Sort(query, sort);
            var list = projectHelper.Paginate(query, pageSize, page);


            int totalItems = TimeUnit.Projects.Get().Count();
            var header = new Header(pageSize, projectHelper.TotalPages, page, sort, totalItems);
            HttpContext.Current.Response
                                       .AddHeader("Pagination", JsonConvert.SerializeObject(header));

            Utility.Log("Get all data from EMPLOYEES table", "INFO");
            return Ok(list);
        }
        /// <summary>
        /// Get project by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        public IHttpActionResult GetById(int id)
        {
            Project project = TimeUnit.Projects.Get(id);
            if (project == null)
            {
                Utility.Log($"Failed to get data for  project with ID = {id}", "ERROR");
                return NotFound();
            }
            else
            {
                Utility.Log($"Get data for project with ID = {id}", "INFO");
                return Ok(TimeFactory.Create(project));
            }
        }
        /// <summary>
        /// Insert project
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult Post([FromBody] Project project)
        {
            try
            {
                project.Customer = TimeUnit.Customers.Get(project.Customer.Id);
                project.Team = TimeUnit.Teams.Get(project.Team.Id);
                TimeUnit.Projects.Insert(project);
                TimeUnit.Save();
                Utility.Log($"Insert data for project {project.Name}", "INFO");
                return Ok(TimeFactory.Create(project));
            }
            catch (Exception ex)
            {
                Utility.Log(ex.Message, "ERROR", ex);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Update project by ID
        /// </summary>
        /// <param name="project"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        public IHttpActionResult Put([FromBody] Project project, int id)
        {
            try
            {
                Project oldProject = TimeUnit.Projects.Get(id);
                if (oldProject==null) return NotFound();
                project = FillNewProjectWithOldData(project, oldProject);


                //project.Customer = TimeUnit.Customers.Get(project.Customer.Id);
                //project.Team = TimeUnit.Teams.Get(project.Team.Id);
                //project.CustomerId = project.Customer.Id;
                //project.TeamId = project.Team.Id;
                TimeUnit.Projects.Update(project, id);
                TimeUnit.Save();
                Utility.Log($"Update data for project with ID = {id}", "INFO");
                return Ok(TimeFactory.Create(project));
            }
            catch (Exception ex)
            {
                Utility.Log(ex.Message, "ERROR", ex);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Delete project by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                Project project = TimeUnit.Projects.Get(id);
                if (project == null) return NotFound();
                foreach(var ass in project.Assignments)
                {
                    TimeUnit.Assignments.Delete(ass.Id);
                    break;
                }
                TimeUnit.Projects.Delete(project);
                TimeUnit.Save();
                Utility.Log($"Delete data for project with ID = {id}", "INFO");
                return Ok();
            }
            catch (Exception ex)
            {
                Utility.Log(ex.Message, "ERROR", ex);
                return BadRequest(ex.Message);
            }
        }
        private Project FillNewProjectWithOldData(Project newProject, Project oldProject)
        {
            newProject.Id = oldProject.Id;
            newProject.CreatedBy = oldProject.CreatedBy;
            newProject.CreatedOn = oldProject.CreatedOn;

            if (newProject.Monogram == null && oldProject.Monogram != null)
                newProject.Monogram = oldProject.Monogram;

            if (newProject.Name == null && oldProject.Name != null)
                newProject.Name = oldProject.Name;

            if (newProject.Assignments.Count == 0 && oldProject.Assignments.Count != 0)
                newProject.Assignments = oldProject.Assignments;

            if (newProject.BeginDate == null && oldProject.BeginDate != null)
                newProject.BeginDate = oldProject.BeginDate;

            if (newProject.Customer == null && oldProject.Customer != null)
                newProject.Customer = TimeUnit.Customers.Get(oldProject.Customer.Id);

            if (newProject.Description == null && oldProject.Description != null)
                newProject.Description = oldProject.Description;

            if (newProject.EndDate == null && oldProject.EndDate != null)
                newProject.EndDate = oldProject.EndDate;

            if (newProject.Team == null && oldProject.Team != null)
                newProject.Team = TimeUnit.Teams.Get(oldProject.Team.Id);

            if (newProject.TeamId == null)
                newProject.TeamId = newProject.Team.Id;

            if (newProject.CustomerId == 0)
                newProject.CustomerId = newProject.Customer.Id;

            if (newProject.Amount == null && oldProject.Amount != null)
                newProject.Amount = oldProject.Amount;
            return newProject;
        }
    }
}