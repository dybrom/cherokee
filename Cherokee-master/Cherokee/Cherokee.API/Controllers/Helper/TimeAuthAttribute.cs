using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Cherokee.API.Models;
using Cherokee.DAL;
using Cherokee.DAL.Entities;
using Cherokee.DAL.Repositories;

namespace Cherokee.API.Controllers.Helper
{
    public class TimeAuthAttribute : AuthorizationFilterAttribute
    {
        private string Roles;
        public TimeAuthAttribute(string _roles = "")
        {
            Roles = _roles == "" ? "Administrator,Leader,User" : _roles;
        }
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            UnitOfWork timeUnit = new UnitOfWork();
            ModelFactory factory = new ModelFactory();
            UserModel CurrentUser = new UserModel();
            Dictionary<string, string> token = new Dictionary<string, string>();
            string provider = "iserver";

            try
            {
                string id_token = actionContext.Request.Headers.Authorization.Parameter;
                if (HttpContext.Current.Request.Headers["Provider"] != null)
                    provider = actionContext.Request.Headers.GetValues("Provider").FirstOrDefault();
                token = TokenUtility.GenToken(id_token);
                Employee emp = new Employee();
                string field = provider == "iserver" ? "sub" : "email";
                emp = timeUnit.Employees.Get(x => x.Email == token[field]).FirstOrDefault();
                if (emp != null) CurrentUser = factory.Create(emp, provider);
                if (Roles.Contains(CurrentUser.Role)) return;
                actionContext.Response = actionContext.Request
                    .CreateResponse(HttpStatusCode.Unauthorized);
            }
            catch
            {
                actionContext.Response = actionContext.Request
                    .CreateResponse(HttpStatusCode.Unauthorized);
            }
        }
    }
}