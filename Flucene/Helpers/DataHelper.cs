using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;


namespace Lucene.Net.Odm.Helpers
{
    /// <summary>
    /// Represents the helper class for data operations such as parsing, determining the type of data, etc.
    /// </summary>
    public static class DataHelper
    {
        /// <summary>
        /// Converts the string representations of a specified conversion type to its equivalent.
        /// </summary>
        /// <param name="values">A collection of strings that contains object to convert.</param>
        /// <param name="conversionType">The target type.</param>
        /// <returns>object that is equivalent to the target type in <paramref name="values"/>.</returns>
        public static Object Parse(ICollection<string> values, Type conversionType)
        {
            if (conversionType.IsArray)
            {
                Type elementType = conversionType.GetElementType();
                return ArrayParse(values, elementType);
            }
            else if (IsGenericEnumerable(conversionType))
            {
                return EnumerableParse(values, conversionType);
            }
            else
            {
                return Parse(values.First(), conversionType);
            }
        }

        /// <summary>
        /// Converts the string representation of a specified conversion type to its equivalent.
        /// </summary>
        /// <param name="value">A string that contains object to convert.</param>
        /// <param name="conversionType">The target type.</param>
        /// <returns>object that is equivalent to the target type in <paramref name="value"/>.</returns>
        public static Object Parse(string value, Type conversionType)
        {
            if (!String.IsNullOrEmpty(value))
            {
                if (conversionType.IsEnum)
                    return Enum.Parse(conversionType, value);
                else if (conversionType == typeof(DateTime))
                    return DateTimeParse(value);
                else
                {
                    if (IsNullableType(conversionType))
                        conversionType = Nullable.GetUnderlyingType(conversionType);

                    return Convert.ChangeType(value, conversionType, CultureInfo.InvariantCulture);
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Create instance for the specified generic list type.
        /// </summary>
        /// <param name="targetType">The type of generic list.</param>
        /// <param name="elementType">The element type of generic list.</param>
        /// <returns>instance of generic list.</returns>
        public static IList MakeGenericList(Type targetType, Type elementType)
        {
            if (targetType.IsInterface)
                return (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));
            else
                return (IList)Activator.CreateInstance(targetType);
        }

        /// <summary>
        /// Returns a value that indicates the specified type is nullable.
        /// </summary>
        /// <param name="type">The target type.</param>
        /// <returns>true if <paramref name="type"/> is nullable; otherwise false.</returns>
        public static bool IsNullableType(Type type)
        {
            return type.IsGenericType && typeof(Nullable<>) == type.GetGenericTypeDefinition();
        }

        /// <summary>
        /// Returns a value that indicates the specified type implement <see cref="System.IConvertible"/> interface.
        /// </summary>
        /// <param name="type">The target type.</param>
        /// <returns>true if <paramref name="type"/> is convertible; otherwise false.</returns>
        public static bool IsConvertibleType(Type type)
        {
            return typeof(IConvertible).IsAssignableFrom(type);
        }

        /// <summary>
        /// Returns a value that indicates whether the specified type is generic enumerable.
        /// </summary>
        /// <param name="type">The target type.</param>
        /// <returns>true if <paramref name="type"/> is generic enumerable; otherwise false.</returns>
        public static bool IsGenericEnumerable(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                return true;

            return typeof(string) != type &&
                type.GetInterfaces()
                .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }



        private static DateTime DateTimeParse(string source)
        {
            long binaryDateTime;

            if (long.TryParse(source, out binaryDateTime))
            {
                return DateTime.FromBinary(binaryDateTime);
            }
            return DateTime.Parse(source, CultureInfo.InvariantCulture);
        }

        private static Array ArrayParse(ICollection<string> values, Type elementType)
        {
            Array array = Array.CreateInstance(elementType, values.Count);
            int i = 0;
            foreach (string value in values)
            {
                array.SetValue(Parse(value, elementType), i++);
            }
            return array;
        }

        private static IEnumerable EnumerableParse(IEnumerable<string> values, Type targetType)
        {
            Type elementType = targetType.GetGenericArguments()[0];
            IEnumerable<object> enumerable = values
                .Select(x => Parse(x, elementType));

            IList collection = MakeGenericList(targetType, elementType);

            foreach (var o in enumerable)
            {
                collection.Add(o);
            }
            return collection;
        }
    }
}
