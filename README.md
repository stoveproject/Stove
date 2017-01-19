

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

* Autofac for Ioc
* AmbientContext Unit Of Work pattern
* Conventional Registration Mechanism with [Autofac.Extras.IocManager](https://github.com/osoykan/Autofac.Extras.IocManager) 
* EventBus for DDD use cases
* EntityFramework
* Generic Repository Pattern, DbContext, Multiple DbContext control in one unit of work, TransactionScope support


## Composition Root
```csharp
IRootResolver resolver = IocBuilder.New
                                   .UseAutofacContainerBuilder()
                                   .UseStove(starterBootstrapperType: typeof(StoveDemoBootstrapper), autoUnitOfWorkInterceptionEnabled: true)
                                   .UseStoveEntityFramework()
                                   .UseStoveDapper()
                                   .UseStoveMapster()
                                   .UseStoveDefaultEventBus()
                                   .UseStoveDbContextEfTransactionStrategy()
                                   .UseStoveTypedConnectionStringResolver()
                                   .UseStoveNLog()
                                   .UseStoveBackgroundJobs()
                                   .UseStoveRedisCaching()
                                   .UseStoveHangfire(configuration =>
                                   {
                                       configuration.GlobalConfiguration
                                                    .UseSqlServerStorage("Default")
                                                    .UseNLogLogProvider();
                                       return configuration;
                                   })
                                   .RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()))
                                   .CreateResolver();

```
