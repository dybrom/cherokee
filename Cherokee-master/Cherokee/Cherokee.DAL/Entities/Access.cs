using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cherokee.DAL.Entities
{
    public class Access : BaseClass<int>
    {
        [StringLength(int.MaxValue)]
        public string Entity { get; set; }

        [StringLength(int.MaxValue)]
        public string Right { get; set; }
        
        [StringLength(int.MaxValue)]
        public string TeamFlag { get; set; }

        [StringLength(int.MaxValue)]
        public string PersonFlag { get; set; }

        [ForeignKey("Role")]
        public string RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
