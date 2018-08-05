using Cherokee.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cherokee.DAL.Repositories
{
    public class UnitOfWork : IDisposable
    {
        private readonly CherokeeContext context = new CherokeeContext();
        private IRepository<Access,int> _accesses;
        private IRepository<Assignment, int> _assignments;
        private IRepository<Category, int> _categories;
        private IRepository<Customer, int> _customers;
        private IRepository<Day, int> _days;
        private IRepository<Employee, int> _employees;
        private IRepository<Engagement, int> _engagements;
        private IRepository<Project, int > _projects;
        private IRepository<Role, string> _roles;
        private IRepository<Team, string> _teams;

        public IRepository<Access, int> Accesses
        {
            get
            {
                if (_accesses == null) _accesses = new Repository<Access, int>(context);
                return _accesses;
            }
        }
        public IRepository<Assignment,int> Assignments
        {
            get
            {
                return _assignments ?? (_assignments = new Repository<Assignment, int>(context)); 
            }
        }
        public IRepository<Category, int> Categories
        {
            get
            {
                return _categories ?? (_categories = new Repository<Category, int>(context));
            }
        }
        public IRepository<Customer, int> Customers
        {
            get
            {
                return _customers ?? (_customers = new Repository<Customer, int>(context));
            }
        }
        public IRepository<Day, int> Days
        {
            get
            {
                return _days ?? (_days = new Repository<Day, int>(context));
            }
        }
        public IRepository<Employee, int> Employees
        {
            get
            {
                return _employees ?? (_employees = new Repository<Employee, int>(context));
            }
        }
        public IRepository<Engagement, int> Engagements
        {
            get
            {
                return _engagements ?? (_engagements = new Repository<Engagement, int>(context));
            }
        }
        public IRepository<Project, int> Projects
        {
            get
            {
                return _projects ?? (_projects = new Repository<Project, int>(context));
            }
        }
        public IRepository<Role, string> Roles
        {
            get
            {
                return _roles ?? (_roles = new Repository<Role, string>(context));
            }
        }
        public IRepository<Team, string> Teams
        {
            get
            {
                return _teams ?? (_teams = new Repository<Team, string>(context));
            }
        }





        public void Dispose()
        {
            context.Dispose();
        }
        public bool Save()
        {
            return (context.SaveChanges() > 0);
        }
    }
}
