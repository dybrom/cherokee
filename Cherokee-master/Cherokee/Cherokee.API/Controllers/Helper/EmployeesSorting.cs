using Cherokee.API.Models;
using Cherokee.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cherokee.API.Controllers.Helper
{
    public class EmployeesSorting : BaseController, ISorting<Employee, EmployeeModel>
    {
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int ItemCount { get; set; }

        public IQueryable<Employee> Sort(IQueryable<Employee> query, int sort)
        {
            switch (sort)
            {
                case 1: query = query.OrderBy(x => x.LastName); break;
                case 2: query = query.OrderBy(x => x.BirthDate); break;
                default: query = query.OrderBy(x => x.Id); break;

            }

            return query;
        }

        public List<EmployeeModel> Paginate(IQueryable<Employee> query, int pageSize, int page)
        {
            ItemCount = query.Count();
            TotalPages = (int)Math.Ceiling((double)ItemCount / pageSize);

            var list = query.Skip(pageSize * page)
                                          .Take(pageSize)
                                          .ToList()
                                          .Select(t => TimeFactory.Create(t))
                                          .ToList();

            return list;

        }

        public IQueryable<Employee> Filter(IQueryable<Employee> query, string filter)
        {
            if (filter != "") query = query.Where(e => e.LastName.Contains(filter) || e.FirstName.Contains(filter));

            return query;
        }
    }
}