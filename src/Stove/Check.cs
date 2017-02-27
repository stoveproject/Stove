using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

using Stove.Collections.Extensions;
using Stove.Extensions;

namespace Stove
{
    [DebuggerStepThrough]
    public static class Check
    {
        [ContractAnnotation("value:null => halt")]
        public static T NotNull<T>(T value, [InvokerParameterName] [NotNull] string parameterName)
        {
            if (value == null)
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw new ArgumentNullException(parameterName);
            }

            return value;
        }

        [ContractAnnotation("value:null => halt")]
        public static T NotNull<T>(
            [NoEnumeration] T value,
            [InvokerParameterName] [NotNull] string parameterName,
            [NotNull] string propertyName)
        {
            if (ReferenceEquals(value, null))
            {
                NotEmpty(parameterName, nameof(parameterName));
                NotEmpty(propertyName, nameof(propertyName));

                throw new ArgumentException(propertyName + " " + parameterName + " argument property is null!");
            }

            return value;
        }

        [ContractAnnotation("value:null => halt")]
        public static string NotNullOrWhiteSpace(string value, [InvokerParameterName] [NotNull] string parameterName)
        {
            if (value.IsNullOrWhiteSpace())
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw new ArgumentException($"{parameterName} can not be null, empty or white space!", parameterName);
            }

            return value;
        }

        [ContractAnnotation("value:null => halt")]
        public static ICollection<T> NotNullOrEmpty<T>(ICollection<T> value, [InvokerParameterName] [NotNull] string parameterName)
        {
            if (value.IsNullOrEmpty())
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw new ArgumentException(parameterName + " can not be null or empty!", parameterName);
            }

            return value;
        }

        [ContractAnnotation("value:null => halt")]
        public static string NullButNotEmpty(string value, [InvokerParameterName] [NotNull] string parameterName)
        {
            if (!ReferenceEquals(value, null) && value.Length == 0)
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw new ArgumentException(parameterName + "is empty!", parameterName);
            }

            return value;
        }

        public static IReadOnlyList<T> HasNoNulls<T>(IReadOnlyList<T> value, [InvokerParameterName] [NotNull] string parameterName)
            where T : class
        {
            NotNull(value, parameterName);

            if (value.Any(e => e == null))
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw new ArgumentException(parameterName);
            }

            return value;
        }

        public static T IsDefined<T>(T value, [InvokerParameterName] [NotNull] string parameterName)
            where T : struct
        {
            if (!Enum.IsDefined(typeof(T), value))
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw new ArgumentException(parameterName + " is an invalid Enum!", parameterName);
            }

            return value;
        }

        public static Type ValidEntityType(Type value, [InvokerParameterName] [NotNull] string parameterName)
        {
            if (!value.GetTypeInfo().IsClass)
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw new ArgumentException(parameterName + " is an invalid entity type!", parameterName);
            }

            return value;
        }

        [ContractAnnotation("value:null => halt")]
        public static string NotEmpty(string value, [InvokerParameterName] [NotNull] string parameterName)
        {
            Exception exception = null;
            if (ReferenceEquals(value, null))
            {
                exception = new ArgumentNullException(parameterName);
            }
            else if (value.Trim().Length == 0)
            {
                exception = new ArgumentException(parameterName + " can not be empty!", parameterName);
            }

            if (exception != null)
            {
                NotEmpty(parameterName, nameof(parameterName));

                // ReSharper disable once UnthrowableException
                throw exception;
            }

            return value;
        }
    }
}
