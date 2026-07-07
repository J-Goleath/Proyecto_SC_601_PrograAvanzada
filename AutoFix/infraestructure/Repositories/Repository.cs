using AutoFix.infraestructure.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace AutoFix.infraestructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AutoFixContext Context;
        protected readonly DbSet<T> DbSet;

        public Repository(AutoFixContext context)
        {
            Context = context;
            DbSet = Context.Set<T>();
        }

        public virtual void Add(T entity)
        {
            DbSet.Add(entity);
            Context.SaveChanges();
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate).ToList();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return DbSet.ToList();
        }

        public virtual T GetById(int id)
        {
            return DbSet.Find(id);
        }

        public virtual void Remove(T entity)
        {
            DbSet.Remove(entity);
            Context.SaveChanges();
        }

        public virtual void Update(T entity)
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