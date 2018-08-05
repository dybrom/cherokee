using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cherokee.API.Models
{
    public class FullTeamModel:TeamModel
    {

        public string Image { get; set; }

        public ICollection<MemberModel> Members { get; set; }
        public ICollection<ProjectModel> Projects { get; set; }
    }
    public class TeamModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

    }
}