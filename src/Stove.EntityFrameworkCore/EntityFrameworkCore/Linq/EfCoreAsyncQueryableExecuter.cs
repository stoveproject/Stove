using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Autofac.Extras.IocManager;

using Microsoft.EntityFrameworkCore;

using Stove.Linq;

namespace Stove.EntityFrameworkCore.Linq
{
    public class EfCoreAsyncQueryableExecuter : IAsyncQueryableExecuter, ISingletonDependency
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
