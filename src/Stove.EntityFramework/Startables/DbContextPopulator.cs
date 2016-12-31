using System;

using Autofac;
using Autofac.Extras.IocManager;

using FluentAssemblyScanner;

using Stove.EntityFramework.EntityFramework;

namespace Stove.EntityFramework.Startables
{
    public class DbContextPopulator : IStartable, ITransientDependency
    {
        private readonly IDbContextTypeMatcher _dbContextTypeMatcher;

        public DbContextPopulator(IDbContextTypeMatcher dbContextTypeMatcher)
        {
            _dbContextTypeMatcher = dbContextTypeMatcher;
        }

        public void Start()
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
