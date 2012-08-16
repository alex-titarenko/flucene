using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;


namespace Lucene.Net.Odm.Helpers
{
    public static class DataHelper
    {
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


        public static DateTime DateTimeParse(string source)
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

        public static IList MakeGenericList(Type targetType, Type elementType)
        {
            if (targetType.IsInterface)
                return (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));
            else
                return (IList)Activator.CreateInstance(targetType);
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
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                return true;

            return typeof(string) != type &&
                type.GetInterfaces()
                .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }
    }
}
