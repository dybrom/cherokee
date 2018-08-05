using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cherokee.DAL.Entities
{
   public enum DayType
    {
        WorkingDay = 1,
        PublicHoliday,
        OtherAbsence,
        ReligiousDay,
        SickLeave,
        Vacation,
        BusinessAbsence
    }
    public class Day : BaseClass<int>
    {
    

        public Day()
        {
            Assignments = new List<Assignment>();
        }

        public DateTime? Date { get; set; }

        [Required(ErrorMessage = "Hours for day is required!")]
        [Range(1, 3000)]
        public decimal? Hours { get; set; }
        
        [StringLength(int.MaxValue)]
        public string Comment { get; set; }



        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }   
        public virtual Employee Employee { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public  virtual ICollection<Assignment> Assignments { get; set; }
    }
}
