using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cherokee.DAL.Entities
{
    public enum ProjectStatus
    {
        [Display(Name = "In progress")]
        InProgress = 0,
        [Display(Name = "On hold")]
        OnHold = 1,
        Finished = 2,
        Canceled = 3
    }

    public enum ProjectPricing
    {
        [Display(Name = "Hourly Rate")]
        HourlyRate = 0,
        [Display(Name = "Per Capita Rate")]
        PerCapitaRate = 1,
        [Display(Name = "Fixed Price")]
        FixedPrice = 2,
        [Display(Name = "Not Billable")]
        NotBillable = 3
    }
   
    public class Project : BaseClass<int>
    {
        public Project()
        {
            Assignments = new List<Assignment>();
         
        }
        [Required(ErrorMessage = "Project name is required!")]
        [StringLength(int.MaxValue),MinLength(4)]
        public string Name { get; set; }

        [StringLength(int.MaxValue)]
        public string Monogram { get; set; }

        [StringLength(int.MaxValue)]
        public string Description { get; set; }

        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        public ProjectStatus? Status { get; set; }

        public ProjectPricing? Pricing { get; set; }

        [RegularExpression(@"^\d{0,9}(\.\d{2,2})?$")]
        public decimal? Amount { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        [ForeignKey("Team")]
        public string TeamId { get; set; }
        public virtual Team Team { get; set; }

        public virtual ICollection<Assignment> Assignments { get; set; }
    }
}
