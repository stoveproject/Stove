using System;

using JetBrains.Annotations;

using StackExchange.Redis;

namespace Stove.Redis.Redis
{
    /// <summary>
    ///     Interface to be implemented by all custom (de)serialization methods used when persisting and retrieving
    ///     objects from the Redis cache.
    /// </summary>
    public interface IRedisCacheSerializer
    {
        /// <summary>
        ///     Creates an instance of the object from its serialized string representation.
        /// </summary>
        /// <param name="objbyte">String representation of the object from the Redis server.</param>
        /// <returns>
        ///     Returns a newly constructed object.
        /// </returns>
        [NotNull]
        object Deserialize(RedisValue objbyte);

        /// <summary>
        ///     Produce a string representation of the supplied object.
        /// </summary>
        /// <param name="value">Instance to serialize.</param>
        /// <param name="type">Type of the object.</param>
        /// <returns>Returns a string representing the object instance that can be placed into the Redis cache.</returns>
        /// <seealso cref="Deserialize" />
        [NotNull]
        string Serialize([NotNull] object value, [CanBeNull] Type type);

        /// <summary>
        ///     Produce a string representation of the supplied object.
        /// </summary>
        /// <param name="value">Instance to serialize.</param>
        /// <returns>
        ///     Returns a string representing the object instance that can be placed into the Redis cache.
        /// </returns>
        /// <seealso cref="Deserialize" />
        [NotNull]
        string Serialize([NotNull] object value);
    }
}
