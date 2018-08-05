using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cherokee.API.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public List<UserModelTeam> Teams { get; set; }
        public string Provider { get; set; }
        public int ProfileId { get; set; }
    }
}