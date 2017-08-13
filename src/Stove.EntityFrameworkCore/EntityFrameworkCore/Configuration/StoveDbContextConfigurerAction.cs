using System;

using Microsoft.EntityFrameworkCore;

namespace Stove.EntityFrameworkCore.Configuration
{
    public class StoveDbContextConfigurerAction<TDbContext> : IStoveDbContextConfigurer<TDbContext>
        where TDbContext : DbContext
    {
        public Action<StoveDbContextConfiguration<TDbContext>> Action { get; set; }

        public StoveDbContextConfigurerAction(Action<StoveDbContextConfiguration<TDbContext>> action)
        {
            Action = action;
        }

        public void Configure(StoveDbContextConfiguration<TDbContext> configuration)
        {
            Action(configuration);
        }
    }
}