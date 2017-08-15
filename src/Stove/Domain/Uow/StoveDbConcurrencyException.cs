using System;
using System.Runtime.Serialization;

namespace Stove.Domain.Uow
{
	public class StoveDbConcurrencyException : StoveException
	{
		/// <summary>
		///     Creates a new <see cref="StoveDbConcurrencyException" /> object.
		/// </summary>
		public StoveDbConcurrencyException()
		{
		}

		/// <summary>
		///     Creates a new <see cref="StoveException" /> object.
		/// </summary>
		public StoveDbConcurrencyException(SerializationInfo serializationInfo, StreamingContext context)
			: base(serializationInfo, context)
		{
		}

		/// <summary>
		///     Creates a new <see cref="StoveDbConcurrencyException" /> object.
		/// </summary>
		/// <param name="message">Exception message</param>
		public StoveDbConcurrencyException(string message)
			: base(message)
		{
		}

		/// <summary>
		///     Creates a new <see cref="StoveDbConcurrencyException" /> object.
		/// </summary>
		/// <param name="message">Exception message</param>
		/// <param name="innerException">Inner exception</param>
		public StoveDbConcurrencyException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
