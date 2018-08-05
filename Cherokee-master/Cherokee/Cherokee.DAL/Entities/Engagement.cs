using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cherokee.DAL.Entities
{
    public class Engagement : BaseClass<int>
    {
        [Required(ErrorMessage = "Hours for engagement is required!" )]
        [Range(1, 3000)]
        public decimal? Hours { get; set; }

        [Index("empTeamRole", 1, IsUnique = true)]
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        [Index("empTeamRole", 2, IsUnique = true)]
        [ForeignKey("Team")]
        public string TeamId { get; set; }
        public virtual  Team Team { get; set; }

        [Index("empTeamRole", 3, IsUnique = true)]
        [ForeignKey("Role")]
        public string RoleId { get; set; }
        public virtual Role Role { get; set; }        
    }
}
