using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DevTeam.EntityFrameworkExtensions;
using Microsoft.EntityFrameworkCore;

namespace DevTeam.GenericRepository
{
    public class Repository : IRepository
    {
        protected readonly IDbContext Context;

        public Repository(IDbContext context)
        {
            Context = context;
        }

        protected virtual IQueryable<TEntity> GetQuery<TEntity>()
            where TEntity: class
        {
            return Context.Set<TEntity>().AsQueryable();
        }

        public virtual IQueryable<TEntity> Query<TEntity>() 
            where TEntity : class
        {
            var query = GetQuery<TEntity>();

            if (typeof(IDeleted).IsAssignableFrom(typeof(TEntity)))
            {
                query = (IQueryable<TEntity>)((IQueryable<IDeleted>) query).Where(x => !x.IsDeleted);
            }

            return query;
        }

        public virtual IQueryable<TEntity> GetList<TEntity>(Expression<Func<TEntity, bool>>? filter = null)
            where TEntity : class
        {
            var query = Query<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query;
        }

        public virtual IQueryable<TEntity> QueryOne<TEntity, TKey>(TKey id)
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>
        {
            return GetList<TEntity>(x => x.Id.Equals(id));
        }

        public virtual IQueryable<TEntity> QueryOne<TEntity>(int id)
            where TEntity : class, IEntity
        {
            return QueryOne<TEntity, int>(id);
        }

        public virtual TEntity Get<TEntity>(Expression<Func<TEntity, bool>> filter)
            where TEntity : class
        {
            return GetList(filter).FirstOrDefault();
        }

        public virtual Task<TEntity> GetAsync<TEntity>(Expression<Func<TEntity, bool>> filter)
            where TEntity : class
        {
            return GetList(filter).FirstOrDefaultAsync();
        }

        public virtual TEntity Get<TEntity, TKey>(TKey id) 
            where TEntity : class, IEntity<TKey>
            where TKey: IEquatable<TKey>
        {
            return Get<TEntity>(x => x.Id.Equals(id));
        }

        public virtual Task<TEntity> GetAsync<TEntity, TKey>(TKey id)
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>
        {
            return GetAsync<TEntity>(x => x.Id.Equals(id));
        }

        public virtual TEntity Get<TEntity>(int id)
            where TEntity : class, IEntity
        {
            return Get<TEntity, int>(id);
        }

        public virtual Task<TEntity> GetAsync<TEntity>(int id)
            where TEntity : class, IEntity
        {
            return GetAsync<TEntity, int>(id);
        }

        public virtual TProperty GetProperty<TEntity, TProperty>(Expression<Func<TEntity, bool>> filter,
                                                                 Expression<Func<TEntity, TProperty>> selector)
            where TEntity : class
        {
            return GetList(filter).Select(selector).FirstOrDefault();
        }

        public virtual Task<TProperty> GetPropertyAsync<TEntity, TProperty>(Expression<Func<TEntity, bool>> filter,
                                                                 Expression<Func<TEntity, TProperty>> selector)
            where TEntity : class
        {
            return GetList(filter).Select(selector).FirstOrDefaultAsync();
        }

        public virtual TProperty GetProperty<TEntity, TProperty, TKey>(TKey id, Expression<Func<TEntity, TProperty>> selector)
            where TEntity : class, IEntity<TKey>
            where TKey: IEquatable<TKey>
        {
            return GetProperty(x => x.Id.Equals(id), selector);
        }

        public virtual Task<TProperty> GetPropertyAsync<TEntity, TProperty, TKey>(TKey id, Expression<Func<TEntity, TProperty>> selector)
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>
        {
            return GetPropertyAsync(x => x.Id.Equals(id), selector);
        }

        public virtual TProperty GetProperty<TEntity, TProperty>(int id, Expression<Func<TEntity, TProperty>> selector)
            where TEntity : class, IEntity
        {
            return GetProperty<TEntity, TProperty, int>(id, selector);
        }

        public virtual Task<TProperty> GetPropertyAsync<TEntity, TProperty>(int id, Expression<Func<TEntity, TProperty>> selector)
            where TEntity : class, IEntity
        {
            return GetPropertyAsync<TEntity, TProperty, int>(id, selector);
        }

        public virtual bool Any<TEntity>(Expression<Func<TEntity, bool>>? filter = null) 
            where TEntity : class
        {
            var query = Query<TEntity>();
            return filter != null ? query.Any(filter) : query.Any();
        }

        public virtual Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>>? filter = null)
            where TEntity : class
        {
            var query = Query<TEntity>();
            return filter != null ? query.AnyAsync(filter) : query.AnyAsync();
        }

        public virtual TEntity Add<TEntity>(TEntity entity) 
            where TEntity : class
        {
            Context.Set<TEntity>().Add(entity);
            return entity;
        }

        public virtual async Task<TEntity> AddAsync<TEntity>(TEntity entity)
            where TEntity : class
        {
            await Context.Set<TEntity>().AddAsync(entity);
            return entity;
        }

        public virtual List<TEntity> AddRange<TEntity>(List<TEntity> entities)
            where TEntity: class 
        {
            Context.Set<TEntity>().AddRange(entities);
            return entities;
        }

        public virtual async Task<List<TEntity>> AddRangeAsync<TEntity>(List<TEntity> entities)
            where TEntity : class
        {
            await Context.Set<TEntity>().AddRangeAsync(entities);
            return entities;
        }

        public virtual void Update<TEntity>(TEntity entity)
            where TEntity : class
        {
            var item = Context.Entry(entity);

            if (item is IEntity entry)
            {
                var local = Context.Set<TEntity>().Local.Cast<IEntity>().FirstOrDefault(x => x.Id == entry.Id);
                if (local != null)
                {
                    Context.Entry(local).CurrentValues.SetValues(entity);
                    return;
                }
            }

            item.State = EntityState.Modified;
        }

        public virtual void Delete<TEntity>(int id) 
            where TEntity : class, IEntity
        {
            var entity = Get<TEntity>(id);
            Delete(entity);
        }

        public virtual async Task DeleteAsync<TEntity>(int id)
            where TEntity : class, IEntity
        {
            var entity = await GetAsync<TEntity>(id);
            Delete(entity);
        }

        public virtual void Delete<TEntity>(Expression<Func<TEntity, bool>> filter)
            where TEntity : class
        {
            var entity = Get(filter);
            Delete(entity);
        }

        public virtual async Task DeleteAsync<TEntity>(Expression<Func<TEntity, bool>> filter)
            where TEntity : class
        {
            var entity = await GetAsync(filter);
            Delete(entity);
        }

        public virtual void Delete<TEntity>(TEntity entity)
            where TEntity : class
        {
            if (entity is IDeleted deleted)
            {
                deleted.IsDeleted = true;
                Update(entity);
            }
            else
            {
                Context.Set<TEntity>().Remove(entity);
            }
        }

        public virtual int Save()
        {
            return Context.SaveChanges();
        }

        public virtual Task<int> Save(CancellationToken cancellationToken = default)
        {
            return Context.SaveChangesAsync();
        }
    }
}
