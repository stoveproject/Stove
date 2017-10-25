using System.Linq;
using System.Reflection;

namespace Stove.Reflection
{
    public static class ProxyHelper
    {
        /// <summary>
        ///     Returns dynamic proxy target object if this is a proxied object, otherwise returns the given object.
        /// </summary>
        public static object UnProxyOrSelf(object obj)
        {
            if (obj.GetType().Namespace != "Castle.Proxies")
            {
                return obj;
            }

            FieldInfo targetField = obj.GetType().GetTypeInfo()
                                       .GetFields()
                                       .FirstOrDefault(f => f.Name == "__target");

            if (targetField == null)
            {
                return obj;
            }

            return targetField.GetValue(obj);
        }
    }
}
