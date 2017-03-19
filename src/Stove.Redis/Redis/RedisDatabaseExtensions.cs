using System;

using JetBrains.Annotations;

using StackExchange.Redis;

namespace Stove.Redis
{
    /// <summary>
    ///     Extension methods for <see cref="IDatabase" />.
    /// </summary>
    internal static class RedisDatabaseExtensions
    {
        public static void KeyDeleteWithPrefix([NotNull] this IDatabase database, [NotNull] string prefix)
        {
            Check.NotNull(database, nameof(database));
            Check.NotNull(prefix, nameof(prefix));

            if (string.IsNullOrWhiteSpace(prefix))
            {
                throw new ArgumentException("Prefix cannot be empty", nameof(database));
            }

            database.ScriptEvaluate(@"
                local keys = redis.call('keys', ARGV[1]) 
                for i=1,#keys,5000 do 
                redis.call('del', unpack(keys, i, math.min(i+4999, #keys)))
                end", values: new RedisValue[] { prefix });
        }

        public static int KeyCount([NotNull] this IDatabase database, [NotNull] string prefix)
        {
            Check.NotNull(database, nameof(database));
            Check.NotNull(prefix, nameof(prefix));

            RedisResult retVal = database.ScriptEvaluate("return table.getn(redis.call('keys', ARGV[1]))", values: new RedisValue[] { prefix });

            if (retVal.IsNull)
            {
                return 0;
            }

            return (int)retVal;
        }
    }
}
