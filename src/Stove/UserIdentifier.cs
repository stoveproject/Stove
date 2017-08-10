using System;
using System.Reflection;

using Stove.Extensions;

namespace Stove
{
	/// <summary>
	///     Used to identify a user.
	/// </summary>
	[Serializable]
	public class UserIdentifier : IUserIdentifier
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="UserIdentifier" /> class.
		/// </summary>
		protected UserIdentifier()
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="UserIdentifier" /> class.
		/// </summary>
		/// <param name="userId">Id of the user.</param>
		public UserIdentifier(long userId)
		{
			UserId = userId;
		}

		/// <summary>
		///     Id of the user.
		/// </summary>
		public long UserId { get; protected set; }

		/// <summary>
		///     Parses given string and creates a new <see cref="UserIdentifier" /> object.
		/// </summary>
		/// <param name="userIdentifierString">
		///     Should be formatted one of the followings:
		///     - "userId". Ex: 1 (for host users)
		/// </param>
		public static UserIdentifier Parse(string userIdentifierString)
		{
			if (userIdentifierString.IsNullOrEmpty())
			{
				throw new ArgumentNullException(nameof(userIdentifierString), "user can not be null or empty!");
			}

			string[] splitted = userIdentifierString.Split('@');
			if (splitted.Length == 1)
			{
				return new UserIdentifier(splitted[0].To<long>());
			}

			if (splitted.Length == 2)
			{
				return new UserIdentifier(splitted[0].To<long>());
			}

			throw new ArgumentException("user is not properly formatted", nameof(userIdentifierString));
		}

		/// <summary>
		///     Creates a string represents this <see cref="UserIdentifier" /> instance.
		///     Formatted one of the followings:
		///     - "userId". Ex: 1 (for host users)
		///     Returning string can be used in <see cref="Parse" /> method to re-create identical <see cref="UserIdentifier" />
		///     object.
		/// </summary>
		public string ToUserIdentifierString()
		{
			return UserId.ToString();
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is UserIdentifier))
			{
				return false;
			}

			//Same instances must be considered as equal
			if (ReferenceEquals(this, obj))
			{
				return true;
			}

			//Transient objects are not considered as equal
			var other = (UserIdentifier)obj;

			//Must have a IS-A relation of types or must be same type
			Type typeOfThis = GetType();
			Type typeOfOther = other.GetType();
			if (!typeOfThis.GetTypeInfo().IsAssignableFrom(typeOfOther) && !typeOfOther.GetTypeInfo().IsAssignableFrom(typeOfThis))
			{
				return false;
			}

			return UserId == other.UserId;
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			return (int)UserId;
		}

		/// <inheritdoc />
		public static bool operator ==(UserIdentifier left, UserIdentifier right)
		{
			if (Equals(left, null))
			{
				return Equals(right, null);
			}

			return left.Equals(right);
		}

		/// <inheritdoc />
		public static bool operator !=(UserIdentifier left, UserIdentifier right)
		{
			return !(left == right);
		}

		public override string ToString()
		{
			return ToUserIdentifierString();
		}
	}

	public static class UserIdentifierExtensions
	{
		/// <summary>
		///     Creates a new <see cref="UserIdentifier" /> object from any object implements <see cref="IUserIdentifier" />.
		/// </summary>
		/// <param name="userIdentifier">User identifier.</param>
		public static UserIdentifier ToUserIdentifier(this IUserIdentifier userIdentifier)
		{
			return new UserIdentifier(userIdentifier.UserId);
		}
	}
}
