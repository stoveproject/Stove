using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

using Autofac.Extras.IocManager;

using Stove.Linq;

namespace Stove.EntityFramework.Linq
{
    public class EfAsyncQueryableExecuter : IAsyncQueryableExecuter, ISingletonDependency
    {
        public Task<int> CountAsync<T>(IQueryable<T> queryable)
        {
            return queryable.CountAsync();
        }

        public Task<List<T>> ToListAsync<T>(IQueryable<T> queryable)
        {
            return queryable.ToListAsync();
        }

        public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> queryable)
        {
            return queryable.FirstOrDefaultAsync();
        }
    }
}
