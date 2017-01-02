using System;

using FluentAssemblyScanner;

using Stove.Configuration;
using Stove.Configuration.Configurers;
using Stove.EntityFramework.EntityFramework;

namespace Stove.EntityFramework.Configurer
{
    public class DbContextPopulator : StoveConfigurer
    {
        private readonly IDbContextTypeMatcher _dbContextTypeMatcher;

        public DbContextPopulator(IStoveStartupConfiguration configuration, IDbContextTypeMatcher dbContextTypeMatcher) : base(configuration)
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
