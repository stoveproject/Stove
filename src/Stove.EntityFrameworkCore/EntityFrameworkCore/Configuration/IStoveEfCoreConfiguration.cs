using System;

using Microsoft.EntityFrameworkCore;

namespace Stove.EntityFrameworkCore.Configuration
{
    public interface IStoveEfCoreConfiguration
    {
        void AddDbContext<TDbContext>(Action<StoveDbContextConfiguration<TDbContext>> action)
            where TDbContext : DbContext;
    }
}
