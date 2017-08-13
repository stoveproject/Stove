using Microsoft.EntityFrameworkCore;

namespace Stove.EntityFrameworkCore.Configuration
{
    public interface IStoveDbContextConfigurer<TDbContext>
        where TDbContext : DbContext
    {
        void Configure(StoveDbContextConfiguration<TDbContext> configuration);
    }
}