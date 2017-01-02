using System;

using FluentAssemblyScanner;

using Stove.Configuration.Configurers;
using Stove.EntityFramework.EntityFramework;

namespace Stove.EntityFramework.Configurer
{
    public class DbContextTypePopulateConfigurer : StoveConfigurer
    {
        private readonly IDbContextTypeMatcher _dbContextTypeMatcher;

        public DbContextTypePopulateConfigurer(IDbContextTypeMatcher dbContextTypeMatcher)
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
