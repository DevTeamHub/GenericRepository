using DevTeam.EntityFrameworkExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DevTeam.GenericRepository
{
    public class ReadOnlyRepository: Repository, IReadOnlyRepository
    {
        public ReadOnlyRepository(IDbContext context)
            :base(context)
        { }

        protected override IQueryable<TEntity> GetQuery<TEntity>()
        {
            return Context.Set<TEntity>().AsNoTracking();
        }
    }
}
