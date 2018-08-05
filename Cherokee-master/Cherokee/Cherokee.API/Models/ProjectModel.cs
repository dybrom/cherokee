using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cherokee.API.Models
{
    public class FullProjectModel:ProjectModel
    {
        public string Monogram { get; set; }
        public string Description { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
        public string Pricing { get; set; }
        public decimal? Amount { get; set; }
        public BaseModel Customer { get; set; }
        public TeamModel Team { get; set; }
    }
    public class ProjectModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    
}