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
    [RoutePrefix("api/teams")]
    public class TeamsController : BaseController
    {
        /// <summary>
        /// Get all teams
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult Get()
        {
            var list = TimeUnit.Teams.Get().ToList()
                                 .Select(t => TimeFactory.Create(t))
                                 .ToList();
            Utility.Log($"Get all data from TEAMS table", "INFO");
            return Ok(list);
        }
        /// <summary>
        /// Get  team by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        public IHttpActionResult GetById(string id)
        {
            Team team = TimeUnit.Teams.Get(id);
            if (team == null)
            {
                Utility.Log($"Failed to get data for team with ID = {id}", "ERROR");
                return NotFound();
            }
            else
            {
                Utility.Log($"Get data for team with ID = {id}", "INFO");
                return Ok(TimeFactory.Create(team));
            }
        }
        /// <summary>
        /// Insert team
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult Post([FromBody] Team team)
        {
            try
            {
                TimeUnit.Teams.Insert(team);
                TimeUnit.Save();
                Utility.Log($"Insert data for team {team.Name}", "INFO");
                return Ok(TimeFactory.Create(team));
            }
            catch(Exception ex)
            {
                Utility.Log(ex.Message, "ERROR", ex);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Update team by ID
        /// </summary>
        /// <param name="team"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        public IHttpActionResult Put([FromBody] Team team, string id)
        {
            try
            {
                Team oldteam = TimeUnit.Teams.Get(id);
                if (oldteam == null) return NotFound();

                team = FillNewTeamWithOldData(team, oldteam);
                TimeUnit.Teams.Update(team, id);
                TimeUnit.Save();
                Utility.Log($"Update data for team with ID = {id}", "INFO");
                return Ok(TimeFactory.Create(team));
            }
            catch(Exception ex)
            {
                Utility.Log(ex.Message, "ERROR", ex);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Delete team by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        public IHttpActionResult Delete(string id)
        {
            try
            {
                Team team = TimeUnit.Teams.Get(id);
                if (team == null) return NotFound();
                foreach(var eng in team.Engagements)
                {
                    TimeUnit.Engagements.Delete(eng.Id);
                    break;
                }
                //foreach (var proj in team.Projects)
                //{
                //    proj.Team.Id = TimeUnit.Teams.Get("Id benc tima");
                //}
                TimeUnit.Teams.Delete(team);
                TimeUnit.Save();
                Utility.Log($"Delete data for team with ID = {id}", "INFO");
                return Ok();
            }
            catch(Exception ex)
            {
                Utility.Log(ex.Message, "ERROR", ex);
                return BadRequest(ex.Message);
            }
        }
        private Team FillNewTeamWithOldData(Team newTeam, Team oldTeam)
        {
            newTeam.Id = oldTeam.Id;
            newTeam.CreatedBy = oldTeam.CreatedBy;
            newTeam.CreatedOn = oldTeam.CreatedOn;

            if (newTeam.Engagements.Count == 0 && oldTeam.Engagements.Count != 0)
                newTeam.Engagements = oldTeam.Engagements;
                //newTeam.Engagements = TimeUnit.Engagements.Get().Where(a=> a.Team.Id == newTeam.Id).ToList();

            if (newTeam.Projects.Count == 0 && oldTeam.Projects.Count != 0)
                newTeam.Projects = oldTeam.Projects;

            if (newTeam.Image == null && oldTeam.Image != null)
                newTeam.Image = oldTeam.Image;

            if (newTeam.Name == null && oldTeam.Name != null)
                newTeam.Name = oldTeam.Name;

            if (newTeam.Status == null && oldTeam.Status != null)
                newTeam.Status = oldTeam.Status;

            return newTeam;
            
        }


    }
}
