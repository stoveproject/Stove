using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace Stove.WebApi.Commands
{
    public static class CorrelationIdExtensions
    {
        /// <summary>
        ///     Enables correlation IDs for the request.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app)
        {
            Check.NotNull(app, nameof(app));

            return app.UseCorrelationId(new CommandContextOptions());
        }

        /// <summary>
        ///     Enables correlation IDs for the request.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="header">The header field name to use for the correlation ID.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app, string header)
        {
            Check.NotNull(app, nameof(app));

            return app.UseCorrelationId(new CommandContextOptions
            {
                Header = header
            });
        }

        /// <summary>
        ///     Enables correlation IDs for the request.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app, CommandContextOptions options)
        {
            Check.NotNull(app, nameof(app));
            Check.NotNull(options, nameof(options));

            return app.UseMiddleware<CorrelationIdMiddleware>(Options.Create(options));
        }
    }
}
