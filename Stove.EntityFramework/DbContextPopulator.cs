using System;

using Autofac;

using FluentAssemblyScanner;

using Stove.EntityFramework.EntityFramework;

namespace Stove.EntityFramework
{
    public class DbContextPopulator : IStartable
    {
        private readonly IDbContextTypeMatcher _dbContextTypeMatcher;

        public DbContextPopulator(IDbContextTypeMatcher dbContextTypeMatcher)
        {
            _dbContextTypeMatcher = dbContextTypeMatcher;
        }

        public void Start()
        {
            Type[] dbContextTypes = AssemblyScanner.FromThisAssembly()
                                                   .BasedOn<StoveDbContext>()
                                                   .Filter()
                                                   .Classes()
                                                   .NonStatic()
                                                   .Scan().ToArray();

            _dbContextTypeMatcher.Populate(dbContextTypes);
        }
    }
}
