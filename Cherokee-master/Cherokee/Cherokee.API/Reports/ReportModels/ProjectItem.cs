using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cherokee.API.Reports.Models
{
    public class ProjectItem
    {
        public int Id { get; set; }
        public string Monogram { get; set; }
        public string Project { get; set; }
        public decimal? Hours { get; set; }
    }
}