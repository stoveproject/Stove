using System;
using System.Runtime.Serialization;

namespace Stove
{
	/// <summary>
	///     Base exception type for those are thrown by Stove system for Stove specific exceptions.
	/// </summary>
	[Serializable]
	public class StoveException : Exception
	{
		/// <summary>
		///     Creates a new <see cref="StoveException" /> object.
		/// </summary>
		public StoveException()
		{
		}

		/// <summary>
		///     Creates a new <see cref="StoveException" /> object.
		/// </summary>
		public StoveException(SerializationInfo serializationInfo, StreamingContext context)
			: base(serializationInfo, context)
		{
		}

		/// <summary>
		///     Creates a new <see cref="StoveException" /> object.
		/// </summary>
		/// <param name="message">Exception message</param>
		public StoveException(string message)
			: base(message)
		{
		}

		/// <summary>
		///     Creates a new <see cref="StoveException" /> object.
		/// </summary>
		/// <param name="message">Exception message</param>
		/// <param name="innerException">Inner exception</param>
		public StoveException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
