using System;
using System.Linq.Expressions;
using System.Reflection;

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using Stove.Timing;

namespace Stove.EntityFrameworkCore
{
    public class StoveEntityMaterializerSource : EntityMaterializerSource
    {
        private static readonly MethodInfo NormalizeDateTimeMethod = typeof(StoveEntityMaterializerSource).GetTypeInfo().GetMethod(nameof(NormalizeDateTime), BindingFlags.Static | BindingFlags.NonPublic);
        private static readonly MethodInfo NormalizeNullableDateTimeMethod = typeof(StoveEntityMaterializerSource).GetTypeInfo().GetMethod(nameof(NormalizeNullableDateTime), BindingFlags.Static | BindingFlags.NonPublic);

        public override Expression CreateReadValueExpression(Expression valueBuffer, Type type, int index, IPropertyBase propertyBase)
        {
            if (type == typeof(DateTime))
            {
                return Expression.Call(
                    NormalizeDateTimeMethod,
                    base.CreateReadValueExpression(valueBuffer, type, index, propertyBase)
                );
            }

            if (type == typeof(DateTime?))
            {
                return Expression.Call(
                    NormalizeNullableDateTimeMethod,
                    base.CreateReadValueExpression(valueBuffer, type, index, propertyBase)
                );
            }

            return base.CreateReadValueExpression(valueBuffer, type, index, propertyBase);
        }

        private static DateTime NormalizeDateTime(DateTime value)
        {
            return Clock.Normalize(value);
        }

        private static DateTime? NormalizeNullableDateTime(DateTime? value)
        {
            if (value == null)
            {
                return null;
            }

            return Clock.Normalize(value.Value);
        }
    }
}