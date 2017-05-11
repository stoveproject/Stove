

#   Stove [![Build status](https://ci.appveyor.com/api/projects/status/wv4049ey7666vrq4?svg=true)](https://ci.appveyor.com/project/osoykan/stove-jo52k) <img src="https://raw.githubusercontent.com/osoykan/Stove/master/stove.png" alt="alt text" width="100" height="100">  


 
 


|Package|Status|
|:------|:-----:|
|Stove|[![NuGet version](https://badge.fury.io/nu/Stove.svg)](https://badge.fury.io/nu/Stove)|
|Stove.Entityframework|[![NuGet version](https://badge.fury.io/nu/Stove.EntityFramework.svg)](https://badge.fury.io/nu/Stove.EntityFramework)|
|Stove.Hangfire|[![NuGet version](https://badge.fury.io/nu/Stove.Hangfire.svg)](https://badge.fury.io/nu/Stove.Hangfire)|
|Stove.NLog|[![NuGet version](https://badge.fury.io/nu/Stove.NLog.svg)](https://badge.fury.io/nu/Stove.NLog)|
|Stove.Mapster|[![NuGet version](https://badge.fury.io/nu/Stove.Mapster.svg)](https://badge.fury.io/nu/Stove.Mapster)|
|Stove.Redis|[![NuGet version](https://badge.fury.io/nu/Stove.Redis.svg)](https://badge.fury.io/nu/Stove.Redis)|
|Stove.Dapper|[![NuGet version](https://badge.fury.io/nu/Stove.Dapper.svg)](https://badge.fury.io/nu/Stove.Dapper)|
|Stove.RabbitMQ|[![NuGet version](https://badge.fury.io/nu/Stove.RabbitMQ.svg)](https://badge.fury.io/nu/Stove.RabbitMQ)|
|Stove.NHibernate|[![NuGet version](https://badge.fury.io/nu/Stove.NHibernate.svg)](https://badge.fury.io/nu/Stove.NHibernate)|
|Stove.RavenDB|[![NuGet version](https://badge.fury.io/nu/Stove.RavenDB.svg)](https://badge.fury.io/nu/Stove.RavenDB)|

* Autofac for Ioc
* AmbientContext Unit Of Work pattern
* Conventional Registration Mechanism with [Autofac.Extras.IocManager](https://github.com/osoykan/Autofac.Extras.IocManager) 
* EventBus for DDD use cases
* EntityFramework
* NHibernate
* Generic Repository Pattern, DbContext, Multiple DbContext control in one unit of work, TransactionScope support
* Dapper and EF both can use in one application.
* Dapper and EF have their own repositories. `IDapperRepository<Product>`, `IRepository<Product>`
* Dapper-EntityFramework works under same transaction and unit of work scope, if any exception appears in domain whole transaction will be rollback, including Dapper's insert/deletes and EF's.
* Stove.Dapper supports *Dynamic Filters* to filter automatically and default ISoftDelete or other user defined filters.
* Stove.Dapper also works with NHibernate under same transaction. Like EF transaction sharing.
* RabbitMQ support
* HangFire support
* Redis support
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

## It will be documented in detail!
