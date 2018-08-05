using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cherokee.DAL.Entities
{
    public enum TeamStatus
    {
        Waiting = 0,
        Active = 1,
        [Display(Name = "On hold")]
        OnHold = 2
    }

    public class Team : BaseClass<string>
    {
        public Team()
        {
            Engagements = new List<Engagement>();
            Projects = new List<Project>();
        }
        [Required(ErrorMessage = "Team name is required!")]
        [StringLength(int.MaxValue),MinLength(4)]
        public string Name { get; set; }

        [StringLength(int.MaxValue)]
        public string Image { get; set; }

        [StringLength(int.MaxValue)]
        public string Description { get; set; }

        public TeamStatus? Status { get; set; }

        public virtual ICollection<Engagement> Engagements { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
    }
}
