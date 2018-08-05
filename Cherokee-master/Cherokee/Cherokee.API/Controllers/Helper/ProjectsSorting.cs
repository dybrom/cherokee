using Cherokee.API.Models;
using Cherokee.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cherokee.API.Controllers.Helper
{
    public class ProjectsSorting : BaseController, ISorting<Project, ProjectModel>
    {
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int ItemCount { get; set; }

        public IQueryable<Project> Sort(IQueryable<Project> query, int sort)
        {
            switch (sort)
            {
                case 1: query = query.OrderBy(x => x.Name); break;
                case 2: query = query.OrderByDescending(x => x.Amount); break;
                default: query = query.OrderBy(x => x.Id); break;

            }

            return query;
        }

        public List<ProjectModel> Paginate(IQueryable<Project> query, int pageSize, int page)
        {
            ItemCount = TimeUnit.Projects.Get().Count();
            TotalPages = (int)Math.Ceiling((double)ItemCount / pageSize);

            var list = query.Skip(pageSize * page)
                                          .Take(pageSize)
                                          .ToList()
                                          .Select(t => TimeFactory.Create(t))
                                          .ToList();

            return list;

        }

        public IQueryable<Project> Filter(IQueryable<Project> query, string filter)
        {
            if (filter != "") query = query.Where(e => e.Name.Contains(filter));

            return query;
        }
    }
}