using System.Collections.Generic;
using System.Reflection;

using Autofac;
using Autofac.Extras.IocManager;

using JetBrains.Annotations;

using Stove.Extensions;
using Stove.Orm;

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
            AutoRegisterRepositories(builder);

            return builder.RegisterServices(r => r.RegisterAssemblyByConvention(typeof(StoveDapperRegistrationExtensions).GetTypeInfo().Assembly));
        }

        private static void AutoRegisterRepositories(IIocBuilder builder)
        {
            builder.RegisterServices(r => r.BeforeRegistrationCompleted += (sender, args) => LastChanceRegistration(args.ContainerBuilder));
        }

        private static void LastChanceRegistration(ContainerBuilder cb)
        {
            if (!cb.Properties.ContainsKey(StoveConsts.OrmRegistrarContextKey))
            {
                throw new StoveInitializationException("Dapper registration should be after EntityFramework or NHibernate registration" +
                                                       " use StoveEntityFramework() or StoveNHibernate() registration methods before use StoveDapper().");
            }

            var ormRegistrars = cb.Properties[StoveConsts.OrmRegistrarContextKey].As<IList<ISecondaryOrmRegistrar>>();
            ormRegistrars.ForEach(registrar =>
            {
                switch (registrar.OrmContextKey)
                {
                    case StoveConsts.Orms.EntityFramework:
                        registrar.RegisterRepositories(EfBasedDapperAutoRepositoryTypes.Default);
                        break;
	                case StoveConsts.Orms.EntityFrameworkCore:
		                registrar.RegisterRepositories(EfBasedDapperAutoRepositoryTypes.Default);
		                break;
					case StoveConsts.Orms.NHibernate:
                        registrar.RegisterRepositories(NhBasedDapperAutoRepositoryTypes.Default);
                        break;
                }
            });
        }
    }
}
