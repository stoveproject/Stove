using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using JetBrains.Annotations;

namespace Stove.Extensions
{
    /// <summary>
    ///     Extension methods for all objects.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        ///     Used to simplify and beautify casting an object to a type.
        /// </summary>
        /// <typeparam name="T">Type to be casted</typeparam>
        /// <param name="obj">Object to cast</param>
        /// <returns>Casted object</returns>
        [NotNull]
        public static T As<T>([NotNull] this object obj)
            where T : class
        {
            return (T)obj;
        }

        /// <summary>
        ///     Converts given object to a value type using <see cref="Convert.ChangeType(object,System.TypeCode)" /> method.
        /// </summary>
        /// <param name="obj">Object to be converted</param>
        /// <typeparam name="T">Type of the target object</typeparam>
        /// <returns>Converted object</returns>
        public static T To<T>([NotNull] this object obj)
            where T : struct
        {
            return (T)Convert.ChangeType(obj, typeof(T), CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     Check if an item is in a list.
        /// </summary>
        /// <param name="item">Item to check</param>
        /// <param name="list">List of items</param>
        /// <typeparam name="T">Type of the items</typeparam>
        public static bool IsIn<T>(this T item, [NotNull] params T[] list)
        {
            return list.Contains(item);
        }

        /// <summary>
        ///     Fors the each.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <param name="action">The action.</param>
        public static void ForEach<T>([CanBeNull] this IEnumerable<T> items, [NotNull] Action<T> action)
        {
            if (items == null)
            {
                return;
            }

            foreach (T obj in items)
            {
                action(obj);
            }
        }
    }
}
