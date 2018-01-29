using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

using Stove.Commands;

namespace Stove.WebApi.Commands
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IOptions<CommandContextOptions> _options;

        /// <summary>
        ///     Creates a new instance of the CorrelationIdMiddleware.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="options">The configuration options.</param>
        public CorrelationIdMiddleware(RequestDelegate next, IOptions<CommandContextOptions> options)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        ///     Processes a request to synchronise TraceIdentifier and Correlation ID headers. Also creates a
        ///     <see cref="CommandContext" /> for the current request and disposes of it when the request is completing.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext" /> for the current request.</param>
        /// <param name="commandContextAccessor">
        ///     The <see cref="IStoveCommandContextAccessor" /> which can create a
        ///     <see cref="CommandContext" />.
        /// </param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context, IStoveCommandContextAccessor commandContextAccessor)
        {
            if (context.Request.Headers.TryGetValue(_options.Value.Header, out StringValues correlationId))
            {
                context.TraceIdentifier = correlationId;
            }
            else
            {
                correlationId = Guid.NewGuid().ToString();
                context.TraceIdentifier = correlationId;
            }

            using (commandContextAccessor.Use(correlationId))
            {
                if (_options.Value.IncludeInResponse)
                {
                    // apply the correlation ID to the response header for client side tracking
                    context.Response.OnStarting(() =>
                    {
                        if (!context.Response.Headers.ContainsKey(_options.Value.Header))
                        {
                            context.Response.Headers.Add(_options.Value.Header, context.TraceIdentifier);
                        }

                        return Task.CompletedTask;
                    });
                }

                await _next(context);
            }
        }
    }
}
