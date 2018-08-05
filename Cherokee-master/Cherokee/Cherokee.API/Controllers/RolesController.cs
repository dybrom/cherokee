
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
// ovo dole je komentar za swagger :D
namespace Cherokee.API.Controllers
{
    [RoutePrefix("api/roles")]
    public class RolesController : BaseController
    {
        /// <summary>
        /// Get all roles 
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult Get()
        {
            var list = TimeUnit.Roles.Get().ToList()
                                 .Select(r => TimeFactory.Create(r))
                                 .ToList();
            Utility.Log($"Get all data from ROLES table", "INFO");
            return Ok(list);
        }
        //[Route("{type}")]
        //public IHttpActionResult Get(int type)
        //{
        //    List<RoleModel> list = TimeUnit.Roles.Get(x => x.Type == (RoleType)type).Select(r => TimeFactory.Create(r)).ToList();
        //        return Ok(list);
        //}

        /// <summary>
        /// Get role by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        public IHttpActionResult GetById(string id)
        {
            Role role = TimeUnit.Roles.Get(id);
            if (role == null)
            {
                Utility.Log($"Failed to get data for role with ID = {id}", "ERROR");
                return NotFound();
            }
            else
            {
                Utility.Log($"Get data for role with ID = {id}", "INFO");
                return Ok(TimeFactory.Create(role));
            }
        }
        /// <summary>
        /// Insert role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult Post([FromBody] Role role)
        {
            try
            {
                TimeUnit.Roles.Insert(role);
                TimeUnit.Save();
                Utility.Log($"Insert data for role {role.Name}", "INFO");
                return Ok(TimeFactory.Create(role));
            }
            catch (Exception ex)
            {
                Utility.Log(ex.Message, "ERROR", ex);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Update role by ID
        /// </summary>
        /// <param name="role"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        public IHttpActionResult Put([FromBody] Role role, string id)
        {
            try
            {
                Role oldRole = TimeUnit.Roles.Get(id);
                if (oldRole == null) return NotFound();
                role = FillRoleWithOldData(role, oldRole);
                TimeUnit.Roles.Update(role, id);
                TimeUnit.Save();
                Utility.Log($"Update data for role with ID = {id}", "INFO");
                return Ok(TimeFactory.Create(role));
            }
            catch (Exception ex)
            {
                Utility.Log(ex.Message, "ERROR", ex);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Delete role by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        public IHttpActionResult Delete(string id)
        {
            try
            {
                Role role = TimeUnit.Roles.Get(id);
                if (role == null) return NotFound();
                TimeUnit.Roles.Delete(role);
                TimeUnit.Save();
                Utility.Log($"Delete data for role with ID = {id}", "INFO");
                return Ok();
            }
            catch (Exception ex)
            {
                Utility.Log(ex.Message, "ERROR", ex);
                return BadRequest(ex.Message);
            }
        }
        private Role FillRoleWithOldData(Role newRole, Role oldRole)
        {
            newRole.Id = oldRole.Id;
            newRole.CreatedBy = oldRole.CreatedBy;
            newRole.CreatedOn = oldRole.CreatedOn;

            if (newRole.Engagements.Count == 0 && oldRole.Engagements.Count != 0)
                newRole.Engagements = oldRole.Engagements;

            if (newRole.Positions.Count == 0 && oldRole.Engagements.Count != 0)
                newRole.Positions = oldRole.Positions;

            if (newRole.HourlyRate == null && oldRole.HourlyRate != null)
                newRole.HourlyRate = oldRole.HourlyRate;

            if (newRole.MonthlyRate == null && oldRole.MonthlyRate != null)
                newRole.MonthlyRate = oldRole.MonthlyRate;

            if (newRole.Name == null && oldRole.Name != null)
                newRole.Name = oldRole.Name;

            if (newRole.Type == null && oldRole.Type != null)
                newRole.Type = oldRole.Type;
            return newRole;
        }

    }
}
