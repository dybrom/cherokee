using Cherokee.DAL.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cherokee.API.Controllers.Helper
{
    public static class Validation
    {
        public static List<string> Validate(this Assignment assignment)
        {
            List<string> errorMsgs = new List<string>();
            if (assignment.Description.Length < 4)
                errorMsgs.Add("Assignment description must contain at least 4 characters!\n");

            return errorMsgs;
        }
        public static List<string> Validate(this Category category)
        {
            List<string> errorMsgs = new List<string>();
            if (category.Description.Length < 4)
                errorMsgs.Add("Category description must contain at least 4 characters!\n");

            return errorMsgs;
        }
        public static List<string> Validate(this Customer customer)
        {
            List<string> errorMsgs = new List<string>();
            if (customer.Name.Length < 4)
                errorMsgs.Add("Customer name must contain at least 4 characters!\n");

            return errorMsgs;
        }
        public static List<string> Validate(this Day day)
        {
            List<string> errorMsgs = new List<string>();
            if (day.Hours < 1)
                errorMsgs.Add("Hours for day is required and it must be >= 1");

            return errorMsgs;
        }
        public static List<string> Validate(this Employee employee)
        {
            List<string> errorMsgs = new List<string>();
            if (employee.FirstName.Length < 2)
                errorMsgs.Add("Employee first name must contain at least 2 characters!");
            if (employee.LastName.Length < 2)
                errorMsgs.Add("Employee first name must contain at least 2 characters!");
            return errorMsgs;
        }
        public static List<string> Validate(this Engagement engagement)
        {
            List<string> errorMsgs = new List<string>();
            if (engagement.Hours< 2)
                errorMsgs.Add("Hours for engagements is required and it must be >= 1");
            return errorMsgs;
        }
        public static List<string> Validate(this Project project)
        {
            List<string> errorMsgs = new List<string>();
            if (project.Name.Length < 3)
                errorMsgs.Add("Name for project is required and it must be at least 2 characters");
            return errorMsgs;
        }
        //public static List<string> Validate(this Project project)
        //{
        //    List<string> errorMsgs = new List<string>();
        //    if (project.Name.Length < 3)
        //        errorMsgs.Add("Name for project is required and it must be at least 2 characters");
        //    return errorMsgs;
        //}

    }
}