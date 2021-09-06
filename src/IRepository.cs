using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DevTeam.EntityFrameworkExtensions;

namespace DevTeam.GenericRepository
{
    public interface IReadOnlyRepository: IRepository 
    { }

    public interface ISoftDeleteRepository: IRepository
    { }

    public interface IReadOnlyDeleteRepository: IRepository
    { }

    public interface IRepository
    {
        IQueryable<TEntity> Query<TEntity>()
            where TEntity : class;
        IQueryable<TEntity> GetList<TEntity>(Expression<Func<TEntity, bool>>? filter = null)
            where TEntity : class;
        IQueryable<TEntity> QueryOne<TEntity, TKey>(TKey id)
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>;
        IQueryable<TEntity> QueryOne<TEntity>(int id)
            where TEntity : class, IEntity;
        TEntity Get<TEntity>(Expression<Func<TEntity, bool>> filter)
            where TEntity : class;
        Task<TEntity> GetAsync<TEntity>(Expression<Func<TEntity, bool>> filter)
            where TEntity : class;
        TEntity Get<TEntity, TKey>(TKey id)
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>;
        Task<TEntity> GetAsync<TEntity, TKey>(TKey id)
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>;
        TEntity Get<TEntity>(int id)
            where TEntity : class, IEntity;
        Task<TEntity> GetAsync<TEntity>(int id)
            where TEntity : class, IEntity;
        TProperty GetProperty<TEntity, TProperty>(Expression<Func<TEntity, bool>> filter,
                                                  Expression<Func<TEntity, TProperty>> selector)
            where TEntity : class;
        Task<TProperty> GetPropertyAsync<TEntity, TProperty>(Expression<Func<TEntity, bool>> filter,
                                                             Expression<Func<TEntity, TProperty>> selector)
            where TEntity : class;
        TProperty GetProperty<TEntity, TProperty, TKey>(TKey id, Expression<Func<TEntity, TProperty>> selector)
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>;
        Task<TProperty> GetPropertyAsync<TEntity, TProperty, TKey>(TKey id, Expression<Func<TEntity, TProperty>> selector)
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>;
        TProperty GetProperty<TEntity, TProperty>(int id, Expression<Func<TEntity, TProperty>> selector)
            where TEntity : class, IEntity;
        Task<TProperty> GetPropertyAsync<TEntity, TProperty>(int id, Expression<Func<TEntity, TProperty>> selector)
            where TEntity : class, IEntity;
        bool Any<TEntity>(Expression<Func<TEntity, bool>>? filter = null)
            where TEntity : class;
        Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>>? filter = null)
            where TEntity : class;
        TEntity Add<TEntity>(TEntity entity)
            where TEntity : class;
        Task<TEntity> AddAsync<TEntity>(TEntity entity)
            where TEntity : class;
        List<TEntity> AddRange<TEntity>(List<TEntity> entities)
            where TEntity : class;
        Task<List<TEntity>> AddRangeAsync<TEntity>(List<TEntity> entities)
            where TEntity : class;
        void Update<TEntity>(TEntity entity)
            where TEntity : class;
        void Delete<TEntity>(int id)
            where TEntity : class, IEntity;
        Task DeleteAsync<TEntity>(int id)
            where TEntity : class, IEntity;
        void Delete<TEntity>(Expression<Func<TEntity, bool>> filter)
            where TEntity : class;
        Task DeleteAsync<TEntity>(Expression<Func<TEntity, bool>> filter)
            where TEntity : class;
        void Delete<TEntity>(TEntity entity)
            where TEntity : class;
        int Save();
        Task<int> Save(CancellationToken cancellationToken = default);
    }
}
