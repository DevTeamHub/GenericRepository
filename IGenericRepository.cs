using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DevTeam.EntityFrameworkExtensions.DbContext;

namespace DevTeam.GenericRepository
{
    public interface IGenericRepository
    {
        IQueryable<TEntity> Query<TEntity>() 
            where TEntity : class;

        TEntity Get<TEntity>(int id) 
            where TEntity : class, IEntity;

        TEntity Get<TEntity>(Expression<Func<TEntity, bool>> predicate = null) 
            where TEntity : class;

        IQueryable<TEntity> GetQuery<TEntity>(int id)
            where TEntity : class, IEntity;

        IQueryable<TEntity> GetList<TEntity>(Expression<Func<TEntity, bool>> predicate = null)
            where TEntity : class;

        TProperty GetProperty<TEntity, TProperty>(int id, Expression<Func<TEntity, TProperty>> selector)
            where TEntity : class, IEntity;

        TProperty GetProperty<TEntity, TProperty>(Expression<Func<TEntity, bool>> predicate,
                                                         Expression<Func<TEntity, TProperty>> selector)
            where TEntity : class;

        bool Any<TEntity>(Expression<Func<TEntity, bool>> predicate) 
            where TEntity : class;

        void Add<TEntity>(TEntity entity) 
            where TEntity : class;

        void AddRange<TEntity>(List<TEntity> entities)
            where TEntity : class;

        void Update<TEntity>(TEntity model) 
            where TEntity : class;

        void Delete<TEntity>(int id) 
            where TEntity : class, IEntity;

        void Delete<TEntity>(Expression<Func<TEntity, bool>> predicate) 
            where TEntity : class;

        void Delete<TEntity>(TEntity entity) 
            where TEntity : class;

        void Save();
    }
}
