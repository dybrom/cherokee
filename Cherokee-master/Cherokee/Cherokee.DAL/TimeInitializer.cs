using Cherokee.DAL;
using Cherokee.DAL.Entities;
using Cherokee.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeKeeper.DAL
{
    internal class TimeInitializer<T> : DropCreateDatabaseAlways<CherokeeContext>
    {
        public override void InitializeDatabase(CherokeeContext context)
        {
            try
            {
                // ensure that old database instance can be dropped
                context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction,
                        $"ALTER DATABASE {context.Database.Connection.Database} SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
            }
            catch
            {
                // database does not exists - no problem ;o)
            }
            finally
            {
                base.InitializeDatabase(context);

                using (UnitOfWork unit = new UnitOfWork())
                {
                    AddRoles(unit);
                    AddTeams(unit);
                    AddCategories(unit);
                    AddEmployees(unit);
                    AddDays(unit);
                    AddEngagements(unit);
                    AddCustomers(unit);
                    AddProjects(unit);
                    AddAssignments(unit);
                }
            }
        }

        void AddRoles(UnitOfWork unit)
        {
            unit.Roles.Insert(new Role()
            {
                Id = "SD",
                Name = "Software Developer",
                Type = RoleType.TeamRole,
                HourlyRate = 30,
                MonthlyRate = 4500
            });
            unit.Roles.Insert(new Role()
            {
                Id = "UX",
                Name = "UI/UX Designer",
                Type = RoleType.TeamRole,
                HourlyRate = 45,
                MonthlyRate = 6500
            });
            unit.Save();
        }
        void AddCategories(UnitOfWork unit)
        {
            unit.Categories.Insert(new Category()
            {
                Description = "WorkingDay"
            });
            unit.Categories.Insert(new Category()
            {
                Description = "SickDay"
            });
            unit.Save();
        }
        void AddTeams(UnitOfWork unit)
        {
            unit.Teams.Insert(new Team()
            {
                Name = "Alpha",
                Id = "A",
                Image = "A",
                Description = "Alpha Team"
            });
            unit.Teams.Insert(new Team()
            {
                Name = "Bravo",
                Id = "B",
                Image = "B",
                Description = "Bravo Team"
            });
            unit.Save();
        }

        void AddEmployees(UnitOfWork unit)
        {
            unit.Employees.Insert(new Employee()
            {
                FirstName = "John",
                LastName = "Doe",
                Image = "image",
                Email = "blabla@bla.com",
                Phone = "969699696",
                BirthDate = DateTime.Now,
                BeginDate = DateTime.Now,
                EndDate = null,
                Position = unit.Roles.Get("UX"),
            });
            unit.Employees.Insert(new Employee()
            {
                FirstName = "Nhoj",
                LastName = "Eod",
                Image = "imagge",
                Email = "blablablaa@blabla.com",
                Phone = "329120391",
                BirthDate = DateTime.Now,
                BeginDate = DateTime.Now,
                EndDate = null,
                Position = unit.Roles.Get("SD"),
            });
            unit.Save();
        }
        void AddDays(UnitOfWork unit)
        {
            unit.Days.Insert(new Day()
            {
                Date = DateTime.Now,
                Hours = 20,
                Comment = "First Day",
                Employee = unit.Employees.Get(1),
                Category = unit.Categories.Get(1)
            });
            unit.Days.Insert(new Day()
            {
                Date = DateTime.Now,
                Hours = 25,
                Comment = "Second Day",
                Employee = unit.Employees.Get(1),
                Category = unit.Categories.Get(1)
            });

            unit.Save();
        }
        void AddEngagements(UnitOfWork unit)
        {
            unit.Engagements.Insert(new Engagement()
            {
                Hours = 20,
                Employee = unit.Employees.Get(1),
                Team = unit.Teams.Get("A"),
                Role = unit.Roles.Get("SD")
            });
            unit.Engagements.Insert(new Engagement()
            {
                Hours = 25,
                Employee = unit.Employees.Get(1),
                Team = unit.Teams.Get("B"),
                Role = unit.Roles.Get("UX")
            });


            unit.Save();
        }
        void AddCustomers(UnitOfWork unit)
        {
            unit.Customers.Insert(new Customer()
            {
                Name = "First Customer",
                Address = { Street = "Ulica", ZipCode = "71000", City = "Sarajevo" }
            });
            unit.Customers.Insert(new Customer()
            {
                Name = "Second Customer",
                Address = { Street = "Ulica2", ZipCode = "72000", City = "Zarajevo" }
            });
            unit.Save();
        }
        void AddProjects(UnitOfWork unit)
        {
            unit.Projects.Insert(new Project()
            {
                Name = "First Project",
                Amount = 55.00m,
                Monogram = "monogram",
                Team = unit.Teams.Get("A"),
                Customer = unit.Customers.Get(1),
                Description = "Something about the project"
            });
            unit.Projects.Insert(new Project()
            {
                Name = "First Project",
                Team = unit.Teams.Get("B"),
                Customer = unit.Customers.Get(1),
                Description = "Something about the second project"
            });

            unit.Save();
        }
        void AddAssignments(UnitOfWork unit)
        {
            unit.Assignments.Insert(new Assignment()
            {
                Description = "First assignment that needs to be finisnhed",
                Project = unit.Projects.Get(1),
                Day = unit.Days.Get(1)
            });

            unit.Assignments.Insert(new Assignment()
            {
                Description = "Second assignment that needs to be finisnhed",
                Project = unit.Projects.Get(1),
                Day = unit.Days.Get(1)
            });

            unit.Save();
        }
    }
}

//Database.SetInitializer<TimeContext>(new CreateDatabaseIfNotExists<TimeContext>());
//Database.SetInitializer<TimeContext>(new DropCreateDatabaseIfModelChanges<TimeContext>());
//base.Database.ExecuteSqlCommand("USE master; ALTER DATABASE Testera SET SINGLE_USER WITH ROLLBACK IMMEDIATE;");
//Database.SetInitializer<TimeContext>(new DropCreateDatabaseAlways<TimeContext>());