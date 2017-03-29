using System;
using System.Data;
using System.Data.SQLite;
using System.Reflection;

using Autofac.Extras.IocManager;

using FluentNHibernate.Cfg.Db;

using NHibernate;
using NHibernate.Tool.hbm2ddl;

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
                        nhConfiguration.FluentConfiguration
                                       .Database(SQLiteConfiguration.Standard.InMemory())
                                       .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
                                       .ExposeConfiguration(cfg => new SchemaExport(cfg).Execute(true, true, false, The<IDbConnection>(), Console.Out));

                        return nhConfiguration;
                    })
                    .UseStoveDefaultConnectionStringResolver()
                    .UseStoveEventBus()
                    .RegisterServices(r =>
                    {
                        r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
                        r.Register<IDbConnection>(ctx => _connection, Lifetime.Singleton);
                    });
            });
        }

        public void UsingSession(Action<ISession> action)
        {
            using (ISession session = The<ISessionFactory>().OpenSession(_connection))
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    action(session);
                    session.Flush();
                    transaction.Commit();
                }
            }
        }

        public T UsingSession<T>(Func<ISession, T> func)
        {
            T result;

            using (ISession session = The<ISessionFactory>().OpenSession(_connection))
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
