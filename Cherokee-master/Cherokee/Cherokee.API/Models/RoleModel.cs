using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cherokee.API.Models
{
    public class FullRoleModel: RoleModel
    {
        public string Type { get; set; }
        public decimal? HourlyRate { get; set; }
        public decimal? MonthlyRate { get; set; }
        public ICollection<MemberModel> Members { get; set; }
    }
    public class RoleModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}