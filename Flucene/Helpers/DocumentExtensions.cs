using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

using Lucene.Net.Documents;


namespace Lucene.Net.Odm.Helpers
{
    /// <summary>
    /// Represents the extension methods for Document.
    /// </summary>
    public static class DocumentExtensions
    {
        private const string ItemsCountFieldSuffix = "_COUNT";


        /// <summary>
        /// Returns a field value and remove field from document by specified field name.
        /// </summary>
        /// <param name="source">The target document.</param>
        /// <param name="name">A string represents the field name.</param>
        /// <returns>string represents the field value.</returns>
        public static string Extract(this Document source, string name)
        {
            string value = source.GetInvariant(name);
            source.RemoveField(name);
            return value;
        }

        /// <summary>
        /// Returns all field values and remove fields from document by specified field name.
        /// </summary>
        /// <param name="source">The target document.</param>
        /// <param name="name">A string represents the field name.</param>
        /// <returns>string collection represents the field values.</returns>
        public static IList<string> ExtractValues(this Document source, string name)
        {
            int count = source.ExtractItemsCount(name);

            IList<string> values = new List<string>(count);
            IList<IFieldable> fields = source.GetFields();
            
            for (int i = 0; i < fields.Count; i++)
            {
                var field = fields[i];
                if (name.Equals(field.Name))
                {
                    values.Add(field.InvariantStringValue());
                    fields.Remove(field);
                    i--;
                }
            }

            return values;
        }

        /// <summary>
        /// Returns the field value for invariant culture.
        /// </summary>
        /// <param name="source">The target document.</param>
        /// <param name="name">A string represents the field name.</param>
        /// <returns>string representing field value for invariant culture.</returns>
        public static string GetInvariant(this Document source, string name)
        {
            IFieldable fieldable = source.GetFieldable(name);

            if (fieldable != null)
                return fieldable.InvariantStringValue();
            else
                return null;
        }

        /// <summary>
        /// Returns the field value for invariant culture.
        /// </summary>
        /// <param name="source">The target field.</param>
        /// <returns>string representing field value for invariant culture.</returns>
        public static string InvariantStringValue(this IFieldable source)
        {
            if (source is NumericField)
            {
                return ((IFormattable)((NumericField)source).NumericValue)
                    .ToString(null, CultureInfo.InvariantCulture);
            }
            else
            {
                return source.StringValue;
            }
        }

        /// <summary>
        /// Adds empty field in the document.
        /// </summary>
        /// <param name="source">The target document.</param>
        /// <param name="name">A string represents the field name.</param>
        public static void AddEmpty(this Document source, string name)
        {
            source.Add(new Field(name, String.Empty, Field.Store.YES, Field.Index.NO));
        }

        /// <summary>
        /// Adds the special field that contains the items count of the list.
        /// </summary>
        /// <param name="source">The target document.</param>
        /// <param name="name">A string represents the field name.</param>
        /// <param name="count">An integer containing the number of items in a list.</param>
        public static void AddItemsCount(this Document source, string name, int count)
        {
            source.Add(new NumericField(GetItemsCountFieldName(name), Field.Store.YES, false).SetIntValue(count));
        }

        /// <summary>
        /// Returns the items count field value and remove the field from document.
        /// </summary>
        /// <param name="source">The target document.</param>
        /// <param name="name">A string represents the field name.</param>
        /// <returns>integer containing the number of items in a list.</returns>
        public static int ExtractItemsCount(this Document source, string name)
        {
            string numFieldName = GetItemsCountFieldName(name);
            
            var field = source.GetFieldable(numFieldName);

            if (field != null)
            {
                int count = int.Parse(field.StringValue);
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
