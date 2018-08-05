using Cherokee.API.Models;
using Cherokee.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cherokee.API.Controllers.Helper
{
    public class CustomersSorting : BaseController, ISorting<Customer, CustomerModel>
    {
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int ItemCount { get; set; }

        public IQueryable<Customer> Sort(IQueryable<Customer> query, int sort)
        {
            switch (sort)
            {
                case 1: query = query.OrderBy(x => x.Name); break;
                case 2: query = query.OrderByDescending(x => x.Projects.Count()); break;
                default: query = query.OrderBy(x => x.Id); break;

            }

            return query;
        }

        public List<CustomerModel> Paginate(IQueryable<Customer> query, int pageSize, int page)
        {
            ItemCount = TimeUnit.Customers.Get().Count();
            TotalPages = (int)Math.Ceiling((double)ItemCount / pageSize);

            var list = query.Skip(pageSize * page)
                                          .Take(pageSize)
                                          .ToList()
                                          .Select(t => TimeFactory.Create(t))
                                          .ToList();

            return list;

        }

        public IQueryable<Customer> Filter(IQueryable<Customer> query, string filter)
        {
            if (filter != "") query = query.Where(e => e.Name.Contains(filter));

            return query;
        }
    }
}