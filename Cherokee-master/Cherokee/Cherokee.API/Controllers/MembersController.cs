using Cherokee.API.Models;
using Cherokee.DAL;
using Cherokee.DAL.Entities;
using Cherokee.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cherokee.API.Controllers
{
    [RoutePrefix("api/members")]
    public class MembersController : BaseController
    {
        /// <summary>
        /// Get all members
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult Get()
        {
            var list = TimeUnit.Engagements.Get().ToList()
                                 .Select(t => TimeFactory.Create(t))
                                 .ToList();
            Utility.Log($"Get all data from ENGAGEMENTS table", "INFO");
            return Ok(list);
        }
        /// <summary>
        /// Get  engagement by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        public IHttpActionResult GetById(int id)
        {
            Engagement engagement = TimeUnit.Engagements.Get(id);
            if (engagement == null)
            {
                Utility.Log($"Failed to get data for ENGAGEMENT with ID = {id}", "ERROR");
                return NotFound();
            }
            else
            {
                Utility.Log($"Get data for team with ID = {id}", "INFO");
                return Ok(TimeFactory.Create(engagement));
            }
        }
        /// <summary>
        /// Insert engagement
        /// </summary>
        /// <param name="engagement"></param>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult Post([FromBody] Engagement engagement)
        {
            try
            {
                engagement.Role = TimeUnit.Roles.Get(engagement.Role.Id);
                engagement.Team = TimeUnit.Teams.Get(engagement.Team.Id);
                engagement.Employee = TimeUnit.Employees.Get(engagement.Employee.Id);

                engagement.RoleId = engagement.Role.Id;
                engagement.TeamId = engagement.Team.Id;
                engagement.EmployeeId = engagement.Employee.Id;

                TimeUnit.Engagements.Insert(engagement);
                TimeUnit.Save();
                Utility.Log($"Insert data for ENGAGEMENT {engagement.Id}", "INFO");
                return Ok(TimeFactory.Create(engagement));
            }
            catch (Exception ex)
            {
                Utility.Log(ex.Message, "ERROR", ex);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Update engagement by ID
        /// </summary>
        /// <param name="engagement"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        public IHttpActionResult Put([FromBody] Engagement engagement, int id)
        {
            try
            {

                Engagement oldEngagement = TimeUnit.Engagements.Get(id);
                if (oldEngagement == null) return NotFound();
                engagement = FillEngagementWithOldData(engagement, oldEngagement);
                //engagement.Role = TimeUnit.Roles.Get(engagement.Role.Id);
                //engagement.Team = TimeUnit.Teams.Get(engagement.Team.Id);
                //engagement.Employee = TimeUnit.Employees.Get(engagement.Employee.Id);

                //engagement.RoleId = engagement.Role.Id;
                //engagement.TeamId = engagement.Team.Id;
                //engagement.EmployeeId = engagement.Employee.Id;
                TimeUnit.Engagements.Update(engagement, id);
                TimeUnit.Save();
                Utility.Log($"Update data for ENGAGEMENT with ID = {id}", "INFO");
                return Ok(TimeFactory.Create(engagement));
            }
            catch (Exception ex)
            {
                Utility.Log(ex.Message, "ERROR", ex);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Delete engagement by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                Engagement engagement = TimeUnit.Engagements.Get(id);
                if (engagement == null) return NotFound();
                TimeUnit.Engagements.Delete(engagement);
                TimeUnit.Save();
                Utility.Log($"Delete data for engagement with ID = {id}", "INFO");
                return Ok();
            }
            catch (Exception ex)
            {
                Utility.Log(ex.Message, "ERROR", ex);
                return BadRequest(ex.Message);
            }
        }

        private Engagement FillEngagementWithOldData(Engagement newEngagement, Engagement oldEngagement)
        {
            newEngagement.Id = oldEngagement.Id;
            newEngagement.CreatedBy = oldEngagement.CreatedBy;
            newEngagement.CreatedOn = oldEngagement.CreatedOn;

            if (newEngagement.Employee == null && oldEngagement.Employee != null)
                newEngagement.Employee = TimeUnit.Employees.Get(oldEngagement.Employee.Id);

            if (newEngagement.EmployeeId == 0)
                newEngagement.EmployeeId = newEngagement.Employee.Id;

            if (newEngagement.Role == null && oldEngagement.Role != null)
                newEngagement.Role = TimeUnit.Roles.Get(oldEngagement.Role.Id);

            if (newEngagement.RoleId == null)
                newEngagement.RoleId = newEngagement.Role.Id;

            if (newEngagement.Hours == null && oldEngagement.Hours != null)
                newEngagement.Hours = oldEngagement.Hours;

            if (newEngagement.Team == null && oldEngagement.Team != null)
                newEngagement.Team = TimeUnit.Teams.Get(oldEngagement.Team.Id);

            if (newEngagement.TeamId == null)
                newEngagement.TeamId = newEngagement.Team.Id;


            return newEngagement;
        }

    }
}
