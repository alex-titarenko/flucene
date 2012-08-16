using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using Lucene.Net.Documents;


namespace Lucene.Net.Odm.Helpers
{
    public static class DocumentExtensions
    {
        private const string ItemsCountFieldSuffix = "_COUNT";


        public static string Extract(this Document source, string name)
        {
            string value = source.GetInvariant(name);
            source.RemoveField(name);
            return value;
        }

        public static IList<string> ExtractValues(this Document source, string name)
        {
            int count = source.ExtractItemsCount(name);

            IList<string> values = new List<string>(count);
            IList fields = source.GetFields();
            
            for (int i = 0; i < fields.Count; i++)
            {
                Fieldable field = fields[i] as Fieldable;
                if (name.Equals(field.Name()))
                {
                    values.Add(field.InvariantStringValue());
                    fields.Remove(field);
                    i--;
                }
            }

            return values;
        }

        public static string GetInvariant(this Document source, string name)
        {
            Fieldable fieldable = source.GetFieldable(name);

            if (fieldable != null)
                return fieldable.InvariantStringValue();
            else
                return null;
        }

        public static string InvariantStringValue(this Fieldable source)
        {
            if (source is NumericField)
            {
                return ((IFormattable)((NumericField)source).GetNumericValue())
                    .ToString(null, CultureInfo.InvariantCulture);
            }
            else
            {
                return source.StringValue();
            }
        }

        public static void AddEmpty(this Document source, string name)
        {
            source.Add(new Field(name, String.Empty, Field.Store.YES, Field.Index.NO));
        }

        public static void AddItemsCount(this Document source, string name, int count)
        {
            source.Add(new NumericField(GetItemsCountFieldName(name), Field.Store.YES, false).SetIntValue(count));
        }

        public static int ExtractItemsCount(this Document source, string name)
        {
            string numFieldName = GetItemsCountFieldName(name);
            
            Fieldable field = source.GetFieldable(numFieldName);

            if (field != null)
            {
                int count = (int)((NumericField)field).GetNumericValue();
                source.RemoveField(numFieldName);
                return count;
            }
            else
            {
                return 1;
            }
        }


        private static string GetItemsCountFieldName(string baseFieldName)
        {
            return baseFieldName + ItemsCountFieldSuffix;
        }
    }
}
