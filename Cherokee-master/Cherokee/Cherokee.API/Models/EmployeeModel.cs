using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cherokee.API.Models
{
    public class FullEmployeeModel: EmployeeModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Image { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Status { get; set; }
        public decimal? Salary { get; set; }
        public virtual ICollection<MemberModel> Teams { get; set; }
        public virtual ICollection<BaseModel> Projects { get; set; }
    }
    public class EmployeeModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public RoleModel Position { get; set; }
    }
}