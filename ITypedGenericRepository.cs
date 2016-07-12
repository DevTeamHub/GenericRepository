using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DevTeam.EntityFrameworkExtensions.DbContext;

namespace DevTeam.GenericRepository
{
    public interface IGenericRepository<TEntity>
        where TEntity: class, IEntity, new()
    {
        IQueryable<TEntity> Query();

        IQueryable<TType> Set<TType>()
            where TType : class, IEntity, new();

        TEntity Get(int id);

        TEntity Get(Expression<Func<TEntity, bool>> selector);

        IQueryable<TEntity> GetList(Expression<Func<TEntity, bool>> selector = null);

        TProperty GetProperty<TProperty>(int id, Expression<Func<TEntity, TProperty>> property);

        TProperty GetProperty<TProperty>(Expression<Func<TEntity, bool>> entity,
                                         Expression<Func<TEntity, TProperty>> property);

        bool Any(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);

        void AddRange(List<TEntity> entities);

        void Update(TEntity entity);

        void Delete(Expression<Func<TEntity, bool>> selector);

        void Delete(TEntity entity);

        void Delete(int id);

        void Save();
    }
}
