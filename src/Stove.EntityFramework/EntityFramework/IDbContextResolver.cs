using System.Data.Common;

namespace Stove.EntityFramework
{
    public interface IDbContextResolver
    {
        TDbContext Resolve<TDbContext>(string connectionString);

        TDbContext Resolve<TDbContext>(DbConnection existingConnection, bool contextOwnsConnection);
    }
}
