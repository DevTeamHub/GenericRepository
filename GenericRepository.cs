using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using DevTeam.EntityFrameworkExtensions.DbContext;
using DevTeam.EntityFrameworkExtensions.Helpers;

namespace DevTeam.GenericRepository
{
    public class GenericRepository : IGenericRepository
    {
        protected readonly IDbContext Context;

        public GenericRepository(IDbContext context)
        {
            Context = context;
        }

        public virtual IQueryable<TEntity> Query<TEntity>() 
            where TEntity : class
        {
            var query = Context.Set<TEntity>().AsQueryable();
            return typeof(IDeleted).IsAssignableFrom(typeof(TEntity))
                ? query.Where(ExpressionHelper.GetInvertedBoolExpression<TEntity>("IsDeleted"))
                : query;
        }

        public virtual TEntity Get<TEntity>(int id) 
            where TEntity : class, IEntity
        {
            var pkExpression = ExpressionHelper.PkFilterExpression<TEntity>(id);
            var entity = Get(pkExpression);
            return entity;
        }

        public virtual TEntity Get<TEntity>(Expression<Func<TEntity, bool>> predicate = null) 
            where TEntity : class
        {
            var result = GetList(predicate).FirstOrDefault();
            return result;
        }

        public virtual IQueryable<TEntity> GetQuery<TEntity>(int id)
            where TEntity : class, IEntity
        {
            var pkExpression = ExpressionHelper.PkFilterExpression<TEntity>(id);
            return GetList(pkExpression);
        }

        public virtual IQueryable<TEntity> GetList<TEntity>(Expression<Func<TEntity, bool>> predicate = null) 
            where TEntity : class
        {
            return predicate == null ? Query<TEntity>() : Query<TEntity>().Where(predicate);
        }

        public virtual TProperty GetProperty<TEntity, TProperty>(int id, Expression<Func<TEntity, TProperty>> selector)
            where TEntity : class, IEntity
        {
            var pkExpression = ExpressionHelper.PkFilterExpression<TEntity>(id);
            return GetProperty(pkExpression, selector);
        }

        public virtual TProperty GetProperty<TEntity, TProperty>(Expression<Func<TEntity, bool>> predicate,
                                                         Expression<Func<TEntity, TProperty>> selector)
            where TEntity : class
        {
            return GetList(predicate).Select(selector).FirstOrDefault();
        }

        public virtual bool Any<TEntity>(Expression<Func<TEntity, bool>> predicate) 
            where TEntity : class
        {
            return predicate != null ? Query<TEntity>().Any(predicate) : Query<TEntity>().Any();
        }

        public virtual void Add<TEntity>(TEntity entity) 
            where TEntity : class
        {
            Context.Set<TEntity>().Add(entity);
        }

        public virtual void AddRange<TEntity>(List<TEntity> entities)
            where TEntity: class 
        {
            Context.Set<TEntity>().AddRange(entities);
        }

        public virtual void Update<TEntity>(TEntity model) 
            where TEntity : class
        {
            var entity = Context.Entry(model);
            if (model is IEntity)
            {
                var id = (model as IEntity).PrimaryKey;
                var local = Context.Set<TEntity>().Local.Cast<IEntity>().FirstOrDefault(x => x.PrimaryKey == id);
                if (local != null)
                {
                    Context.Entry(local).CurrentValues.SetValues(entity);
                    return;
                }
            }

            entity.State = EntityState.Modified;
        }

        public virtual void Delete<TEntity>(int id) 
            where TEntity : class, IEntity
        {
            var entity = Get<TEntity>(id);
            Delete(entity);
        }

        public virtual void Delete<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            var entity = Get(predicate);
            Delete(entity);
        }

        public virtual void Delete<TEntity>(TEntity entity)
            where TEntity : class
        {
            var deleted = entity as IDeleted;
            if (deleted != null)
            {
                deleted.IsDeleted = true;
                Update(entity);
            }
            else
            {
                Context.Set<TEntity>().Remove(entity);
            }
        }

        public virtual void Save()
        {
            Context.SaveChanges();
        }
    }
}
