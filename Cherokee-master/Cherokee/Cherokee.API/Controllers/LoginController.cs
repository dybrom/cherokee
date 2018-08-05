using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using Cherokee.API.Models;
using Cherokee.DAL;
using Cherokee.DAL.Entities;
using Cherokee.API.Controllers.Helper;

namespace Cherokee.API.Controllers
{
    [RoutePrefix("api/login")]
    public class LoginController : BaseController
    {
        [Route("")]
        public IHttpActionResult Get()
        {
            UserModel CurrentUser = BuildUser("iserver", "sub");
            return Ok(CurrentUser);
        }
        [Route("")]
        public IHttpActionResult Post()
        {
            UserModel CurrentUser = BuildUser("google", "email");
            return Ok(CurrentUser);
        }

        UserModel BuildUser(string provider, string field)
        {
            Dictionary<string, string> token = new Dictionary<string, string>();
            UserModel CurrentUser = new UserModel();
            if (HttpContext.Current.Request.Headers["Authorization"] != null)
            {
                string id_token = HttpContext.Current.Request.Headers.GetValues("Authorization").FirstOrDefault().Substring(7);
                token = TokenUtility.GenToken(id_token);
                DateTime expTime = new DateTime(1970, 1, 1)
                    .AddSeconds(Convert.ToDouble(token["exp"]));
                Employee emp = TimeUnit.Employees.Get(x => x.Email == token[field]).FirstOrDefault();
                CurrentUser = TimeFactory.Create(emp, provider);
            }
            return CurrentUser;
        }
    }
}