using Cherokee.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeKeeper.DAL;

namespace Cherokee.DAL
{
    public class CherokeeContext: DbContext
    {
        public CherokeeContext() : base("name=TimeKeeper")
        {
            if (Database.Connection.Database == "Testera")
            {
                Database.SetInitializer(new TimeInitializer<CherokeeContext>());
            }
        }

       

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Engagement> Engagements { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Category> Categories { get; set; }




       




        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Ignore<BaseClass<int>>();
            modelBuilder.Ignore<BaseClass<string>>();

            
            

            modelBuilder.Entity<Customer>()
                        .Map(m => m.Requires("Deleted").HasValue(false))
                        .Ignore(m => m.Deleted);
            modelBuilder.Entity<Day>()
                        .Map(m => m.Requires("Deleted").HasValue(false))
                        .Ignore(m => m.Deleted);
            modelBuilder.Entity<Employee>()
                        .Map(m => m.Requires("Deleted").HasValue(false))
                        .Ignore(m => m.Deleted);
            modelBuilder.Entity<Engagement>()
                        .Map(m => m.Requires("Deleted").HasValue(false))
                        .Ignore(m => m.Deleted);
            modelBuilder.Entity<Project>()
                        .Map(m => m.Requires("Deleted").HasValue(false))
                        .Ignore(m => m.Deleted);
            modelBuilder.Entity<Role>()
                        .Map(m => m.Requires("Deleted").HasValue(false))
                        .Ignore(m => m.Deleted);
            modelBuilder.Entity<Assignment>()
                        .Map(m => m.Requires("Deleted").HasValue(false))
                        .Ignore(m => m.Deleted);
            modelBuilder.Entity<Team>()
                        .Map(m => m.Requires("Deleted").HasValue(false))
                        .Ignore(m => m.Deleted);
            modelBuilder.Entity<Category>()
                        .Map(m => m.Requires("Deleted").HasValue(false))
                        .Ignore(m => m.Deleted);
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries().Where(p => p.State == EntityState.Deleted)) SoftDelete(entry);
            return base.SaveChanges();
        }

        private void SoftDelete(DbEntityEntry entry)
        {
            Type entryEntityType = entry.Entity.GetType();
            string tableName = GetTableName(entryEntityType);
            string sql = string.Format("UPDATE {0} SET Deleted = 1 WHERE id = @id", tableName);
            Database.ExecuteSqlCommand(sql, new SqlParameter("@id", entry.OriginalValues["Id"]));
            entry.State = EntityState.Detached;
        }

        private static Dictionary<Type, EntitySetBase> _mappingCache = new Dictionary<Type, EntitySetBase>();

        private string GetTableName(Type type)
        {
            EntitySetBase es = GetEntitySet(type);
            return string.Format("[{0}].[{1}]", es.MetadataProperties["Schema"].Value, es.MetadataProperties["Table"].Value);
        }

        private EntitySetBase GetEntitySet(Type type)
        {
            if (!_mappingCache.ContainsKey(type))
            {
                ObjectContext octx = ((IObjectContextAdapter)this).ObjectContext;
                string typeName = ObjectContext.GetObjectType(type).Name;
                var es = octx.MetadataWorkspace.GetItemCollection(DataSpace.SSpace).GetItems<EntityContainer>().SelectMany(c => c.BaseEntitySets.Where(e => e.Name == typeName)).FirstOrDefault();
                if (es == null) throw new ArgumentException("Entity type not found in GetTableName", typeName);
                _mappingCache.Add(type, es);
            }
            return _mappingCache[type];
        }
    }
}

