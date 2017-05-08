using System.Reflection;
using System.Web.Http;
using System.Web.Http.Cors;

using Autofac.Extras.IocManager;
using Autofac.Integration.WebApi;

using Microsoft.Owin;

using Owin;

using Stove.Demo.WebApi;

using Swashbuckle.Application;

[assembly: OwinStartup(typeof(Startup))]

namespace Stove.Demo.WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            IRootResolver rootResolver = IocBuilder.New
                                                   .UseAutofacContainerBuilder()
                                                   .UseStoveWithNullables<StoveDemoWebApiBootstapper>(true)
                                                   .UseStoveEntityFramework()
                                                   .UseStoveDbContextEfTransactionStrategy()
                                                   .UseStoveDapper()
                                                   .UseStoveEventBus()
                                                   .UseStoveNLog()
                                                   .RegisterServices(r =>
                                                   {
                                                       r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
                                                       r.UseBuilder(cb =>
                                                       {
                                                           cb.RegisterApiControllers(Assembly.GetExecutingAssembly());
                                                           cb.RegisterWebApiFilterProvider(config);
                                                       });
                                                   })
                                                   .CreateResolver();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(rootResolver.Container);
            config.EnableSwagger("docs/{apiVersion}/swagger", c => { c.SingleApiVersion("v1", "Stove WebApi").Description("Stove WebApi documentation."); })
                  .EnableSwaggerUi("help/{*assetPath}", c => { c.DisableValidator(); });

            config.MapHttpAttributeRoutes();
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));
            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional }
            );

            app.UseWebApi(config);
        }
    }
}
