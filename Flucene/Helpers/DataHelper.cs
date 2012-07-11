using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace Lucene.Net.Orm.Helpers
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
            else if (IsEnumerable(conversionType))
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
                {
                    return Enum.Parse(conversionType, value);
                }
                else if (conversionType == typeof(DateTime))
                {
                    long binaryDateTime;

                    if (long.TryParse(value, out binaryDateTime))
                        return DateTime.FromBinary(binaryDateTime);
                    return DateTime.Parse(value);
                }
                else
                {
                    if (conversionType.IsGenericType &&
                        typeof(Nullable<>) == conversionType.GetGenericTypeDefinition())
                        conversionType = Nullable.GetUnderlyingType(conversionType);

                    return Convert.ChangeType(value, conversionType);
                }
            }
            else
            {
                return null;
            }
        }


        private static bool IsEnumerable(Type type)
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
