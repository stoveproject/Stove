using System;

using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.IocManager;

using Hangfire;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using NLog.Extensions.Logging;
using NLog.Web;

using Stove.Demo.WebApi.Core.Domain.DbContexts;
using Stove.EntityFramework;
using Stove.Redis.Configurations;
using Stove.Reflection.Extensions;

using Swashbuckle.AspNetCore.Swagger;

using ILogger = Stove.Log.ILogger;

namespace Stove.Demo.WebApi.Core
{
	public class Startup
	{
		private const string RootLifetimeTag = "root";

		public Startup(IHostingEnvironment env)
		{
			IConfigurationBuilder builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", false, true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
				.AddEnvironmentVariables();

			env.ConfigureNLog("NLog.config");

			Configuration = builder.Build();
		}

		public IConfigurationRoot Configuration { get; }

		public IRootResolver RootResolver { get; private set; }

		public ILifetimeScope RootScope { get; private set; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			services.AddAutofac();

			// Add framework services.
			services.AddMvc().AddControllersAsServices();

			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info { Title = "Stove Web Api", Version = "v1" }); });

			IocBuilder builder = IocBuilder.New;

			RootResolver = builder.UseStove<StoveWebApiCoreBootstrapper>()
			                      .UseStoveEntityFrameworkCore(configuration => configuration)
			                      .AddStoveDbContext<AnimalDbContext>(contextConfiguration =>
			                      {
				                      if (contextConfiguration.ExistingConnection != null)
				                      {
					                      contextConfiguration.DbContextOptions.UseSqlServer(contextConfiguration.ExistingConnection);
				                      }
				                      else
				                      {
					                      contextConfiguration.DbContextOptions.UseSqlServer(Configuration.GetConnectionString("Default"));
				                      }
			                      })
			                      .AddStoveDbContext<PersonDbContext>(contextConfiguration =>
			                      {
				                      if (contextConfiguration.ExistingConnection != null)
				                      {
					                      contextConfiguration.DbContextOptions.UseSqlServer(contextConfiguration.ExistingConnection);
				                      }
				                      else
				                      {
					                      contextConfiguration.DbContextOptions.UseSqlServer(Configuration.GetConnectionString("Default"));
				                      }
			                      })
			                      .UseStoveDapper()
			                      .UseStoveTypedConnectionStringResolver()
			                      .UseStoveEventBus()
			                      .UseStoveBackgroundJobs()
			                      .UseStoveMapster()
			                      .UseStoveHangfire(configuration =>
			                      {
				                      configuration.GlobalConfiguration
				                                   .UseSqlServerStorage(Configuration.GetConnectionString("Default"))
				                                   .UseNLogLogProvider();

				                      return configuration;
			                      })
			                      .UseStoveNLog()
			                      .UseStoveRabbitMQ(configuration =>
			                      {
				                      configuration.HostAddress = "rabbitmq://localhost/";
				                      configuration.Username = "admin";
				                      configuration.Password = "admin";
				                      configuration.QueueName = "NetCore2";
				                      return configuration;
			                      })
			                      .UseStoveRedisCaching(configuration =>
			                      {
				                      configuration.ConfigurationOptions
				                                   .AddEndpoint("127.0.0.1")
				                                   .SetDefaultDatabase(0)
				                                   .SetConnectionTimeOut(TimeSpan.FromMinutes(5));

				                      return configuration;
			                      })
			                      .RegisterServices(r =>
			                      {
				                      r.RegisterAssemblyByConvention(typeof(Startup).GetAssembly());
				                      r.BeforeRegistrationCompleted += (sender, args) => { args.ContainerBuilder.Populate(services); };
			                      })
			                      .CreateResolver();

			RootResolver.Resolve<ILogger>().Error("anan");
			return new AutofacServiceProvider(RootResolver.Container);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime applicationLifetime)
		{
			loggerFactory.AddNLog();

			app.AddNLogWeb();

			app.UseMvc();

			app.UseSwagger();

			app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Stove WebApi Core"); });

			applicationLifetime.ApplicationStopped.Register(() =>
			{
				RootScope.Dispose();
				RootResolver.Dispose();
			});
		}
	}
}
