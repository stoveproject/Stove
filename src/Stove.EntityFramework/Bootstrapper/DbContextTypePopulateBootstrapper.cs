using System;
using System.Linq;

using Stove.Bootstrapping;
using Stove.Bootstrapping.Bootstrappers;
using Stove.EntityFramework.EntityFramework;
using Stove.Reflection.Extensions;

namespace Stove.EntityFramework.Bootstrapper
{
    [DependsOn(
        typeof(StoveKernelBootstrapper)
    )]
    public class DbContextTypePopulateBootstrapper : StoveBootstrapper
    {
        private readonly IDbContextTypeMatcher _dbContextTypeMatcher;

        public DbContextTypePopulateBootstrapper(IDbContextTypeMatcher dbContextTypeMatcher)
        {
            _dbContextTypeMatcher = dbContextTypeMatcher;
        }

        public override void Start()
        {
            Type[] dbContextTypes = typeof(StoveDbContext).AssignedTypes().ToArray();
            _dbContextTypeMatcher.Populate(dbContextTypes);
        }
    }
}
