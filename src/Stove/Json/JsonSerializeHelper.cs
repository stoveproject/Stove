using System;

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
        public static string SerializeWithType(object obj)
        {
            return SerializeWithType(obj, obj.GetType());
        }

        /// <summary>
        ///     Serializes an object with a type information included.
        ///     So, it can be deserialized using <see cref="DeserializeWithType" /> method later.
        /// </summary>
        public static string SerializeWithType(object obj, Type type)
        {
            string serialized = obj.ToJsonString();

            return $"{type.AssemblyQualifiedName}{TypeSeperator}{serialized}";
        }

        /// <summary>
        ///     Deserializes an object serialized with <see cref="SerializeWithType(object)" /> methods.
        /// </summary>
        public static T DeserializeWithType<T>(string serializedObj)
        {
            return (T)DeserializeWithType(serializedObj);
        }

        /// <summary>
        ///     Deserializes an object serialized with <see cref="SerializeWithType(object)" /> methods.
        /// </summary>
        public static object DeserializeWithType(string serializedObj)
        {
            int typeSeperatorIndex = serializedObj.IndexOf(TypeSeperator);
            Type type = Type.GetType(serializedObj.Substring(0, typeSeperatorIndex));
            string serialized = serializedObj.Substring(typeSeperatorIndex + 1);

            var options = new JsonSerializerSettings();
            options.Converters.Insert(0, new StoveDateTimeConverter());

            return JsonConvert.DeserializeObject(serialized, type, options);
        }
    }
}
