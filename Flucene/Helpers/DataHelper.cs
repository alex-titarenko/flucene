using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;


namespace Lucene.Net.Odm.Helpers
{
    public static class DataHelper
    {
        public static Object Parse(string[] values, Type conversionType)
        {
            if (conversionType.IsArray)
            {
                Type elementType = conversionType.GetElementType();
                return CreateArray(values, elementType);
            }
            else if (IsGenericEnumerable(conversionType))
            {
                return CreateCollection(values, conversionType);
            }
            else
            {
                return Parse(values[0], conversionType);
            }
        }

        public static Object Parse(string value, Type conversionType)
        {
            if (!String.IsNullOrEmpty(value))
            {
                if (conversionType.IsEnum)
                    return Enum.Parse(conversionType, value);
                else if (conversionType == typeof(DateTime))
                    return ParseDateTime(value);
                else
                {
                    if (IsNullableType(conversionType))
                        conversionType = Nullable.GetUnderlyingType(conversionType);

                    return Convert.ChangeType(value, conversionType);
                }
            }
            else
            {
                return null;
            }
        }


        public static DateTime ParseDateTime(string source)
        {
            long binaryDateTime;

            if (long.TryParse(source, out binaryDateTime))
                return DateTime.FromBinary(binaryDateTime);
            return DateTime.Parse(source, CultureInfo.InvariantCulture);
        }


        public static bool IsNullableType(Type type)
        {
            return type.IsGenericType && typeof(Nullable<>) == type.GetGenericTypeDefinition();
        }

        public static bool IsConvertibleType(Type type)
        {
            return typeof(IConvertible).IsAssignableFrom(type);
        }


        public static bool IsGenericEnumerable(Type type)
        {
            return typeof(string) != type &&
                type.GetInterfaces()
                .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }

        private static Array CreateArray(string[] values, Type elementType)
        {
            Array array = Array.CreateInstance(elementType, values.Length);
            for (int i = 0; i < values.Length; i++)
            {
                array.SetValue(Parse(values[i], elementType), i);
            }
            return array;
        }

        private static IEnumerable CreateCollection(string[] values, Type targetType)
        {
            Type listType = targetType.GetGenericArguments()[0];
            IEnumerable<object> enumerable = values
                .Select(x => Parse(x, listType));

            IList collection = null;

            if (targetType.IsInterface)
                collection = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(listType));
            else
                collection = (IList)Activator.CreateInstance(targetType);

            foreach (var o in enumerable)
            {
                collection.Add(o);
            }
            return collection;
        }
    }
}
