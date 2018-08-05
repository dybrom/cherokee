using Cherokee.API.Models;
using Cherokee.API.Reports.Models;
using Cherokee.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cherokee.API.Controllers
{
    public class BaseController : ApiController
    {
        UnitOfWork unit;
        ModelFactory factory;
        ReportFactory reports;

        public ReportFactory TimeReports { get { return reports ?? (reports = new ReportFactory(TimeUnit)); } }

        public UnitOfWork TimeUnit { get
            {
                if (unit == null) unit = new UnitOfWork();
                return unit;
            } }
        public ModelFactory TimeFactory { get
            {
                if (factory == null) factory = new ModelFactory();
                return factory;
            } }
    }
}
