using Cherokee.API.Controllers.Helper;
using Cherokee.API.Reports;
using Cherokee.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cherokee.API.Models
{
    public class ModelFactory
    {
        public TeamModel Create(Team t, bool flag = false)
        {
            if (!flag)
                return new FullTeamModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                    Image = t.Image,
                    Members = t.Engagements.Where(e=> e.Employee != null).Select(e => Create(e)).ToList(),
                    Projects = t.Projects.Select(p => Create(p, true)).ToList()
                };
            else
                return new TeamModel()
                {
                    Id = t.Id,
                    Name = t.Name
                };
        }

        public MemberModel Create(Engagement e)
        {
          
           return new MemberModel()
            {
                Id = e.Id,
                Team = new TeamModel { Id = e.Team.Id, Name = e.Team.Name },
                Employee = new BaseModel { Id = e.Employee.Id, Name = e.Employee.FullName },
                Role = new RoleModel { Id = e.Role.Id, Name = e.Role.Name },
                Hours = e.Hours
            };
        }

        public ProjectModel Create(Project p, bool flag = false)
        {
            if(!flag)
            return new FullProjectModel()
            {
                Id = p.Id,
                Name = p.Name,
                Monogram = p.Monogram,
                Description = p.Description,
                BeginDate = p.BeginDate,
                EndDate = p.EndDate,
                Status = p.Status.ToString(),
                Pricing = p.Pricing.ToString(),
                Amount = p.Amount,
                Customer = new BaseModel { Id = p.Customer.Id, Name = p.Customer.Name},
                Team = new TeamModel { Id = p.Team.Id, Name = p.Team.Name},
             
            };
            else return new ProjectModel(){
                Id = p.Id,
                Name = p.Name
                
                
            };
        }
       

        public EmployeeModel Create(Employee emp, bool flag=false)
        {
            if (!flag)
                return new FullEmployeeModel()
                {
                    Id = emp.Id,
                    FirstName = emp.FirstName,
                    LastName = emp.LastName,
                    Name = emp.FullName,
                    //Image = emp.Image,
                    Image = "data:image/png;base64, " + emp.ConvertToBase64(),
                    Email = emp.Email,
                    Phone = emp.Phone,
                    BirthDate = emp.BirthDate,
                    BeginDate = emp.BeginDate,
                    EndDate = emp.EndDate,
                    Status = Convert.ToInt32(emp.Status),
                    Salary = emp.Salary,
                    Position = Create(emp.Position, true),
                    //Days = emp.Days.Select(d => Create(d)).ToList(),
                    Teams = emp.Engagements.Select(e => Create(e)).ToList(),
                    Projects = emp.Engagements.Select(x => x.Team).SelectMany(y => y.Projects).Select(z=>new BaseModel()
                    {
                        Id= z.Id,
                        Name = z.Name
                    }).ToList()
                     
                };
            else
                return new EmployeeModel()
                {
                    Id = emp.Id,
                    FirstName = emp.FirstName,
                    LastName = emp.LastName,
                    Position = Create(emp.Position,true)
                };
        }

        public RoleModel Create(Role r, bool flag=false)
        {
            if (!flag)
                return new FullRoleModel()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Type = r.Type.ToString(),
                    HourlyRate = r.HourlyRate,
                    MonthlyRate = r.MonthlyRate,
                    Members = r.Engagements.Select(a => Create(a)).ToList()
                };
            else return new RoleModel()
            {
                Id=r.Id,
                Name = r.Name
            };
        }
     
        //public AssignmentModel Create(Assignment a)
        //{
        //    return new AssignmentModel()
        //    {
        //        Description = a.Description,
        //        Hours = a.Hours,
        //        Day = a.Day.Date.ToString(),
        //        Project = a.Project.Name
        //    };
        //}
        //public ShortAssignmentModel Create1(Assignment sa)
        //{
        //    return new ShortAssignmentModel()
        //    {
        //        Description = sa.Description
        //    };
        //}
        
        //public AssignmentModel Create(Assignment a)
        //{

        //     return new AssignmentModel()
        //    {
        //        Id = a.Id,
        //        Description = a.Description,
        //             Hours = a.Hours,
        //             Day = a.Day.Date.ToString(),
        //             Project = a.Project.Name,
        //             DayId = a.Day.Id,
        //             ProjectId = a.Project.Id
        //         };
        //}
        public DetailModel Create(Assignment a)
        {
            return new DetailModel()
            {
                Id = a.Id,
                Description = a.Description,
                Hours = a.Hours.Value,
                Deleted = a.Deleted,
                Project = new BaseModel { Id = a.Project.Id, Name = a.Project.Name }
            };
        }
        //public DayModel Create(Day d)
        //{
        //    return new DayModel()
        //    {
        //        Employee = d.Employee.FirstName + ' ' + d.Employee.LastName,
        //        EmployeeId = d.Employee.Id,
        //        Month = d.Date.Value.Month,
        //        Year = d.Date.Value.Year
        //    };
        //}

        public CustomerModel Create(Customer c)
        {
            return new CustomerModel()
            {
                Id = c.Id,
                Name = c.Name,
                Monogram = c.Monogram,
                Image = c.Image,
                Contact = c.Contact,
                Email = c.Email,
                Phone = c.Phone,
                Status =Convert.ToInt32(c.Status),
                Projects = c.Projects.Select(p => Create(p,true)).ToList()

            };
        }

        public CategoryModel Create(Category c)
        {
            return new CategoryModel()
            {
                Id = c.Id,
                Description = c.Description,

            };
        }
        public UserModel Create(Employee emp, string provider)
        {
            return new UserModel()
            {
                Id = emp.Id,
                Name = emp.FullName,
                Role = emp.Position.Name,
                Teams = emp.Engagements.Where(x=>x.Role.Id.Contains("TL")).Select(x => x.Team).Select(x=>Create(x,emp)).ToList(),
                Provider = provider,
                ProfileId = emp.Id
            };
        }
        public UserModelTeam Create(Team t,Employee emp)
        {
            return new UserModelTeam()
            {
                Id = t.Id,
                Name = t.Name,
                Role = emp.Engagements.Where(x=>x.Team.Id==t.Id).Select(x=>x.Role).Select(x=>Create(x,true)).FirstOrDefault()
            };
        }

    }
}