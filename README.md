#   Stove [![Build status](https://ci.appveyor.com/api/projects/status/wv4049ey7666vrq4?svg=true)](https://ci.appveyor.com/project/osoykan/stove-jo52k) <img src="https://raw.githubusercontent.com/osoykan/Stove/master/stove.png" alt="alt text" width="100" height="100">  


|Package|Status|
|:------|:-----:|
|Stove|[![NuGet version](https://badge.fury.io/nu/Stove.svg)](https://badge.fury.io/nu/Stove)|
|Stove.Entityframework|[![NuGet version](https://badge.fury.io/nu/Stove.EntityFramework.svg)](https://badge.fury.io/nu/Stove.EntityFramework)|
|Stove.EntityframeworkCore|[![NuGet version](https://badge.fury.io/nu/Stove.EntityFrameworkCore.svg)](https://badge.fury.io/nu/Stove.EntityFrameworkCore)|
|Stove.Hangfire|[![NuGet version](https://badge.fury.io/nu/Stove.Hangfire.svg)](https://badge.fury.io/nu/Stove.Hangfire)|
|Stove.NLog|[![NuGet version](https://badge.fury.io/nu/Stove.NLog.svg)](https://badge.fury.io/nu/Stove.NLog)|
|Stove.Serilog|[![NuGet version](https://badge.fury.io/nu/Stove.Serilog.svg)](https://badge.fury.io/nu/Stove.Serilog)|
|Stove.Mapster|[![NuGet version](https://badge.fury.io/nu/Stove.Mapster.svg)](https://badge.fury.io/nu/Stove.Mapster)|
|Stove.Redis|[![NuGet version](https://badge.fury.io/nu/Stove.Redis.svg)](https://badge.fury.io/nu/Stove.Redis)|
|Stove.Dapper|[![NuGet version](https://badge.fury.io/nu/Stove.Dapper.svg)](https://badge.fury.io/nu/Stove.Dapper)|
|Stove.RabbitMQ|[![NuGet version](https://badge.fury.io/nu/Stove.RabbitMQ.svg)](https://badge.fury.io/nu/Stove.RabbitMQ)|
|Stove.NHibernate|[![NuGet version](https://badge.fury.io/nu/Stove.NHibernate.svg)](https://badge.fury.io/nu/Stove.NHibernate)|
|Stove.RavenDB|[![NuGet version](https://badge.fury.io/nu/Stove.RavenDB.svg)](https://badge.fury.io/nu/Stove.RavenDB)|
|Stove.Couchbase|[![NuGet version](https://badge.fury.io/nu/Stove.Couchbase.svg)](https://badge.fury.io/nu/Stove.Couchbase)|

Stove is an application framework that wraps and abstracts your needs for easy use. Built with strongly adopted dependency injection principles.

## IoC
* Autofac
* Conventional Registration Mechanism with [Autofac.Extras.IocManager](https://github.com/osoykan/Autofac.Extras.IocManager) 

## Use-Case & Transaction approach
* AsyncLocal Unit Of Work pattern

## Adopted principles
* Domain Driven Design
* Persistence agnosticism with **IRepository&lt;T&gt;**
* EventBus for DDD use cases

## Persistence Support
* EntityFramework
* EntityFramework Core
* NHibernate

## Transactional structure
|Tool|Supports Multiple Database/Session Control inside one UOW|
|:------|:-----:|
|EntityFramework| :white_check_mark: |
|EntityFrameworkCore| :white_check_mark: |
|NHibernate| :white_check_mark: |
|Dapper| :white_check_mark: |

### Notes

* To work with Dapper, you must use EF & EF Core or NHibernate as primary ORM choice. Dapper shares their transactions to execute its sqls inside of one **Unit Of Work** scope.
* **Dapper-EntityFramework**, **Dapper-NHibernate** or **Dapper-EntityFrameworkCore** works under same transaction and unit of work scope, if any exception appears in domain **whole transaction will be rollback**, including Dapper's insert/deletes or EF's or NH's.
* Stove.Dapper supports **Dynamic Filters** to filter automatically and default ISoftDelete or other user defined filters.

* NHibernate **supports multiple database** in one UOW scope.

### Nhibernate Multiple Database

Let's assume that we have two entities which are inherited from **Entity&lt;int&gt;**. If we want to work with multiple database with NHibernate we have to tell to Stove that which entities belong to which database or session. To achive that Stove has to know your database distinction. Basically `StoveSessionContext` does that.

#### StoveSessionContext

```csharp
public class PrimaryStoveSessionContext : StoveSessionContext
{
    public IStoveSessionSet<Product> Products { get; set; }
}
```

```csharp
public class SecondaryStoveSessionContext : StoveSessionContext
{
    public IStoveSessionSet<Category> Categories { get; set; }
}
```

Registration:

```csharp
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
```

After this definition, we have to define which SessionContext uses which connection string for connect to database and creating session factory.
```csharp
 [DependsOn(
     typeof(StoveNHibernateBootstrapper)
 )]
 public class StoveNHibernateTestBootstrapper : StoveBootstrapper
 {
     public override void PreStart()
     {
         StoveConfiguration.DefaultNameOrConnectionString = "data source=:memory:";
         StoveConfiguration.TypedConnectionStrings.Add(typeof(PrimaryStoveSessionContext),StoveConfiguration.DefaultNameOrConnectionString);
         StoveConfiguration.TypedConnectionStrings.Add(typeof(SecondaryStoveSessionContext),StoveConfiguration.DefaultNameOrConnectionString);

         DapperExtensions.DapperExtensions.SqlDialect = new SqliteDialect();
         DapperExtensions.DapperExtensions.SetMappingAssemblies(new List<Assembly> { Assembly.GetExecutingAssembly() });
     }
 }
```
As you see connection strings are same but they are inside of different **StoveSessionContexts**. If these entities are inside of same database but you want to treat as different bounded contexts to them, you can choose this kind of approach for your sessions. Otherwise entities can sit together in one **SessionContext**.

Usage is always same and persistence agnostic:
```csharp
 using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
 {
     The<IDapperRepository<Product>>().GetAll().Count().ShouldBe(1);
     The<IRepository<Product>>().GetAll().Count().ShouldBe(1);

     uow.Complete();
 }
```

## Document Databases
* RavenDB -> **IRepository&lt;T&gt;**
* Couchbase -> **IRepository&lt;T&gt;**

## Queue Mechanism

* RabbitMQ support

## Background Jobs

* HangFire support

## Caching

* Redis support

## Others

* A lot of extensions
* Strictly **SOLID**

## Composition Root
```csharp
IRootResolver resolver = IocBuilder.New
                                   .UseAutofacContainerBuilder()
                                   .UseStove<StoveDemoBootstrapper>(autoUnitOfWorkInterceptionEnabled: true)
                                   .UseStoveEntityFramework()
                                   .UseStoveDapper()
                                   .UseStoveMapster()
                                   .UseStoveDefaultEventBus()
                                   .UseStoveDbContextEfTransactionStrategy()
                                   .UseStoveTypedConnectionStringResolver()
                                   .UseStoveNLog()
                                   .UseStoveBackgroundJobs()
                                   .UseStoveRedisCaching()
                                   .UseStoveRabbitMQ(configuration =>
                                   {
                                       configuration.HostAddress = "rabbitmq://localhost/";
                                       configuration.Username = "admin";
                                       configuration.Password = "admin";
                                       configuration.QueueName = "Default";
                                       return configuration;
                                   })
                                   .UseStoveHangfire(configuration =>
                                   {
                                       configuration.GlobalConfiguration
                                                    .UseSqlServerStorage("Default")
                                                    .UseNLogLogProvider();
                                       return configuration;
                                   })
                                   .RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()))
                                   .CreateResolver();

var someDomainService = resolver.Resolve<SomeDomainService>();
someDomainService.DoSomeStuff();
 
```
