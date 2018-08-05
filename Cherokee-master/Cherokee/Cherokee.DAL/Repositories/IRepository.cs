using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cherokee.DAL.Repositories
{
    public interface IRepository<ClassEntity, T>
    {
        // Omogucavanje prikupljanje cijele baze za odredjeni entitet
        IQueryable<ClassEntity> Get();
        List<ClassEntity> Get(Func<ClassEntity, bool> where);
        //Omogucavanje dobijanja jednogg reda unutar entiteta preko ID-a
        ClassEntity Get(T id);

        void Insert(ClassEntity entity);
        void Update(ClassEntity entity, T id);
        void Delete(ClassEntity entity);
        void Delete(T id);
    }
}
