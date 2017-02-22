using JetBrains.Annotations;

using Stove.Runtime.Session;

namespace Stove.Application.Services
{
    /// <summary>
    ///     This class can be used as a base class for application services.
    /// </summary>
    /// <seealso cref="Stove.StoveServiceBase" />
    /// <seealso cref="Stove.Application.Services.IApplicationService" />
    public abstract class ApplicationService : StoveServiceBase, IApplicationService
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        protected ApplicationService()
        {
            StoveSession = NullStoveSession.Instance;
        }

        /// <summary>
        ///     Gets current session information.
        /// </summary>
        /// <value>
        ///     The stove session.
        /// </value>
        [NotNull]
        public IStoveSession StoveSession { get; set; }
    }
}
