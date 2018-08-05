using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cherokee.DAL.Entities
{
    public enum RoleType
    {
        JobTitle = 0,
        TeamRole = 1,
        AppRole = 2,
    }

    public class Role : BaseClass<string>
    {
        public Role()
        {
            Positions = new List<Employee>();
            Engagements = new List<Engagement>();
            Accesses = new List<Access>();
        }
        [Required(ErrorMessage = "Role name is required!")]
        [StringLength(int.MaxValue), MinLength(4)]
        public string Name { get; set; }

        //[RegularExpression(@"^\d{0,9}(\.\d{2,2})?$")]
        public decimal? HourlyRate { get; set; }

        //[RegularExpression(@"^\d{0,9}(\.\d{2,2})?$")]
        public decimal? MonthlyRate { get; set; }

        public RoleType? Type { get; set; }

        public virtual ICollection<Employee> Positions { get; set; }    
        public virtual ICollection<Engagement> Engagements { get; set; }
        public virtual ICollection<Access> Accesses { get; set; }
    }
}
