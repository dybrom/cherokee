using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cherokee.DAL.Repositories
{
    public class Repository<ClassEntity, T> : IRepository<ClassEntity, T> where ClassEntity : class
    {
        protected CherokeeContext context;
        protected DbSet<ClassEntity> dbSet;
        

        public Repository(CherokeeContext _context)
        {
            context = _context;
            dbSet = context.Set<ClassEntity>();
        }

        public IQueryable<ClassEntity> Get()
        {
            return dbSet;
        }
        public List<ClassEntity> Get(Func<ClassEntity, bool> where)
        {
            //List<ClassEntity> list;
            //IQueryable<ClassEntity> dbQuery = dbSet;
            //list = dbQuery.Where(where).ToList();
            //return list;
            return dbSet.Where(where).ToList();
        }

        public void Delete(ClassEntity entity)
        {
            dbSet.Remove(entity);
        }

        public void Delete(T id)
        {
            ClassEntity entity = Get(id);
            if (entity != null)
            {
                
                dbSet.Remove(entity);
            }
        }

        public ClassEntity Get(T id)
        {
            
            return dbSet.Find(id);
        }

        public void Insert(ClassEntity entity)
        {
            dbSet.Add(entity);
            Utility.Log($"REPOSITORY: inserted", "INFO");
        }

        public void Update(ClassEntity entity, T id)
        {
            ClassEntity old = Get(id);
            if (old != null)
            {
                context.Entry(old).CurrentValues.SetValues(entity);
            }
            Utility.Log($"REPOSITORY: updated", "INFO");
        }
    }
}
