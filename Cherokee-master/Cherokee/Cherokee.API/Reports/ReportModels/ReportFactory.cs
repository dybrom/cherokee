using Cherokee.API.Models;
using Cherokee.DAL.Entities;
using Cherokee.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cherokee.API.Reports.Models
{
    public class ReportFactory
    {
        private UnitOfWork TimeUnit;



        public ReportFactory(UnitOfWork _unit)
        {
            TimeUnit = _unit;
        }

        //public RoleModel Create(Role r)
        //{
           
        // return new RoleModel()
        //    {
        //        Id = r.Id,
        //        Name = r.Name
        //    };
        //}

        public List<AnnualModel> AnnualReport(int year)
        {
            var query = TimeUnit.Projects.Get().OrderBy(x => x.Name)
                                               .Select(x => new
                                               {
                                                   project = x.Name,
                                                   details = x.Assignments.Where(d => d.Day.Date.Value.Year == year)
                                                                      .GroupBy(d => d.Day.Date.Value.Month)
                                                                      .Select(w => new { month = w.Key, hours = w.Sum(d => d.Hours) })
                                                                      .ToList()
                                               }).ToList();

            List<AnnualModel> list = new List<AnnualModel>();
            AnnualModel total = new AnnualModel { ProjectName = "T O T A L" };
            total.TotalHours = 0;

            foreach (var q in query)
            {
                   
                AnnualModel item = new AnnualModel() { ProjectName = q.project };
                item.TotalHours = 0;
                foreach (var w in q.details)
                {
                    item.TotalHours += w.hours.Value;
                    total.TotalHours += w.hours.Value;
                    item.MonthlyHours[w.month - 1] = w.hours.Value;
                    total.MonthlyHours[w.month - 1] += w.hours.Value;
                }
                if (item.TotalHours > 0) list.Add(item);
                item.NumOfEmployees = TimeUnit.Projects.Get().Where(p => p.Name == q.project).Select(t => t.Team).SelectMany(e => e.Engagements).Select(e=>e.Employee).Where(e=>e.BeginDate.Value.Year<=year).Count();
            }
            list.Add(total);
            return list;
        }

        public MonthlyModel MonthlyReport(int year, int month)
        {
            MonthlyModel result = new MonthlyModel() { Year = year, Month = month };
            result.Projects = TimeUnit.Projects.Get().OrderBy(x => x.Monogram).Select(x => new ProjectItem
            {
                Id= x.Id,
                Project = x.Name,
                Monogram = x.Monogram,
                Hours = x.Assignments.Where(d => d.Day.Date.Value.Year == year && d.Day.Date.Value.Month == month).Sum(d => d.Hours)
            }).ToList();

            result.Projects.RemoveAll(q => q.Hours == null);
            result.Total = result.Projects.Sum(x => x.Hours.Value);
            int listSize = result.Projects.Count();

            var query = TimeUnit.Employees.Get().OrderBy(e => e.FirstName).ThenBy(e => e.LastName).ToList()
                                .Select(x => new {
                                    employee = x.FirstName + " " + x.LastName,
                                    details = x.Days.Where(c => c.Date.Value.Year == year && c.Date.Value.Month == month)
                                                        .SelectMany(c => c.Assignments)
                                                        .OrderBy(p => p.Project.Monogram)
                                                        .GroupBy(p => p.Project.Monogram)
                                                        .Select(d => new { project = d.Key, hours = d.Sum(w => w.Hours) }).ToList()
                                }).ToList();
            foreach (var q in query)
            {
                MonthlyItem item = new MonthlyItem(listSize) { Employee = q.employee, Total = q.details.Sum(w => w.hours.Value) };
                foreach (var d in q.details)
                {
                    int i = result.Projects.FindIndex(x => x.Monogram == d.project);
                    item.Hours[i] = d.hours.Value;
                }
                if (item.Total != 0) result.Items.Add(item);
            }

            return result;
        }

        public ProjectHistoryModel CreateProjectHistoryReport(int id)
        {
            decimal? monthlyRate = TimeUnit.Projects.Get(id).Amount;
            

            var project = TimeUnit.Projects.Get(id);
            List<int> years = new List<int>
            {
                2016,
                2017,
                2018
            };
            List<ProjectHistoryEmployee> ReportEmployees = new List<ProjectHistoryEmployee>();
            List<ProjectHistoryTotal> Total = new List<ProjectHistoryTotal>();
            var employees = TimeUnit.Projects.Get().Where(x => x.Id == id).Select(y => y.Team).SelectMany(z => z.Engagements).Select(y => y.Employee).Where(d=>d.Days.Count>0).ToList();
            foreach(var employee in employees)
            {
                decimal? employeeTotalHours = 0;
                ProjectHistoryEmployee empToAdd = new ProjectHistoryEmployee()
                {
                    Id = employee.Id,
                    Name = employee.FullName
                };
          
                

                var totalHours = TimeUnit.Days.Get().Where(x => x.Employee.Id == employee.Id).SelectMany(y => y.Assignments).Where(x => x.Project.Id == id).Select(x => x.Hours).DefaultIfEmpty(0).Sum();
                foreach(var year in years)
                {
                    
                    ProjectHistoryTotal empvar = new ProjectHistoryTotal()
                    {
                        Year = year,
                        TotalHours = TimeUnit.Days.Get().Where(x => x.Employee.Id == employee.Id && x.Date.Value.Year == year).SelectMany(y => y.Assignments).Where(x => x.Project.Id == id).Select(x => x.Hours).DefaultIfEmpty(0).Sum()
                    };
                    for(int i = 0; i < 12; i++)
                    {
                        decimal? monthlyHours = 0;
                        monthlyHours = TimeUnit.Days.Get().Where(x => x.Employee.Id == employee.Id && x.Date.Value.Month == i + 1 && x.Date.Value.Year == year).SelectMany(y => y.Assignments).Where(x => x.Project.Id == id).Select(x => x.Hours).DefaultIfEmpty(0).Sum();
                        empvar.MonthlyHours[i].Hours = monthlyHours;
                            

                    }
                    empToAdd.Sums.Add(empvar);

                    employeeTotalHours += empvar.TotalHours;

                }
                empToAdd.TotalHours = employeeTotalHours;
                if(empToAdd.TotalHours>0)
                ReportEmployees.Add(empToAdd);
                
            }
            
            

            var projectTotalHours = TimeUnit.Days.Get().SelectMany(y => y.Assignments).Where(x => x.Project.Id == id).Select(x => x.Hours).DefaultIfEmpty(0).Sum();
            
            foreach(var year in years)
            {
                ProjectHistoryTotal YearTotal = new ProjectHistoryTotal()
                {
                    Year = year,
                    TotalHours = TimeUnit.Days.Get().Where(x => x.Date.Value.Year == year).SelectMany(y => y.Assignments).Where(x => x.Project.Id == id).Select(x => x.Hours).DefaultIfEmpty(0).Sum()
                };
                for(int i = 0; i < 12; i++)
                {
                    decimal? monthlyHours = 0;
                    monthlyHours = TimeUnit.Days.Get().Where(x => x.Date.Value.Year == year && x.Date.Value.Month==i+1).SelectMany(y => y.Assignments).Where(x => x.Project.Id == id).Select(x => x.Hours).DefaultIfEmpty(0).Sum();
                    YearTotal.MonthlyHours[i].Hours = monthlyHours;
                }
                Total.Add(YearTotal);
            }


            return new ProjectHistoryModel() {
                Id = id,
                Name = project.Name,
                TotalHours = projectTotalHours,
                Employees = ReportEmployees,
                Amount = project.Amount,
                BeginDate = project.BeginDate,
                EndDate = project.EndDate,
                Total = Total

            };
            /*var query = project.Assignments.GroupBy(x => new { year = x.Day.Date.Value.Year, empl = x.Day.Employee }).
                Select(x => new ProjectHistoryEmployee()
                {
                    Id = x.Key.empl.Id,
                    Name = x.Key.empl.FullName,
                   
                    //Employee = new BaseModel { Id = x.Key.empl.Id, Name = x.Key.empl.FullName}
                }).ToList();*/

            //var report = TimeUnit.Projects.Get()
            //                          .Where(x => x.Id == id)
            //                          .Select(x => new ProjectHistoryModel()
            //                          {
            //                              ProjectName = x.Name,
            //                              Amount = x.Amount.Value,
            //                              BeginDate = x.BeginDate.Value,
            //                              EndDate = x.EndDate,
            //                              Hours = x.Assignments.GroupBy(m => m.Day.Date.Value.Year)
            //                                             .Select(d => d.Sum(h => h.Hours.Value)).DefaultIfEmpty(0)
            //                                             .ToList(),
            //                          }).FirstOrDefault();
            // report.Employees = query;
            //return report;

        }

    }
   
}