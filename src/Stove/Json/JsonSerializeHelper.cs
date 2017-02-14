using System;

using JetBrains.Annotations;

using Newtonsoft.Json;

namespace Stove.Json
{
    /// <summary>
    ///     Defines helper methods to work with JSON.
    /// </summary>
    public static class JsonSerializationHelper
    {
        private const char TypeSeperator = '|';

        /// <summary>
        ///     Serializes an object with a type information included.
        ///     So, it can be deserialized using <see cref="DeserializeWithType" /> method later.
        /// </summary>
        [NotNull]
        public static string SerializeWithType([NotNull] object obj)
        {
            return SerializeWithType(obj, obj.GetType());
        }

        /// <summary>
        ///     Serializes the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        [NotNull]
        public static string Serialize([NotNull] object obj)
        {
            Check.NotNull(obj, nameof(obj));

            string serialized = obj.ToJsonString();
            return serialized;
        }

        /// <summary>
        ///     Serializes an object with a type information included.
        ///     So, it can be deserialized using <see cref="DeserializeWithType" /> method later.
        /// </summary>
        [NotNull]
        public static string SerializeWithType([NotNull] object obj, [NotNull] Type type)
        {
            Check.NotNull(obj, nameof(obj));
            Check.NotNull(type, nameof(type));

            string serialized = obj.ToJsonString();

            return $"{type.AssemblyQualifiedName}{TypeSeperator}{serialized}";
        }

        /// <summary>
        ///     Deserializes an object serialized with <see cref="SerializeWithType(object)" /> methods.
        /// </summary>
        [NotNull]
        public static T DeserializeWithType<T>([NotNull] string serializedObj)
        {
            return (T)DeserializeWithType(serializedObj);
        }

        /// <summary>
        ///     Deserializes an object serialized with <see cref="SerializeWithType(object)" /> methods.
        /// </summary>
        [NotNull]
        public static object DeserializeWithType([NotNull] string serializedObj)
        {
            int typeSeperatorIndex = serializedObj.IndexOf(TypeSeperator);
            Type type = Type.GetType(serializedObj.Substring(0, typeSeperatorIndex));
            string serialized = serializedObj.Substring(typeSeperatorIndex + 1);

            var options = new JsonSerializerSettings();
            options.Converters.Insert(0, new StoveDateTimeConverter());

            return JsonConvert.DeserializeObject(serialized, type, options);
        }

        /// <summary>
        ///     Deserializes the specified serialized object.
        /// </summary>
        /// <param name="serializedObj">The serialized object.</param>
        /// <returns></returns>
        [NotNull]
        public static object Deserialize([NotNull] string serializedObj)
        {
            var options = new JsonSerializerSettings();
            options.Converters.Insert(0, new StoveDateTimeConverter());

            return JsonConvert.DeserializeObject(serializedObj, options);
        }
    }
}
