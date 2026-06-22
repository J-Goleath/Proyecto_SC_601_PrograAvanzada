using Caso1.infraestructure.DBContexts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Caso1.infraestructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly Caso1Context Context;
        protected readonly DbSet<T> DbSet;

        public Repository(Caso1Context context)
        {
            Context = context;
            DbSet = Context.Set<T>();
        }

        public void Add(T entity)
        {
            DbSet.Add(entity);
            Context.SaveChanges();
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate).ToList();
        }

        public IEnumerable<T> GetAll()
        {
            return DbSet.ToList();
        }

        public T GetById(int id)
        {
            return DbSet.Find(id);
        }

        public void Remove(T entity)
        {
            DbSet.Remove(entity);
            Context.SaveChanges();
        }

        
        public void Update(T entity)
        {
            
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty == null)
            {
                throw new InvalidOperationException("La entidad no tiene una propiedad 'Id'");
            }

            var id = idProperty.GetValue(entity);

            
            var existingEntity = DbSet.Find(id);

            
            if (existingEntity != null && !ReferenceEquals(existingEntity, entity))
            {
                Context.Entry(existingEntity).State = EntityState.Detached;
            }

            
            DbSet.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;

           
            Context.SaveChanges();
        }
    }
}