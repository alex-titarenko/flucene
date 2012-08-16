using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lucene.Net.Documents;
using Lucene.Net.Odm.Mapping;
using System.Globalization;
using System.Collections;
using Lucene.Net.Odm.Mapping.Configuration;


namespace Lucene.Net.Odm.Helpers
{
    public static class MappingHelper
    {
        public static readonly CultureInfo StringFormat = CultureInfo.InvariantCulture;

        private const int DefaultPrecisionStep = 64;


        public static IEnumerable<Fieldable> GetFields(FieldMapping mapping, Object value, string prefix)
        {
            if (value == null) return null;

            IEnumerable<AbstractField> fields = GetFieldsInternal(mapping, value, mapping.FieldName, prefix);

            if (mapping.Boost.HasValue)
            {
                foreach (AbstractField field in fields)
                {
                    field.SetBoost(mapping.Boost.Value);
                }
            }

            return fields;
        }


        private static IEnumerable<AbstractField> GetFieldsInternal(FieldMapping mapping, Object value, string fieldName, string prefix)
        {
            if (IsPrimitive(value))
                return new AbstractField[] { CreateField(mapping, value, prefix + fieldName) };
            else if (value is IEnumerable)
                return CreateEnumerableField(mapping, value as IEnumerable, fieldName, prefix);
            else if (value is KeyValuePair<string, object>)
                return CreateField(mapping, (KeyValuePair<string, object>)value, prefix);
            else
                throw new ArgumentException(
                    String.Format(Properties.Resources.EXC_FIELD_MAPPING_TYPE_NOT_SUPPORTED, value.GetType()));
        }

        private static AbstractField CreateField(FieldMapping mapping, object value, string fieldName)
        {
            if (mapping.IsNumeric)
            {
                return CreateNumericField(mapping, (ValueType)value, fieldName);
            }
            else
            {
                string strValue = (value is IFormattable) ?
                        ((IFormattable)value).ToString(null, StringFormat) : value.ToString();

                return new Field(fieldName, strValue, mapping.Store, mapping.Index);
            }
        }

        private static IEnumerable<AbstractField> CreateEnumerableField(FieldMapping mapping, IEnumerable values, string fieldName, string prefix)
        {
            List<AbstractField> fields = new List<AbstractField>();
            foreach (object obj in values)
            {
                fields.AddRange(GetFieldsInternal(mapping, obj, fieldName, prefix));
            }

            return fields;
        }

        private static IEnumerable<AbstractField> CreateField(FieldMapping mapping, KeyValuePair<string, object> pair, string prefix)
        {
            return GetFieldsInternal(mapping, pair.Value, pair.Key, prefix);
        }

        private static AbstractField CreateNumericField(FieldMapping mapping, ValueType value, string fieldName)
        {
            NumericField numField = new NumericField(
                    fieldName, DefaultPrecisionStep,
                    mapping.Store, mapping.Index != Field.Index.NO);

            if (value is int)
                numField.SetIntValue((int)value);
            else if (value is long)
                numField.SetLongValue((long)value);
            else if (value is float)
                numField.SetFloatValue((float)value);
            else if (value is double)
                numField.SetDoubleValue((double)value);
            else if (value is decimal)
                numField.SetDoubleValue((double)(decimal)value);
            else if (value is DateTime)
                numField.SetLongValue(((DateTime)value).ToBinary());
            else
                throw new Exception(String.Format(Properties.Resources.EXC_TYPE_NOT_SUPPORTED, value));

            return numField;
        }



        public static Field GetField(string data, string fieldName, Field.Index indexField, bool storeData = true)
        {
            var store = storeData ? Field.Store.YES : Field.Store.NO;
            return new Field(fieldName, String.IsNullOrEmpty(data) ? String.Empty : data, store, indexField);
        }

        public static Field GetField(double data, string fieldName, bool analyzeInIndex = false, bool storeData = true)
        {
            return GetField(data.ToString(StringFormat), fieldName, analyzeInIndex, storeData);
        }

        public static Field GetField(int data, string fieldName, bool analyzeInIndex = false, bool storeData = true)
        {
            return GetField(data.ToString(StringFormat), fieldName, analyzeInIndex, storeData);
        }

        public static Field GetField(DateTime date, string fieldName, bool analyzeInIndex = false, bool storeData = true)
        {
            return GetField(DateTools.DateToString(date, DateTools.Resolution.SECOND), fieldName, analyzeInIndex, storeData);
        }

        public static Field GetField(bool data, string fieldName, bool analyzeInIndex = false, bool storeData = true)
        {
            return GetField(data.ToString(StringFormat), fieldName, analyzeInIndex, storeData);
        }

        public static Field GetField(string data, string fieldName, bool analyzeInIndex = false, bool storeData = true)
        {
            var store = storeData ? Field.Store.YES : Field.Store.NO;
            var analyze = analyzeInIndex ? Field.Index.ANALYZED : Field.Index.NOT_ANALYZED;
            return new Field(fieldName, String.IsNullOrEmpty(data) ? String.Empty : data, store, analyze);
        }




        private static bool IsPrimitive(Object value)
        {
            return
                value is string ||
                value is Enum ||
                value is Boolean ||
                value is int ||
                value is long ||
                value is float ||
                value is double ||
                value is decimal ||
                value is DateTime;
        }

    }
}
