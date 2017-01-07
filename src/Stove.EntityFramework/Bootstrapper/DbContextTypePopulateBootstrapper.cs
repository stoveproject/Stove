using System;
using System.Linq;

using Stove.Bootstrapping;
using Stove.EntityFramework.EntityFramework;
using Stove.Reflection.Extensions;

namespace Stove.EntityFramework.Bootstrapper
{
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
