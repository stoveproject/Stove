using System;
using System.Reflection;

using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.IocManager;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Stove.WebApi.Commands;

namespace Stove.Demo.WebApiCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddControllersAsServices();

            IRootResolver resolver = IocBuilder.New
                                               .UseAutofacContainerBuilder()
                                               .UseStoveWithNullables<StoveWebApiCoreBootstrapper>()
                                               .RegisterServices(r =>
                                               {
                                                   r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
                                                   r.BeforeRegistrationCompleted += (sender, args) =>
                                                   {
                                                       args.ContainerBuilder.Populate(services);
                                                   };
                                               }).CreateResolver();

            return new AutofacServiceProvider(resolver.Container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCorrelationId();

            app.UseMvc();
        }
    }
}
