using System.Collections.Generic;
using System.Reflection;

using Autofac.Extras.IocManager;

using JetBrains.Annotations;

using Stove.Dapper;
using Stove.Extensions;
using Stove.Orm;

namespace Stove
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
            builder.RegisterServices(r => r.UseBuilder(cb =>
            {
                if (!cb.Properties.ContainsKey(StoveConsts.OrmRegistrarContextKey))
                {
                    throw new StoveInitializationException("Dapper registration should be after EntityFramework or NHibernate registration" +
                                                           " use StoveEntityFramework() or StoveNHibernate() registration methods before use StoveDapper().");
                }

                var ormRegistrars = cb.Properties[StoveConsts.OrmRegistrarContextKey].As<IList<IAdditionalOrmRegistrar>>();

                ormRegistrars.ForEach(registrar =>
                {
                    switch (registrar.OrmContextKey)
                    {
                        case StoveOrms.EntityFramework:
                            registrar.RegisterRepositories(EfBasedDapperAutoRepositoryTypes.Default);
                            break;
                        case StoveOrms.NHibernate:
                            registrar.RegisterRepositories(NhBasedDapperAutoRepositoryTypes.Default);
                            break;
                    }
                });
            }));
        }
    }
}
