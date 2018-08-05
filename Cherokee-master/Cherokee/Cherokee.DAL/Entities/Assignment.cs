using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cherokee.DAL.Entities
{
    public class Assignment : BaseClass<int>
    {
       
        public string Description { get; set; }

        
        
        public decimal? Hours { get; set; }

        [ForeignKey("Day")]
        public int DayId { get; set; }        
        public virtual Day Day { get; set; }

        [ForeignKey("Project")]
        public int ProjectId { get; set; }        
        public virtual Project Project { get; set; }
    }
}
