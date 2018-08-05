using Cherokee.API.Controllers.Helper.Reports;
using Cherokee.API.Models;
using Cherokee.API.Reports.ReportModels;
using Cherokee.DAL.Repositories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cherokee.API.Controllers.Helper
{
    // personal report model
    // personal report day model
    // personal report task model 
    public static class ReportsExtensions
    {
       public static PersonalReport GetPersonalReport(this UnitOfWork unit, int employeeId, int year, int month,ModelFactory factory)
        {
            var emp = (FullEmployeeModel)unit.Employees.Get().Where(x => x.Id == employeeId).ToList()
                                                             .Select(x => factory.Create(x)).FirstOrDefault();
            if (emp == null) return null;
            decimal? overtime = unit.Engagements.Get().Select(x => x.Employee).SelectMany(y => y.Days).Where(x => x.Employee.Id == emp.Id
                                                       && x.Date.Value.Year == year
                                                       && x.Date.Value.Month == month && x.Hours > 8)
                                                       .Select(x => x.Hours).DefaultIfEmpty(0).Sum();
            int overtimeCount = unit.Engagements.Get().Select(x => x.Employee).SelectMany(y => y.Days)
            .Where(x => x.Employee.Id == emp.Id
                                            && x.Date.Value.Year == year
                                            && x.Date.Value.Month == month && x.Hours > 8).Select(x => x.Hours).Count();
            overtime -= overtimeCount * 8;
            var days = unit.Days.Get().Where(x => x.EmployeeId == employeeId && x.Date.Value.Year == year && x.Date.Value.Month == month)
                                .Select(x => new PersonalReportDay()
                                {
                                    Id = x.Id,
                                    Date = x.Date,
                                    Type = x.Category.Description,
                                    Comment = x.Comment,
                                    Hours = x.Hours,
                                    OvertimeHours = overtime,
                                    Ordinal = x.Date.Value.Day,
                                    
                                    Tasks = x.Assignments.Select(y => new PersonalReportTask()
                                    {
                                        Hours = y.Hours,
                                        Description = y.Description,
                                        Project = y.Project.Name
                                    }).ToList()

                                }).OrderBy(y => y.Date).ToList();

            int workingDays = unit.Days.Get().Where(x => x.Employee.Id == employeeId
                                                    && x.Date.Value.Year == year
                                                    && x.Date.Value.Month == month
                                                    && x.Category.Description == "Working day").Count();

            int vacationDays = unit.Days.Get().Where(x => x.Employee.Id == employeeId
                                                     && x.Date.Value.Year == year
                                                     && x.Date.Value.Month == month
                                                     && x.Category.Description == "Vacation").Count();

            
            int businessAbscenceDays = unit.Days.Get().Where(x => x.Employee.Id == employeeId
                                                     && x.Date.Value.Year == year
                                                     && x.Date.Value.Month == month
                                                     && x.Category.Description == "Business Absence").Count();
            int publicHolidays = unit.Days.Get().Where(x => x.Employee.Id == employeeId
                                                     && x.Date.Value.Year == year
                                                     && x.Date.Value.Month == month
                                                     && x.Category.Description == "Public holiday").Count();
            int sickLeaveDays = unit.Days.Get().Where(x => x.Employee.Id == employeeId
                                                     && x.Date.Value.Year == year
                                                     && x.Date.Value.Month == month
                                                     && x.Category.Description == "Sick leave").Count();

            int religiousDays = unit.Days.Get().Where(x => x.Employee.Id == employeeId
                                                     && x.Date.Value.Year == year
                                                     && x.Date.Value.Month == month
                                                     && x.Category.Description == "Religious day").Count();
            int otherAbsenceDays = unit.Days.Get().Where(x => x.Employee.Id == employeeId
                                                     && x.Date.Value.Year == year
                                                     && x.Date.Value.Month == month
                                                     && x.Category.Description == "Other absence").Count();
            decimal? totalhoursss = unit.Days.Get().Where(x => x.Employee.Id == employeeId
                                                    && x.Date.Value.Year == year
                                                    && x.Date.Value.Month == month && x.Category.Description == "Working day").Select(x => x.Hours).DefaultIfEmpty(0).Sum();
            List<int> listDates = null;
            var date = new DateTime(year, month, 1);

            if(emp.BeginDate.Value.Year == date.Year && emp.BeginDate.Value.Month == date.Month 
                && emp.BeginDate.Value.Day < DateTime.DaysInMonth(year, month))
            {
                listDates = DateTimeHelper.ListOfWorkingDays(year, month, emp.BeginDate.Value.Day).ToList();
            }
            else
            {
                listDates = DateTimeHelper.ListOfWorkingDays(year, month).ToList();
            }
            List<PersonalReportProjectModel> projects = new List<PersonalReportProjectModel>();
            var allprojects = unit.Engagements.Get().Where(e => e.Employee.Id == employeeId).Select(t => t.Team).SelectMany(p => p.Projects).ToList();
            foreach(var project in allprojects)
            {
                decimal? hoursAtProject = unit.Days.Get()
                        .Where(d => d.Employee.Id == employeeId && d.Date.Value.Year == year && d.Date.Value.Month == month)
                        .SelectMany(a => a.Assignments).Where(a => a.Project.Id == project.Id).Select(h => h.Hours).DefaultIfEmpty(0).Sum();
                projects.Add(new PersonalReportProjectModel()
                {
                    Id = project.Id,
                    Name = project.Name,
                    Hours = hoursAtProject
                });
            }
            var report = new PersonalReport()
            {
                Employee = emp,
                WorkingDays = workingDays,
                VacationDays = vacationDays,
                BusinessAbscenceDays = businessAbscenceDays,
                PublicHolidays = publicHolidays,
                SickLeaveDays = sickLeaveDays,
                ReligiousDays = religiousDays,
                OtherAbscenceDays = otherAbsenceDays,
                PercentageOfWorkingDays = Math.Round(100 * (double)workingDays / listDates.Count(), 2),
                MissingEntries = (emp.BeginDate.Value.Date > date.Date) ? 0 : listDates.Except(days.Select(x => x.Date.Value.Day)).Count(),
                OvertimeHours = days.Sum(x => x.OvertimeHours),
                Days = days,
                TotalHours = totalhoursss.Value,
                Year = year,
                Month = month,
                TotalHoursInYear = unit.Days.Get().Where(x => x.Employee.Id == employeeId && x.Date.Value.Year == year && x.Category.Description == "Working day").Select(x => x.Hours).Sum(),
                TotalPossibleHours = listDates.Count()*8,
                Projects = projects
            };
            return report;
        }



        public static TeamReport GetTeamReport(this UnitOfWork unit, string teamId, int year, int month, ModelFactory factory)
        {
            var date = new DateTime(year, month, 1);
            var employees = unit.Engagements.Get().Where(x => x.Team.Id == teamId).Select(x => x.Employee).Where(x => x.BeginDate < date).ToList();
            List<TeamMemberModel> reports = new List<TeamMemberModel>();
            List<PersonalReportProjectModel> ProjectDetails = new List<PersonalReportProjectModel>();

            var projects = unit.Teams.Get().Where(x => x.Id == teamId).SelectMany(t => t.Projects).ToList();

            decimal? hours = 0;
            var numberOfEmployeesInTeam = unit.Engagements.Get().Where(x => x.Team.Id == teamId).Select(x => x.Employee).Where(x=>x.BeginDate<date).Count();
            
            DayStatisticModel statistika = new DayStatisticModel()
            {
                BusinessAbscenceDays = 0,
                MissingEntries = 0,
                OtherAbscenceDays = 0,
                OvertimeHours = 0,
                PercentageOfWorkingDays = 0,
                PublicHolidays = 0,
                ReligiousDays = 0,
                SickLeaveDays = 0,
                VacationDays = 0,
                WorkingDays = 0
            };
            decimal? overtime = 0;
            foreach (var employee in employees)
            {
                int PTO = 0;
                var days = unit.Engagements.Get().Where(x => x.Team.Id == teamId).Select(x => x.Employee).SelectMany(y => y.Days)
                    .Where(x => x.Employee.Id == employee.Id
                                                    && x.Date.Value.Year == year
                                                    && x.Date.Value.Month == month).ToList();
                List<int> listDates = null;

                if (employee.BeginDate.Value.Year == date.Year && employee.BeginDate.Value.Month == date.Month
                    && employee.BeginDate.Value.Day < DateTime.DaysInMonth(year, month))
                {
                    listDates = DateTimeHelper.ListOfWorkingDays(year, month, employee.BeginDate.Value.Day).ToList();
                }
                else
                {
                    listDates = DateTimeHelper.ListOfWorkingDays(year, month).ToList();
                }
                int workingDays = unit.Engagements.Get().Where(x=>x.Team.Id == teamId).Select(x=>x.Employee).SelectMany(y=>y.Days)
                    .Where(x => x.Employee.Id == employee.Id
                                                    && x.Date.Value.Year == year
                                                    && x.Date.Value.Month == month
                                                    && x.Category.Description == "Working day").Count();
                statistika.WorkingDays += workingDays;


                overtime = unit.Engagements.Get().Where(x => x.Team.Id == teamId).Select(x => x.Employee).SelectMany(y => y.Days).Where(x => x.Employee.Id == employee.Id
                                                       && x.Date.Value.Year == year
                                                       && x.Date.Value.Month == month && x.Hours > 8)
                                                       .Select(x => x.Hours).DefaultIfEmpty(0).Sum();

                int overtimeCount = unit.Engagements.Get().Where(x => x.Team.Id == teamId).Select(x => x.Employee).SelectMany(y => y.Days)
                .Where(x => x.Employee.Id == employee.Id
                                                && x.Date.Value.Year == year
                                                && x.Date.Value.Month == month && x.Hours > 8).Select(x => x.Hours).Count();
                overtime -= overtimeCount * 8;

                statistika.OvertimeHours += overtime;

                int vacationDays = unit.Engagements.Get().Where(x => x.Team.Id == teamId).Select(x => x.Employee).SelectMany(y => y.Days).Where(x => x.Employee.Id == employee.Id
                                                         && x.Date.Value.Year == year
                                                         && x.Date.Value.Month == month
                                                         && x.Category.Description == "Vacation").Count();
                statistika.VacationDays += vacationDays;
                PTO += vacationDays;

                int businessAbscenceDays = unit.Engagements.Get().Where(x => x.Team.Id == teamId).Select(x => x.Employee).SelectMany(y => y.Days).Where(x => x.Employee.Id == employee.Id
                                                         && x.Date.Value.Year == year
                                                         && x.Date.Value.Month == month
                                                         && x.Category.Description == "Business absence").Count();

                statistika.BusinessAbscenceDays += businessAbscenceDays;
                int publicHolidays = unit.Engagements.Get().Where(x => x.Team.Id == teamId).Select(x => x.Employee).SelectMany(y => y.Days).Where(x => x.Employee.Id == employee.Id
                                                         && x.Date.Value.Year == year
                                                         && x.Date.Value.Month == month
                                                         && x.Category.Description == "Public holiday").Count();
                statistika.PublicHolidays += publicHolidays;
                PTO += publicHolidays;

                int sickLeaveDays = unit.Engagements.Get().Where(x => x.Team.Id == teamId).Select(x => x.Employee).SelectMany(y => y.Days).Where(x => x.Employee.Id == employee.Id
                                                         && x.Date.Value.Year == year
                                                         && x.Date.Value.Month == month
                                                         && x.Category.Description == "Sick leave").Count();
                statistika.SickLeaveDays += sickLeaveDays;
                PTO += sickLeaveDays;

                int religiousDays = unit.Engagements.Get().Where(x => x.Team.Id == teamId).Select(x => x.Employee).SelectMany(y => y.Days).Where(x => x.Employee.Id == employee.Id
                                                         && x.Date.Value.Year == year
                                                         && x.Date.Value.Month == month
                                                         && x.Category.Description == "Religious day").Count();

                statistika.ReligiousDays += religiousDays;
                PTO += religiousDays;
                int otherAbsenceDays = unit.Engagements.Get().Where(x => x.Team.Id == teamId).Select(x => x.Employee).SelectMany(y => y.Days).Where(x => x.Employee.Id == employee.Id
                                                         && x.Date.Value.Year == year
                                                         && x.Date.Value.Month == month
                                                         && x.Category.Description == "Other absence").Count();
                statistika.OtherAbscenceDays += otherAbsenceDays;
                PTO += otherAbsenceDays;

                //decimal? totalhoursss = unit.Engagements.Get().Where(x => x.Team.Id == teamId).Select(x => x.Employee).SelectMany(y => y.Days).Where(x => x.Employee.Id == employee.Id
                //                                        && x.Date.Value.Year == year
                //                                        && x.Date.Value.Month == month && x.Category.Description == "Working day")
                //                                        .Select(x => x.Hours).DefaultIfEmpty(0).Sum();
                
                decimal? totalhoursss = 0;

                foreach (var project in projects)
                {
                    var hoursAtProject = unit.Days.Get()
                        .Where(d => d.Employee.Id == employee.Id && d.Date.Value.Year == year && d.Date.Value.Month == month)
                        .SelectMany(a => a.Assignments).Where(a => a.Project.Id == project.Id).Select(h => h.Hours).DefaultIfEmpty(0).Sum();
                    
                    totalhoursss += hoursAtProject;
                }
                var maxPossibleHoursEmployee = listDates.Count() * 8;
                var missingEntries = (employee.BeginDate.Value.Date > date.Date) ? 0 : listDates.Except(days.Select(x => x.Date.Value.Day)).Count();
                statistika.MissingEntries += missingEntries;

                TeamMemberModel employeeModel = new TeamMemberModel()
                {
                    Employee = new BaseModel()
                    {
                        Id = employee.Id,
                        Name = employee.FirstName + ' ' + employee.LastName,

                    },
                    PTO = PTO,
                    Days = new DayStatisticModel()
                    {
                        WorkingDays = workingDays,
                        VacationDays = vacationDays,
                        BusinessAbscenceDays = businessAbscenceDays,
                        PublicHolidays = publicHolidays,
                        SickLeaveDays = sickLeaveDays,
                        ReligiousDays = religiousDays,
                        OtherAbscenceDays = otherAbsenceDays,
                        OvertimeHours = overtime,
                        PercentageOfWorkingDays = Math.Round(100 * (double)totalhoursss / maxPossibleHoursEmployee, 2),
                        MissingEntries = missingEntries,

                    },
                    TotalHours = totalhoursss,
                    MaxPossibleHours = maxPossibleHoursEmployee
                };
                
                reports.Add(employeeModel);
                hours += employeeModel.TotalHours;
            }
            //foreach(var employee in reports)
            //{
            //    hours += employee.TotalHours;
            //}
            //int numberOfEmployees = unit.Engagements.Get().Where(x => x.Team.Id == teamId).Count();
            var fullworkingdays = numberOfEmployeesInTeam * DateTimeHelper.ListOfWorkingDays(year, month).Count();
            int maxPossibleHours = NoDaysInMonth(year, month) * 8 * numberOfEmployeesInTeam;
            statistika.PercentageOfWorkingDays = Math.Round(100 * (double)hours / maxPossibleHours);
            foreach(var project in projects)
            {
                var hoursAtProject = unit.Days.Get()
                        .Where(d =>  d.Date.Value.Year == year && d.Date.Value.Month == month)
                        .SelectMany(a => a.Assignments).Where(a => a.Project.Id == project.Id).Select(h => h.Hours).DefaultIfEmpty(0).Sum();
                ProjectDetails.Add(new PersonalReportProjectModel()
                {
                    Id = project.Id,
                    Name = project.Name,
                    Hours = hoursAtProject
                });
            }

            var report = new TeamReport()
            {
                Id = teamId,
                Name = unit.Teams.Get(teamId).Name,
                NumberOfEmployees = numberOfEmployeesInTeam,
                NumberOfProjects = unit.Teams.Get(teamId).Projects.Count(),
                Dayss = statistika,
                Reports = reports,
                TotalHours = hours ,
                MaxPossibleHours = maxPossibleHours,
                Year = year,
                Month = month,
                Projects = ProjectDetails
                //Members = unit.Engagements.Get().Where(x => x.Team.Id == teamId).Select(y=> GetPersonalReport(unit,y.Employee.Id, year,month,factory)).ToList()
                // Ok(TimeUnit.GetPersonalReport(employeeId, year, month, TimeFactory));
            };
            return report;
        }
        public static CompanyReport GetCompanyReport(this UnitOfWork unit, int year, int month, ModelFactory factory)
        {
            int days = DateTime.DaysInMonth(year, month);
            DateTime Date = new DateTime(year, month, days);
            var AllTeams = unit.Teams.Get().ToList();
            List<CompanyTeamModel> Teams = new List<CompanyTeamModel>();
            List<CompanyProjectModel> Projects = new List<CompanyProjectModel>();
            decimal? FulltotalHours = 0;
            
            foreach (var team in AllTeams)
            {
                int PTO = 0;
                decimal? teamTotalHours = 0;
                int CompMissingEntries = 0;
                decimal? overtimehours = 0;
                var projects = team.Projects;
                var employees = unit.Engagements.Get().Where(x => x.Team.Id == team.Id).Select(x => x.Employee).ToList();
                foreach (var employee in employees)
                {
                   decimal? overtime = 0;
                    var daysss = unit.Engagements.Get().Where(x => x.Team.Id == team.Id).Select(x => x.Employee).SelectMany(y => y.Days)
                    .Where(x => x.Employee.Id == employee.Id
                                                    && x.Date.Value.Year == year
                                                    && x.Date.Value.Month == month);

                    overtime = unit.Engagements.Get().Where(x => x.Team.Id == team.Id).Select(x => x.Employee).SelectMany(y => y.Days).Where(x => x.Employee.Id == employee.Id
                                                       && x.Date.Value.Year == year
                                                       && x.Date.Value.Month == month && x.Hours>8)
                                                       .Select(x => x.Hours).DefaultIfEmpty(0).Sum();
                    int overtimeCount = unit.Engagements.Get().Where(x => x.Team.Id == team.Id).Select(x => x.Employee).SelectMany(y => y.Days)
                    .Where(x => x.Employee.Id == employee.Id
                                                    && x.Date.Value.Year == year
                                                    && x.Date.Value.Month == month && x.Hours > 8).Select(x => x.Hours).Count();
                    overtime -= overtimeCount * 8;

                    overtimehours += overtime;

                    PTO +=  unit.Days.Get().Where(x => x.Employee.Id == employee.Id
                                                     && x.Date.Value.Year == year
                                                     && x.Date.Value.Month == month
                                                     && (x.Category.Description == "Vacation" || 
                                                     x.Category.Description == "Religious day" ||
                                                     x.Category.Description == "Other absence" ||
                                                     x.Category.Description == "Public holiday" ||
                                                     x.Category.Description == "Sick leave") 
                                                     ).Count();


                    //foreach (var day in daysss)
                    //{
                    //    if(day.Hours>8)
                    //        overtimehours += day.Hours - 8;
                    //}
                    //decimal? totalhoursss = unit.Engagements.Get().Where(x => x.Team.Id == team.Id).Select(x => x.Employee)
                    //                    .SelectMany(y => y.Days).Where(x => x.Employee.Id == employee.Id
                    //                                   && x.Date.Value.Year == year
                    //                                   && x.Date.Value.Month == month)
                    //                                   .Select(x => x.Hours).DefaultIfEmpty(0).Sum();
                    decimal? totalHours = 0;
                    foreach(var project in projects)
                    {
                        var hoursAtProject = unit.Days.Get()
                            .Where(d => d.Employee.Id == employee.Id && d.Date.Value.Year == year && d.Date.Value.Month == month)
                            .SelectMany(a => a.Assignments).Where(a => a.Project.Id == project.Id).Select(h => h.Hours).DefaultIfEmpty(0).Sum();
                        totalHours += hoursAtProject;
                    }
                    teamTotalHours += totalHours;


                    List<int> listDates = null;
                    var date = new DateTime(year, month, 1);

                    if (employee.BeginDate.Value.Year == date.Year && employee.BeginDate.Value.Month == date.Month
                        && employee.BeginDate.Value.Day < DateTime.DaysInMonth(year, month))
                    {
                        listDates = DateTimeHelper.ListOfWorkingDays(year, month, employee.BeginDate.Value.Day).ToList();
                    }
                    else
                    {
                        listDates = DateTimeHelper.ListOfWorkingDays(year, month).ToList();
                    }
                    var missingEntries = (employee.BeginDate.Value.Date > date.Date) ? 0 : listDates.Except(daysss.Select(x => x.Date.Value.Day)).Count();
                    CompMissingEntries += missingEntries;
                    
                }
                var totalPossibleHours = NoDaysInMonth(year, month) * employees.Count() * 8;
                var Utilization = Math.Round(((double)teamTotalHours / totalPossibleHours) * 100, 2);
                CompanyTeamModel teamToAdd = new CompanyTeamModel()
                {
                    Name = team.Name,
                    OvertimeHours = overtimehours,
                    MissingEntries = CompMissingEntries,
                    TotalHours =teamTotalHours ,
                    Utilization = Utilization,
                    TotalPossibleHours = totalPossibleHours,
                    PTO = PTO
                };
                Teams.Add(teamToAdd);
                FulltotalHours += teamTotalHours;
            }
            int numberOfEmployees = unit.Employees.Get().Where(x => x.BeginDate <= Date && (x.EndDate == null || x.EndDate > Date || x.EndDate.Value.Month == month)).Count();
            int maxPossibleHours = NoDaysInMonth(year, month) * 8 * numberOfEmployees;
            // pm utilization

            int pmCount = unit.Employees.Get()
                                          .Where(x => x.Position.Id == "MGR" && x.BeginDate <= Date
                                                            && (x.EndDate == null
                                                            || x.EndDate > Date
                                                            || x.EndDate.Value.Month == month))
                                          .Count();

            var pmWorkingDays = unit.Days.Get()
                                            .Where(x => x.Category.Description == "Working day"
                                                            && x.Date.Value.Month == month
                                                            && x.Date.Value.Year == year && x.Employee.Position.Id == "MGR"
                                                            && x.Employee.BeginDate <= Date
                                                            && (x.Employee.EndDate == null
                                                                    || x.Employee.EndDate > Date
                                                                    || x.Employee.EndDate.Value.Month == month))
                                            .Count();

            decimal? pmUtil = 0;
            if ((NoDaysInMonth(year, month) * pmCount) != 0)
            {
                pmUtil = Math.Round((decimal)(pmWorkingDays / (decimal)(NoDaysInMonth(year, month) * pmCount)) * 100, 2);
            }


            //qa utilization
            int qaCount = unit.Employees.Get()
                              .Where(x => x.Position.Id == "QAE" && x.BeginDate <= Date
                                                && (x.EndDate == null
                                                || x.EndDate > Date
                                                || x.EndDate.Value.Month == month))
                              .Count();

            var qaWorkingDays = unit.Days.Get()
                                            .Where(x => x.Category.Description == "Working day"
                                                            && x.Date.Value.Month == month
                                                            && x.Date.Value.Year == year && x.Employee.Position.Id == "QAE"
                                                            && x.Employee.BeginDate <= Date
                                                            && (x.Employee.EndDate == null
                                                                    || x.Employee.EndDate > Date
                                                                    || x.Employee.EndDate.Value.Month == month))
                                            .Count();

            decimal qaUtil = 0;
            if (NoDaysInMonth(year, month) * qaCount != 0)
            {
                qaUtil = Math.Round((decimal)(qaWorkingDays / (decimal)(NoDaysInMonth(year, month) * qaCount)) * 100, 2);
            }


            //dev utilization
            int devCount = unit.Employees.Get()
                              .Where(x => x.Position.Id == "DEV" && x.BeginDate <= Date
                                                && (x.EndDate == null
                                                || x.EndDate > Date
                                                || x.EndDate.Value.Month == month))
                              .Count();

            var devWorkingDays = unit.Days.Get()
                                            .Where(x => x.Category.Description == "Working day"
                                                            && x.Date.Value.Month == month
                                                            && x.Date.Value.Year == year && x.Employee.Position.Id == "DEV"
                                                            && x.Employee.BeginDate <= Date
                                                            && (x.Employee.EndDate == null
                                                                    || x.Employee.EndDate > Date
                                                                    || x.Employee.EndDate.Value.Month == month))
                                            .Count();

            decimal devUtil = 0;
            if (NoDaysInMonth(year, month) * devCount != 0)
            {
                devUtil = Math.Round((decimal)(devWorkingDays / (decimal)(NoDaysInMonth(year, month) * devCount)) * 100, 2);
            }


            //uiux utilization
            int uiuxCount = unit.Employees.Get()
                                          .Where(x => x.Position.Id == "UIX"&& x.BeginDate <= Date
                                                            && (x.EndDate == null
                                                            || x.EndDate > Date
                                                            || x.EndDate.Value.Month == month))
                                          .Count();

            var uiuxWorkingDays = unit.Days.Get()
                                            .Where(x => x.Category.Description == "Working day"
                                                            && x.Date.Value.Month == month
                                                            && x.Date.Value.Year == year&& x.Employee.Position.Id == "UIX"
                                                            && x.Employee.BeginDate <= Date
                                                            && (x.Employee.EndDate == null
                                                                    || x.Employee.EndDate > Date
                                                                    || x.Employee.EndDate.Value.Month == month))
                                            .Count();

            decimal uiuxUtil = 0;
            if (NoDaysInMonth(year, month) * uiuxCount != 0)
            {
                uiuxUtil = Math.Round((decimal)(uiuxWorkingDays / (decimal)(NoDaysInMonth(year, month) * uiuxCount)) * 100, 2);
            }
            var totalCompanyHours = unit.Days.Get().Where(x => x.Date.Value.Year == year && x.Date.Value.Month == month).Select(x => x.Hours).DefaultIfEmpty(0).Sum();
            double Utiliziation = Math.Round(((double)totalCompanyHours / maxPossibleHours)*100, 2);
            CompanyReport report = new CompanyReport()
            {
                Year = year,
                Month = month,
                NumberOfEmployees = numberOfEmployees,
                NumberOfProjects = unit.Projects.Get().Where(x=> x.BeginDate <= Date
                                                            && (x.EndDate == null
                                                            || x.EndDate > Date
                                                            || x.EndDate.Value.Month == month)).Count(),
                //unit.Days.Get().Where(x => x.Date.Value.Year == year && x.Date.Value.Month == month).Select(x => x.Hours).DefaultIfEmpty(0).Sum()
                TotalHours = totalCompanyHours,
                MaxPossibleHours = maxPossibleHours,
                Utilization = Utiliziation,
                PMUtilization = pmUtil.Value,
                PMCount = pmCount,
                DevUtilization = devUtil,
                DevCount = devCount,
                QAUtilization = qaUtil,
                QACount = qaCount,
                UIUXUtilization = uiuxUtil,
                UIUXCount = uiuxCount,
                Teams = Teams,
                Projects = unit.Projects.Get(x => x.BeginDate <= Date
                                                            && (x.EndDate == null
                                                            || x.EndDate > Date
                                                            || x.EndDate.Value.Month == month)).Select(x=> new CompanyProjectModel()
                                                            {
                                                                Name = x.Name,
                                                                Revenue = x.Amount,
                                                                Team = x.Team.Name
                                                            }).ToList()
            };
     


            return report;
        }
        public static int NoDaysInMonth(int year, int month)
        {
            int days = DateTime.DaysInMonth(year, month);
            int daysInMonth = 0;

            for (int i = 1; i <= days; i++)
            {
                DateTime day = new DateTime(year, month, i);
                if (day.DayOfWeek != DayOfWeek.Saturday && day.DayOfWeek != DayOfWeek.Sunday)
                {
                    daysInMonth++;
                }
            }

            return daysInMonth;
        }

        //invoices
        public static List<ProjectInvoiceModel> GetInvoiceReport(this UnitOfWork unit, int year, int month)
        {
            var listInvoices = unit.Projects.Get().SelectMany(x => x.Assignments)
                                                  .Where(x => x.Day.Date.Value.Month == month && x.Day.Date.Value.Year == year)
                                                  .GroupBy(y => y.Project)
                                                  .Select(w => new ProjectInvoiceModel()
                                                  {
                                                      ProjectName = w.Key.Name,
                                                      CustomerName = w.Key.Customer.Name,
                                                      CustomerEmail = w.Key.Customer.Email,
                                                      InvoiceDate = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString(),
                                                      Amount = w.Key.Amount,
                                                      Roles = w.Key.Team.Engagements
                                                                        .Where(t => t.Team.Id == w.Key.TeamId)
                                                                        .GroupBy(r => r.Role)
                                                                        .Select(x => new RoleInvoiceModel()
                                                                        {
                                                                            Description = x.Key.Name,
                                                                            Quantity = x.Key.Engagements
                                                                                           .Where(t => t.Team.Id == w.Key.TeamId)
                                                                                           .Select(em => em.Employee)
                                                                                           .SelectMany(d => d.Days)
                                                                                           .Where(g => g.Date.Value.Month == month && g.Date.Value.Year == year && g.Category.Description == "Working day")
                                                                                           .SelectMany(a => a.Assignments)
                                                                                           .Where(p => p.Project.Id == w.Key.Id)
                                                                                           .Select(h => h.Hours)
                                                                                           .DefaultIfEmpty(0)
                                                                                           .Sum(),
                                                                            Unit = "Hours",
                                                                            UnitPrice = x.Key.HourlyRate,
                                                                            SubTotal = x.Key.HourlyRate * x.Key.Engagements
                                                                                           .Where(t => t.Team.Id == w.Key.TeamId)
                                                                                           .Select(em => em.Employee)
                                                                                           .SelectMany(d => d.Days)
                                                                                           .Where(g => g.Date.Value.Month == month && g.Date.Value.Year == year && g.Category.Description == "Working day")
                                                                                           .SelectMany(a => a.Assignments)
                                                                                           .Where(p => p.Project.Id == w.Key.Id)
                                                                                           .Select(h => h.Hours)
                                                                                           .DefaultIfEmpty(0)
                                                                                           .Sum(),
                                                                            Status = "Included"

                                                                        })
                                                                        .ToList(),
                                                      InvoiceNumber = w.Key.Name.ToString().Substring(0, 2).ToUpper() + "-" + w.Key.Customer.Name.ToString().Substring(0, 2).ToUpper() + "-" + w.Key.TeamId.ToString() + "-" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString().Substring(2, 4) + "-" + month + "/" + year.ToString().Substring(2, 4),
                                                      Status = "Not sent"



                                                  })
                                                  .ToList();


            var conString = "mongodb://localhost:27017";

            var client = new MongoClient(conString);

            var DB = client.GetDatabase("Billings");
            var collection = DB.GetCollection<EmailCustomerModel>("Invoices");

            foreach (var invoice in listInvoices)
            {

                int exist = (int)collection.Find(x => x.InvoiceNumber == invoice.InvoiceNumber).Count();
                if (exist == 0)
                {
                    string MailToSend = "Dear " + invoice.CustomerName + " we are sending You invoice for "
                   + invoice.ProjectName + " for " + invoice.InvoiceDate + ". InvoiceNumber: " + invoice.InvoiceNumber;
                    EmailCustomerModel invoiceToAdd = new EmailCustomerModel()
                    {
                        Name = invoice.CustomerName,
                        Email = invoice.CustomerEmail,
                        Roles = invoice.Roles,
                        InvoiceDate = invoice.InvoiceDate,
                        MailToSend = MailToSend,
                        ProjectName = invoice.ProjectName,
                        InvoiceNumber = invoice.InvoiceNumber,
                        Status = invoice.Status,
                        SentOrCancelDate = "None",
                        Amount = invoice.Amount

                    };
                    collection.InsertOne(invoiceToAdd);
                }

            }

            //GetFromMongo();
            return null;
            //return listInvoices;
        }

        public static List<EmailCustomerModel> GetFromMongo(this UnitOfWork unit, int year, int month)
        {
            GetInvoiceReport(unit, year, month);

            var conString = "mongodb://localhost:27017";

            var client = new MongoClient(conString);

            var DB = client.GetDatabase("Billings");
            var collection = DB.GetCollection<EmailCustomerModel>("Invoices");

            List<EmailCustomerModel> invoices = collection.Find(_ => true).ToList();

            return invoices;


        }



    }

   

    // za radni dani, vacation days i slicno
      // (int)x.Type = (int)Calendar.CategoryType.BussinessAbsense).Count();
}

//public static PersonalReport GetPersonalReport(this UnitOfWork unit, int employeeId, int year, int month, ModelFactory)
//{
// var emp = (FullEmployeeModel)unit.employees.get().where
//    //unit.Days.Get().Where(nesto).select(x => new Model() {
//    //    varijable = varijable
//    //        Tasks = x.tasks.select(y => new Model()
//    //        {
//    //            varijable...
//    //        }
//    //    )).tolist();
//}