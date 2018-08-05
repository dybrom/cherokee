using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cherokee.API.Models
{
    public class OneDayModel
    {
        public OneDayModel()
        {
            Tasks = new List<DetailModel>();
        }
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Ordinal { get; set; }
        public string Type { get; set; }
        public decimal Hours { get; set; }
        public virtual ICollection<DetailModel> Tasks { get; set; }
    }
}