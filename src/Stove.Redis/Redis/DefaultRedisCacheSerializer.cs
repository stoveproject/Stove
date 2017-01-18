using System;

using Autofac.Extras.IocManager;

using StackExchange.Redis;

using Stove.Json;

namespace Stove.Redis.Redis
{
    /// <summary>
    ///     Default implementation uses JSON as the underlying persistence mechanism.
    /// </summary>
    public class DefaultRedisCacheSerializer : IRedisCacheSerializer, ITransientDependency
    {
        /// <summary>
        ///     Creates an instance of the object from its serialized string representation.
        /// </summary>
        /// <param name="objbyte">String representation of the object from the Redis server.</param>
        /// <returns>Returns a newly constructed object.</returns>
        /// <seealso cref="IRedisCacheSerializer.Serialize" />
        public virtual object Deserialize(RedisValue objbyte)
        {
            return JsonSerializationHelper.DeserializeWithType(objbyte);
        }

        /// <summary>
        ///     Produce a string representation of the supplied object.
        /// </summary>
        /// <param name="value">Instance to serialize.</param>
        /// <param name="type">Type of the object.</param>
        /// <returns>Returns a string representing the object instance that can be placed into the Redis cache.</returns>
        /// <seealso cref="IRedisCacheSerializer.Deserialize" />
        public virtual string Serialize(object value, Type type)
        {
            return JsonSerializationHelper.SerializeWithType(value, type);
        }

        /// <summary>
        ///     Produce a string representation of the supplied object.
        /// </summary>
        /// <param name="value">Instance to serialize.</param>
        /// <returns>
        ///     Returns a string representing the object instance that can be placed into the Redis cache.
        /// </returns>
        /// <seealso cref="Deserialize" />
        public string Serialize(object value)
        {
            return JsonSerializationHelper.SerializeWithType(value);
        }
    }
}
