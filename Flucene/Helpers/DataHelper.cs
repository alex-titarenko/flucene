using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


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
            else if (typeof(string) != conversionType &&
                typeof(IList).IsAssignableFrom(conversionType) &&
                conversionType.IsGenericType)
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
            if (conversionType.IsEnum)
                return Enum.Parse(conversionType, value);
            else
                return Convert.ChangeType(value, conversionType);
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
            
            IList collection = (IList)Activator.CreateInstance(targetType);

            foreach (var o in enumerable)
            {
                collection.Add(o);
            }
            return collection;
        }
    }
}
