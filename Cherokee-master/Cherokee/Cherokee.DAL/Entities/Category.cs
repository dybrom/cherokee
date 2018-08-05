using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cherokee.DAL.Entities
{
    public class Category : BaseClass<int>
    {
        public Category()
        {
            Days = new List<Day>();
        }
        [Required(ErrorMessage ="Description is required!")]
        [StringLength(int.MaxValue), MinLength(4)]
        public string Description { get; set; }

        public virtual ICollection<Day> Days { get; set; }
    }
}
