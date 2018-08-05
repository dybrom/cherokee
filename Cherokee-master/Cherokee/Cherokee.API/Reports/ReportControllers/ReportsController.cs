using Cherokee.API.Controllers.Helper;
using Cherokee.API.Reports;
using Cherokee.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Cherokee.API.Controllers
{
    
    public class ReportsController : BaseController
    {
        [Route("api/reports/annual/{year}")]
        public IHttpActionResult GetAnnualReport(int year = 0)
        {
            if (year == 0) year = DateTime.Today.Year;
            return Ok(new
            {
                year,
                list = TimeReports.AnnualReport(year)
            });
        }

        [Route("api/reports/personal/{employeeId}/{year}/{month}")]
        public IHttpActionResult GetPersonalReportForEmployee(int employeeId, int year, int month)
        {
            if (TimeUnit.Employees.Get(employeeId) == null)
                return NotFound();
            return Ok(TimeUnit.GetPersonalReport(employeeId, year, month, TimeFactory));
        }

        [Route("api/reports/monthly/{year}/{month}")]
        public IHttpActionResult GetMonthlyReport(int year = 0, int month = 0)
        {
            if (year == 0) year = DateTime.Today.Year;
            if (month == 0) month = DateTime.Today.Month;
            return Ok(TimeReports.MonthlyReport(year, month));
        }

        [Route("api/reports/ProjectsHistory")]
        public IHttpActionResult GetProjectHistory(int projectId)
        {
            return Ok(TimeReports.CreateProjectHistoryReport(projectId));
        }
        [Route("api/reports/TeamReport")]
        public IHttpActionResult GetTeamReport(string teamId,int year, int month)
        {
            if (TimeUnit.Teams.Get(teamId) == null) return NotFound();
            return Ok(TimeUnit.GetTeamReport(teamId, year, month, TimeFactory));
        }
        [Route("api/reports/CompanyReport")]
        public IHttpActionResult GetCompanyReport(int year, int month)
        {
            //if (TimeUnit.Teams.Get(teamId) == null) return NotFound();
            return Ok(TimeUnit.GetCompanyReport( year, month, TimeFactory));
        }
        //[Route("api/reports/InvoiceReport")]
        //public IHttpActionResult GetInvoiceReport(int year, int month)
        //{
        //    //if (TimeUnit.Teams.Get(teamId) == null) return NotFound();
        //    return Ok(TimeUnit.GetInvoiceReport(year, month, TimeFactory));
        //}
        [Route("api/reports/InvoiceReport")]
        public IHttpActionResult GetInvoiceReport(int year, int month)
        {
            return Ok(TimeUnit.GetFromMongo(year, month));
        }

    }
}

