using System;
using System.Data.Entity;

using NSubstitute;

using Shouldly;

using Stove.Domain.Entities;
using Stove.Domain.Repositories;
using Stove.EntityFramework.EntityFramework;
using Stove.EntityFramework.EntityFramework.Repositories;
using Stove.TestBase;

using Xunit;

namespace Stove.EntityFramework.Tests
{
    public class EntityFrameworkGenericRepositoryRegistrar_Tests : TestBaseWithLocalIocResolver
    {
        public EntityFrameworkGenericRepositoryRegistrar_Tests()
        {
            var fakeBaseDbContextProvider = Substitute.For<IDbContextProvider<MyBaseDbContext>>();
            var fakeMainDbContextProvider = Substitute.For<IDbContextProvider<MyMainDbContext>>();
            var fakeModuleDbContextProvider = Substitute.For<IDbContextProvider<MyModuleDbContext>>();

            Building(builder =>
            {
                builder.RegisterServices(r => r.Register(context => fakeBaseDbContextProvider));
                builder.RegisterServices(r => r.Register(context => fakeMainDbContextProvider));
                builder.RegisterServices(r => r.Register(context => fakeModuleDbContextProvider));

                EfRepositoryRegistrar.RegisterRepositories(typeof(MyModuleDbContext), builder);
                EfRepositoryRegistrar.RegisterRepositories(typeof(MyMainDbContext), builder);
            });
        }

        [Fact]
        public void Should_Resolve_Generic_Repositories()
        {
            //Entity 1 (with default PK)
            var entity1Repository = LocalResolver.Resolve<IRepository<MyEntity1>>();
            entity1Repository.ShouldNotBe(null);
            (entity1Repository is EfRepositoryBase<MyBaseDbContext, MyEntity1>).ShouldBe(true);

            //Entity 1 (with specified PK)

            var entity1RepositoryWithPk = LocalResolver.Resolve<IRepository<MyEntity1>>();
            entity1RepositoryWithPk.ShouldNotBe(null);
            (entity1RepositoryWithPk is EfRepositoryBase<MyBaseDbContext, MyEntity1, int>).ShouldBe(true);

            //Entity 1 (with specified Repository forIMyModuleRepository )
            var entity1RepositoryWithModuleInterface = LocalResolver.Resolve<IMyModuleRepository<MyEntity1>>();
            entity1RepositoryWithModuleInterface.ShouldNotBe(null);
            (entity1RepositoryWithModuleInterface is MyModuleRepositoryBase<MyEntity1>).ShouldBe(true);
            (entity1RepositoryWithModuleInterface is EfRepositoryBase<MyModuleDbContext, MyEntity1, int>).ShouldBe(true);

            //Entity 1 (with specified Repository forIMyModuleRepository )

            var entity1RepositoryWithModuleInterfaceWithPk = LocalResolver.Resolve<IMyModuleRepository<MyEntity1>>();
            entity1RepositoryWithModuleInterfaceWithPk.ShouldNotBe(null);
            (entity1RepositoryWithModuleInterfaceWithPk is MyModuleRepositoryBase<MyEntity1, int>).ShouldBe(true);
            (entity1RepositoryWithModuleInterfaceWithPk is EfRepositoryBase<MyModuleDbContext, MyEntity1, int>).ShouldBe(true);

            //Entity 2
            var entity2Repository = LocalResolver.Resolve<IRepository<MyEntity2, long>>();
            (entity2Repository is EfRepositoryBase<MyMainDbContext, MyEntity2, long>).ShouldBe(true);
            entity2Repository.ShouldNotBe(null);

            //Entity 3
            var entity3Repository = LocalResolver.Resolve<IMyModuleRepository<MyEntity3, Guid>>();
            (entity3Repository is EfRepositoryBase<MyModuleDbContext, MyEntity3, Guid>).ShouldBe(true);
            entity3Repository.ShouldNotBe(null);
        }

        public class MyMainDbContext : MyBaseDbContext
        {
            public virtual DbSet<MyEntity2> MyEntities2 { get; set; }
        }

        [AutoRepositoryTypes(
            typeof(IMyModuleRepository<>),
            typeof(IMyModuleRepository<,>),
            typeof(MyModuleRepositoryBase<>),
            typeof(MyModuleRepositoryBase<,>)
        )]
        public class MyModuleDbContext : MyBaseDbContext
        {
            public virtual DbSet<MyEntity3> MyEntities3 { get; set; }
        }

        public abstract class MyBaseDbContext : StoveDbContext
        {
            public virtual IDbSet<MyEntity1> MyEntities1 { get; set; }
        }

        public class MyEntity1 : Entity
        {
        }

        public class MyEntity2 : Entity<long>
        {
        }

        public class MyEntity3 : Entity<Guid>
        {
        }

        public interface IMyModuleRepository<TEntity> : IRepository<TEntity>
            where TEntity : class, IEntity<int>
        {
        }

        public interface IMyModuleRepository<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>
            where TEntity : class, IEntity<TPrimaryKey>
        {
        }

        public class MyModuleRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<MyModuleDbContext, TEntity, TPrimaryKey>, IMyModuleRepository<TEntity, TPrimaryKey>
            where TEntity : class, IEntity<TPrimaryKey>
        {
            public MyModuleRepositoryBase(IDbContextProvider<MyModuleDbContext> dbContextProvider)
                : base(dbContextProvider)
            {
            }
        }

        public class MyModuleRepositoryBase<TEntity> : MyModuleRepositoryBase<TEntity, int>, IMyModuleRepository<TEntity>
            where TEntity : class, IEntity<int>
        {
            public MyModuleRepositoryBase(IDbContextProvider<MyModuleDbContext> dbContextProvider)
                : base(dbContextProvider)
            {
            }
        }
    }
}
