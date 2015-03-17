using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using Lucene.Net.Documents;
using Lucene.Net.Odm.Mapping;
using Lucene.Net.Odm.Mapping.Configuration;


namespace Lucene.Net.Odm.Helpers
{
    /// <summary>
    /// Represents the helper methods for the document mapping operations.
    /// </summary>
    public static class MappingHelper
    {
        /// <summary>
        /// Gets the default string format for the document mapping.
        /// </summary>
        public static readonly CultureInfo StringFormat = CultureInfo.InvariantCulture;

        private const bool DefautlAnalyzed = false;
        private const bool DefaultStored = true;
        private const int DefaultPrecisionStep = 64;


        /// <summary>
        /// Returns the field collection by the specified field mapping and object.
        /// </summary>
        /// <param name="mapping">The field mapping to convert an object into a set of fields.</param>
        /// <param name="value">An object for which to get a set of fields.</param>
        /// <param name="prefix">A <see cref="System.String"/> representing the prefix for the all field names.</param>
        /// <returns>set of fields for document.</returns>
        public static IEnumerable<IFieldable> GetFields(FieldMapping mapping, Object value, string prefix)
        {
            if (value == null) return null;

            IEnumerable<AbstractField> fields = GetFieldsInternal(mapping, value, mapping.FieldName, prefix);

            if (mapping.Boost.HasValue)
            {
                foreach (AbstractField field in fields)
                {
                    field.Boost = mapping.Boost.Value;
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


        /// <summary>
        /// Returns the field by the specified name and value.
        /// </summary>
        /// <param name="name">A <see cref="System.String"/> represents the field name.</param>
        /// <param name="value">A field value.</param>
        /// <param name="index">An object that indicates how want a field should be indexing.</param>
        /// <param name="stored">A boolean value that indicates whether to store the field in the index.</param>
        /// <returns>field by the <paramref name="name"/> and <paramref name="value"/>.</returns>
        public static Field GetField(string name, string value, Field.Index index, bool stored = DefaultStored)
        {
            var fieldStore = stored ? Field.Store.YES : Field.Store.NO;
            return new Field(name, String.IsNullOrEmpty(value) ? String.Empty : value, fieldStore, index);
        }

        /// <summary>
        /// Returns the field by the specified name and value.
        /// </summary>
        /// <param name="name">A <see cref="System.String"/> represents the field name.</param>
        /// <param name="value">A field value.</param>
        /// <param name="analyzed">A boolean value that indicates whether to analyze the field in the index.</param>
        /// <param name="stored">A boolean value that indicates whether to store the field in the index.</param>
        /// <returns>field by the <paramref name="name"/> and <paramref name="value"/>.</returns>
        public static Field GetField(string name, double value, bool analyzed = DefautlAnalyzed, bool stored = DefaultStored)
        {
            return GetField(name, value.ToString(StringFormat), analyzed, stored);
        }

        /// <summary>
        /// Returns the field by the specified name and value.
        /// </summary>
        /// <param name="name">A <see cref="System.String"/> represents the field name.</param>
        /// <param name="value">A field value.</param>
        /// <param name="analyzed">A boolean value that indicates whether to analyze the field in the index.</param>
        /// <param name="stored">A boolean value that indicates whether to store the field in the index.</param>
        /// <returns>field by the <paramref name="name"/> and <paramref name="value"/>.</returns>
        public static Field GetField(string name, int value, bool analyzed = DefautlAnalyzed, bool stored = DefaultStored)
        {
            return GetField(name, value.ToString(StringFormat), analyzed, stored);
        }

        /// <summary>
        /// Returns the field by the specified name and value.
        /// </summary>
        /// <param name="name">A <see cref="System.String"/> represents the field name.</param>
        /// <param name="value">A field value.</param>
        /// <param name="analyzed">A boolean value that indicates whether to analyze the field in the index.</param>
        /// <param name="stored">A boolean value that indicates whether to store the field in the index.</param>
        /// <returns>field by the <paramref name="name"/> and <paramref name="value"/>.</returns>
        public static Field GetField(string name, DateTime value, bool analyzed = DefautlAnalyzed, bool stored = DefaultStored)
        {
            return GetField(name, DateTools.DateToString(value, DateTools.Resolution.SECOND), analyzed, stored);
        }

        /// <summary>
        /// Returns the field by the specified name and value.
        /// </summary>
        /// <param name="name">A <see cref="System.String"/> represents the field name.</param>
        /// <param name="value">A field value.</param>
        /// <param name="analyzed">A boolean value that indicates whether to analyze the field in the index.</param>
        /// <param name="stored">A boolean value that indicates whether to store the field in the index.</param>
        /// <returns>field by the <paramref name="name"/> and <paramref name="value"/>.</returns>
        public static Field GetField(string name, bool value, bool analyzed = DefautlAnalyzed, bool stored = DefaultStored)
        {
            return GetField(name, value.ToString(StringFormat), analyzed, stored);
        }

        /// <summary>
        /// Returns the field by the specified name and value.
        /// </summary>
        /// <param name="name">A <see cref="System.String"/> represents the field name.</param>
        /// <param name="value">A field value.</param>
        /// <param name="analyzed">A boolean value that indicates whether to analyze the field in the index.</param>
        /// <param name="stored">A boolean value that indicates whether to store the field in the index.</param>
        /// <returns>field by the <paramref name="name"/> and <paramref name="value"/>.</returns>
        public static Field GetField(string name, string value, bool analyzed = DefautlAnalyzed, bool stored = DefaultStored)
        {
            var fieldStore = stored ? Field.Store.YES : Field.Store.NO;
            var fieldAnalyze = analyzed ? Field.Index.ANALYZED : Field.Index.NOT_ANALYZED;
            return new Field(name, String.IsNullOrEmpty(value) ? String.Empty : value, fieldStore, fieldAnalyze);
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
