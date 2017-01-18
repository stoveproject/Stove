using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Autofac.Extras.IocManager;

using Stove.Dapper.Dapper;
using Stove.EntityFramework.EntityFramework;
using Stove.Reflection.Extensions;

namespace Stove.Dapper
{
    public static class DapperRegistrationExtensions
    {
        public static IIocBuilder UseStoveDapper(this IIocBuilder builder)
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
