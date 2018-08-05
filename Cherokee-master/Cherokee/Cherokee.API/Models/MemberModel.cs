using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cherokee.API.Models
{
  
    public class MemberModel
    {

        public int Id { get; set; }
        public BaseModel Employee { get; set; }
        public RoleModel Role { get; set; }
        public TeamModel Team { get; set; }
        public decimal? Hours { get; set; }

    }


}
