using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DevTeam.EntityFrameworkExtensions.DbContext;

namespace DevTeam.GenericRepository
{
    public class GenericRepository<TEntity>: IGenericRepository<TEntity>
        where TEntity: class, IEntity, new()
    {
        protected readonly IDbContext Context;
        protected readonly IGenericRepository Repository;

        public GenericRepository(IDbContext context, IGenericRepository repository)
        {
            Context = context;
            Repository = repository;
        }

        public virtual IQueryable<TEntity> Query()
        {
            return Repository.Query<TEntity>();
        }

        public virtual IQueryable<TType> Set<TType>()
            where TType: class, IEntity, new()
        {
            return Context.Set<TType>().AsQueryable();
        }

        public virtual TEntity Get(int id)
        {
            return Repository.Get<TEntity>(id);
        }

        public virtual TEntity Get(Expression<Func<TEntity, bool>> selector)
        {
            return Repository.Get(selector);
        }

        public virtual IQueryable<TEntity> GetList(Expression<Func<TEntity, bool>> selector = null)
        {
            return Repository.GetList(selector);
        }

        public virtual TProperty GetProperty<TProperty>(int id, Expression<Func<TEntity, TProperty>> property)
        {
            return Repository.GetProperty(id, property);
        }

        public virtual TProperty GetProperty<TProperty>(Expression<Func<TEntity, bool>> entity, 
                                                Expression<Func<TEntity, TProperty>> property)
        {
            return Repository.GetProperty(entity, property);
        }

        public virtual bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return Repository.Any(predicate);
        }

        public virtual void Add(TEntity entity)
        {
            Repository.Add(entity);
        }

        public virtual void AddRange(List<TEntity> entities)
        {
            Repository.AddRange(entities);
        }

        public virtual void Update(TEntity entity)
        {
            Repository.Update(entity);
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> selector)
        {
            Repository.Delete(selector);
        }

        public virtual void Delete(TEntity entity)
        {
            Repository.Delete(entity);
        }

        public virtual void Delete(int id)
        {
            Repository.Delete<TEntity>(id);
        }

        public virtual void Save()
        {
            Context.SaveChanges();
        }
    }
}
