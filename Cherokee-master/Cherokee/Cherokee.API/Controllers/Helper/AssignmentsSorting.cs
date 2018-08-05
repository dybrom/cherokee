using Cherokee.API.Models;
using Cherokee.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cherokee.API.Controllers.Helper
{
    public class AssignmentsSorting : BaseController, ISorting<Assignment, DetailModel>
    {
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int ItemCount { get; set; }

        public IQueryable<Assignment> Sort(IQueryable<Assignment> query, int sort)
        {
            switch (sort)
            {
                case 1: query = query.OrderBy(x => x.Day.Date); break;
                case 2: query = query.OrderBy(x => x.Project.Id); break;
                default: query = query.OrderBy(x => x.Id); break;

            }

            return query;
        }

        public List<DetailModel> Paginate(IQueryable<Assignment> query, int pageSize, int page)
        {
            ItemCount = TimeUnit.Assignments.Get().Count();
            TotalPages = (int)Math.Ceiling((double)ItemCount / pageSize);

            var list = query.Skip(pageSize * page)
                                          .Take(pageSize)
                                          .ToList()
                                          .Select(t => TimeFactory.Create(t))
                                          .ToList();

            return list;

        }

        public IQueryable<Assignment> Filter(IQueryable<Assignment> query, string filter)
        {
            if (filter != "") query = query.Where(e => e.Day.Employee.FullName.Contains(filter));

            return query;
        }
    }
}