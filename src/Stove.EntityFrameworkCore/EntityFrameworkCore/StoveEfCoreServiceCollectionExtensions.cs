using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Stove.EntityFrameworkCore.Configuration;

namespace Stove.EntityFrameworkCore
{
    public static class StoveEfCoreServiceCollectionExtensions
    {
        public static void AddStoveDbContext<TDbContext>(
            this IServiceCollection services,
            Action<StoveDbContextConfiguration<TDbContext>> action)
            where TDbContext : DbContext
        {
            services.AddSingleton(
                typeof(IStoveDbContextConfigurer<TDbContext>),
                new StoveDbContextConfigurerAction<TDbContext>(action)
            );
        }
    }
}
