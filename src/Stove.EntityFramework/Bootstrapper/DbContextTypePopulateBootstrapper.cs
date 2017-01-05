using System;

using FluentAssemblyScanner;

using Stove.Bootstrapping;
using Stove.EntityFramework.EntityFramework;

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
            Type[] dbContextTypes = AssemblyScanner.FromAssemblyInDirectory(new AssemblyFilter(string.Empty))
                                                   .BasedOn<StoveDbContext>()
                                                   .Filter()
                                                   .Classes()
                                                   .NonStatic()
                                                   .Scan().ToArray();

            _dbContextTypeMatcher.Populate(dbContextTypes);
        }
    }
}
