using DevTeam.EntityFrameworkExtensions;
using System.Linq;

namespace DevTeam.GenericRepository
{
    public class SoftDeleteRepository: Repository, ISoftDeleteRepository
    {
        public SoftDeleteRepository(IDbContext context)
            : base(context)
        { }

        public override IQueryable<TEntity> Query<TEntity>()
        {
            return GetQuery<TEntity>();
        }
    }
}
