using System.Text;
using System.Threading.Tasks;

using Autofac.Extras.IocManager;

using StackExchange.Redis.Extensions.Core;

namespace Stove.Redis.Redis
{
    public class RedisSerializer : ISerializer, ITransientDependency
    {
        private readonly IRedisCacheSerializer _redisCacheSerializer;

        public RedisSerializer(IRedisCacheSerializer redisCacheSerializer)
        {
            _redisCacheSerializer = redisCacheSerializer;
        }

        public byte[] Serialize(object item)
        {
            return Encoding.UTF8.GetBytes(_redisCacheSerializer.Serialize(item));
        }

        public Task<byte[]> SerializeAsync(object item)
        {
            return Task.FromResult(Serialize(item));
        }

        public object Deserialize(byte[] serializedObject)
        {
            return _redisCacheSerializer.Deserialize(serializedObject);
        }

        public Task<object> DeserializeAsync(byte[] serializedObject)
        {
            return Task.FromResult(Deserialize(serializedObject));
        }

        public T Deserialize<T>(byte[] serializedObject)
        {
            return (T)_redisCacheSerializer.Deserialize(serializedObject);
        }

        public Task<T> DeserializeAsync<T>(byte[] serializedObject)
        {
            return Task.FromResult(Deserialize<T>(serializedObject));
        }
    }
}
