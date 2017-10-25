using System;
using System.Data.Common;
using System.Data.SQLite;
using System.Reflection;

using Autofac.Extras.IocManager;

using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;

using NHibernate;
using NHibernate.Tool.hbm2ddl;

using Stove.NHibernate.Enrichments;
using Stove.NHibernate.Tests.Sessions;
using Stove.TestBase;

namespace Stove.NHibernate.Tests
{
    public abstract class StoveNHibernateTestBase : ApplicationTestBase<StoveNHibernateTestBootstrapper>
    {
        private readonly SQLiteConnection _connection;

        protected StoveNHibernateTestBase(bool autoUowInterceptionEnabled = false) : base(autoUowInterceptionEnabled)
        {
            _connection = new SQLiteConnection("data source=:memory:");
            _connection.Open();

            Building(builder =>
            {
                builder
                    .UseStoveNHibernate(nhConfiguration =>
                    {
                        nhConfiguration.AddFluentConfigurationFor<PrimaryStoveSessionContext>(() =>
                        {
                            return Fluently.Configure()
                                           .Database(SQLiteConfiguration.Standard.InMemory())
                                           .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
                                           .ExposeConfiguration(cfg => new SchemaExport(cfg).Execute(true, true, false, _connection, Console.Out));
                        });

                        nhConfiguration.AddFluentConfigurationFor<SecondaryStoveSessionContext>(() =>
                        {
                            return Fluently.Configure()
                                           .Database(SQLiteConfiguration.Standard.InMemory())
                                           .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
                                           .ExposeConfiguration(cfg => new SchemaExport(cfg).Execute(true, true, false, _connection, Console.Out));
                        });

                        return nhConfiguration;
                    })
                    .UseStoveEventBus()
                    .RegisterServices(r =>
                    {
                        r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
                        r.Register<DbConnection>(ctx => _connection, Lifetime.Singleton);
                    });
            });
        }

        public void UsingSession<TSessionContext>(Action<ISession> action) where TSessionContext : StoveSessionContext
        {
            using (ISession session = The<ISessionFactoryProvider>().GetSessionFactory<TSessionContext>().OpenSessionWithConnection(_connection))
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    action(session);
                    session.Flush();
                    transaction.Commit();
                }
            }
        }

        public T UsingSession<TSessionContext, T>(Func<ISession, T> func) where TSessionContext : StoveSessionContext
        {
            T result;

            using (ISession session = The<ISessionFactoryProvider>().GetSessionFactory<TSessionContext>().OpenSessionWithConnection(_connection))
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    result = func(session);
                    session.Flush();
                    transaction.Commit();
                }
            }

            return result;
        }

        public override void Dispose()
        {
            _connection.Dispose();
            base.Dispose();
        }
    }
}
