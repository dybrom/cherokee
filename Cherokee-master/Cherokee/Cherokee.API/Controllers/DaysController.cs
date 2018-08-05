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

namespace Cherokee.API.Controllers
{
    [RoutePrefix("api/calendar")]
    public class CalendarController : BaseController
    {
        [Route("")]
        public IHttpActionResult Get(int id, int year = 0, int month = 0)
        {
            if (year == 0) year = DateTime.Today.Year;
            if (month == 0) month = DateTime.Today.Month;
            Employee emp = TimeUnit.Employees.Get(id);
            CalendarModel calendar = new CalendarModel(new BaseModel { Id = emp.Id, Name = emp.FullName }, year, month);

            var days = emp.Days.Where(x => x.Date.Value.Month == month && x.Date.Value.Year == year).ToList();
            int i;
            foreach (var day in days)
            {
                i = day.Date.Value.Day - 1;
                calendar.Days[i].Id = day.Id;
                calendar.Days[i].Type = day.Category.Id;
                calendar.Days[i].Hours = day.Hours.Value;
                calendar.Days[i].Details = day.Assignments.Select(x => TimeFactory.Create(x)).ToArray();
            }
            return Ok(calendar);
        }
        [Route("")]
        public IHttpActionResult Post([FromBody] DayModel model)
        {
            try
            {
                Day day = new Day
                {
                    Id = model.Id,
                    Date = model.Date,
                    Category  = TimeUnit.Categories.Get(model.Type),
                    Hours = model.Hours,
                    Employee = TimeUnit.Employees.Get(model.Employee.Id),
                    CategoryId = model.Type,
                    EmployeeId = model.Employee.Id
                };
                if (day.Id == 0)
                    TimeUnit.Days.Insert(day);
                else
                    TimeUnit.Days.Update(day, day.Id);
                TimeUnit.Save();

                foreach (DetailModel task in model.Details)
                {
                    if (task.Deleted)
                    {
                        TimeUnit.Assignments.Delete(task.Id);
                    }
                    else
                    {
                        Assignment detail = new Assignment
                        {
                            Id = task.Id,
                            Day = TimeUnit.Days.Get(day.Id),
                            DayId = day.Id,
                            Description = task.Description,
                            Hours = task.Hours,
                            Project = TimeUnit.Projects.Get(task.Project.Id),
                            ProjectId = task.Project.Id
                        };
                        if (detail.Id == 0)
                            TimeUnit.Assignments.Insert(detail);
                        else
                            TimeUnit.Assignments.Update(detail, detail.Id);
                    }
                }
                TimeUnit.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                Day day = TimeUnit.Days.Get(id);
                if (day == null) return NotFound();
                foreach(var ass in day.Assignments)
                {
                    TimeUnit.Days.Delete(ass.Id);
                    break;
                }
                TimeUnit.Days.Delete(day);
                TimeUnit.Save();
                Utility.Log($"Delete data for day with ID = {id}", "INFO");
                return Ok();
            }
            catch (Exception ex)
            {
                Utility.Log(ex.Message, "ERROR", ex);
                return BadRequest(ex.Message);
            }
        }
    }
}
