using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cherokee.DAL.Entities
{
    public enum EmployeeStatus
    {
        Active = 0,
        Trial = 1,
        Leaver = 2
    }

    public enum EmployeeJobTitle
    {
        [Display(Name = "General Manager")]
        GeneralManager = 0,
        [Display(Name = "Project Manager")]
        ProjectManager = 1,
        [Display(Name = "QA Engineer")]
        QAEngineer = 2,
        [Display(Name = "UX Designer")]
        UXDesigner = 3,
        [Display(Name = "Junior Developer")]
        JuniorDeveloper = 4,
        [Display(Name = "Senior Developer")]
        SeniorDeveloper = 5
    }

    public class Employee : BaseClass<int>
    {
        public Employee()
        {
            Days = new List<Day>();
            Engagements = new List<Engagement>();        
        }

        [Required(ErrorMessage = "First name is required!")]
        [StringLength(int.MaxValue), MinLength(2)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required!")]
        [StringLength(int.MaxValue),MinLength(2)]
        public string LastName { get; set; }

        [StringLength(int.MaxValue)]
        public string Image { get; set; }
        
        [StringLength(int.MaxValue)]
        public string Email { get; set; }
        
        [StringLength(int.MaxValue)]
        public string Phone { get; set; }
        
        public DateTime? BirthDate { get; set; }
        
        public EmployeeStatus? Status { get; set; }
        
        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        //[RegularExpression(@"^\d{0,9}(\.\d{2,2})?$")]
        public decimal? Salary { get; set; }
        public string Password { get; set; }

        [ForeignKey("Position")]
        [Column("Position")]
        [StringLength(int.MaxValue)]
        public string PositionId { get; set; }
        public  virtual Role Position { get; set; }        

        public virtual ICollection<Day> Days { get; set; }
        public virtual ICollection<Engagement> Engagements { get; set; }
                    
        [NotMapped]
        public string FullName
        {
            get
            {
                return FirstName + ' ' + LastName;
            }
        }
    }
}
