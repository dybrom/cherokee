//using Cherokee.API.Models;
//using Cherokee.DAL.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace Cherokee.API.Controllers.Helper
//{
//    public class DaysSorting : BaseController, ISorting<Day, DayModel>
//    {
//        public int TotalPages { get; set; }
//        public int PageSize { get; set; }
//        public int Page { get; set; }
//        public int ItemCount { get; set; }

//        public IQueryable<Day> Sort(IQueryable<Day> query, int sort)
//        {
//            switch (sort)
//            {
//                case 1: query = query.OrderBy(x => x.Category.Description); break;
//                case 2: query = query.OrderBy(x => x.Date); break;
//                default: query = query.OrderBy(x => x.Id); break;

//            }

//            return query;
//        }

//        public List<DayModel> Paginate(IQueryable<Day> query, int pageSize, int page)
//        {
//            ItemCount = TimeUnit.Days.Get().Count();
//            TotalPages = (int)Math.Ceiling((double)ItemCount / pageSize);

//            var list = query.Skip(pageSize * page)
//                                          .Take(pageSize)
//                                          .ToList()
//                                          .Select(t => TimeFactory.Create(t))
//                                          .ToList();

//            return list;

//        }

//        public IQueryable<Day> Filter(IQueryable<Day> query, string filter)
//        {
//            if (filter != "") query = query.Where(e => e.Employee.FullName.Contains(filter));

//            return query;
//        }
//    }
//}