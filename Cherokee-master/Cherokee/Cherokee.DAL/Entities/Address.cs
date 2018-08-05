using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cherokee.DAL.Entities
{
    public class Address
    {
        [StringLength(int.MaxValue)]
        public string Street { get; set; }

        [StringLength(int.MaxValue)]
        public string ZipCode { get; set; }
        
        [StringLength(int.MaxValue)]
        public string City { get; set; }
    }
}
