using Stove.Runtime.Remoting;

namespace Stove.Runtime.Session
{
	/// <summary>
	///     Implements null object pattern for <see cref="IStoveSession" />.
	/// </summary>
	public class NullStoveSession : StoveSessionBase
	{
		private NullStoveSession() : base(new DataContextAmbientScopeProvider<SessionOverride>(

#if NET461
                new CallContextAmbientDataContext()
#else
				new AsyncLocalAmbientDataContext()
#endif
			))
		{
		}

		/// <summary>
		///     Singleton instance.
		/// </summary>
		public static NullStoveSession Instance { get; } = new NullStoveSession();

		/// <inheritdoc />
		public override long? UserId => null;

		public override long? ImpersonatorUserId => null;
	}
}
