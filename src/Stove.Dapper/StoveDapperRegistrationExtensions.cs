using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Autofac.Extras.IocManager;

using JetBrains.Annotations;

using Stove.Dapper.Dapper;
using Stove.EntityFramework.EntityFramework;
using Stove.Reflection.Extensions;

namespace Stove.Dapper
{
    public static class StoveDapperRegistrationExtensions
    {
        /// <summary>
        ///     Dapper Integration for Stove, registers and arrange all Dapper structure to Ioc Container.
        ///     It should be called in composition root to use correctly.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        [NotNull]
        public static IIocBuilder UseStoveDapper([NotNull] this IIocBuilder builder)
        {
            builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()));

            AutoRegisterRepositories(builder);

            return builder;
        }

        private static void AutoRegisterRepositories(IIocBuilder builder)
        {
            List<Type> dbContextTypes = typeof(StoveDbContext).AssignedTypes().ToList();
            dbContextTypes.ForEach(type => DapperRepositoryRegistrar.RegisterRepositories(type, builder));
        }
    }
}
