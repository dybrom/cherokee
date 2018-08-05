using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cherokee.DAL.Entities
{
    public enum CustomerStatus
    {
        Prospect = 0,
        Client = 1
    }

    public class Customer : BaseClass<int>
    {
        public Customer()
        {
            Address = new Address();
            Projects = new List<Project>();
        }
        [Required(ErrorMessage = "Customer name is required!"), MinLength(4)]
        [StringLength(int.MaxValue)]
        public string Name { get; set; }

        [StringLength(int.MaxValue)]
        public string Image { get; set; }
        
        [StringLength(int.MaxValue)]
        public string Monogram { get; set; }
        
        [StringLength(int.MaxValue)]
        public string Contact { get; set; }
        
        [StringLength(int.MaxValue)]
        public string Email { get; set; }
        
        [StringLength(int.MaxValue)]
        public string Phone { get; set; }
        
        public CustomerStatus? Status  { get; set; }
        
        public Address Address { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
    }
}
