using DevTeam.EntityFrameworkExtensions;
using System.Linq;

namespace DevTeam.GenericRepository
{
    public class ReadOnlyDeleteRepository: ReadOnlyRepository, IReadOnlyDeleteRepository
    {
        public ReadOnlyDeleteRepository(IDbContext context)
            : base(context)
        { }

        public override IQueryable<TEntity> Query<TEntity>()
        {
            return GetQuery<TEntity>();
        }
    }
}
