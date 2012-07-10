using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Lucene.Net.Documents;
using Lucene.Net.Orm.Helpers;
using Lucene.Net.Orm.Mapping;
using Lucene.Net.Orm.Mapping.Configuration;


namespace Lucene.Net.Orm.Mappers
{
    public class ReflectionDocumentMapper : IDocumentMapper
    {
        public Document GetDocument<TModel>(DocumentMapping<TModel> mapping, TModel model, IMappingsService mappingService)
        {
            Document doc = new Document();

            // Adds mapped fields to document
            foreach (KeyValuePair<PropertyInfo, IFieldConfiguration> item in mapping.PropertyMaps)
            {
                object propertyValue = item.Key.GetValue(model, null);
                if (propertyValue != null)
                {
                    IEnumerable<Fieldable> fields = item.Value.GetFields(propertyValue);
                    foreach (Fieldable field in fields)
                    {
                        doc.Add(field);
                    }
                }
            }

            // Adds custom fields to document.
            foreach (KeyValuePair<CustomMap<TModel>, IFieldConfiguration> item in mapping.CustomMaps)
            {
                Func<TModel, object> selector = item.Key.Selector;
                foreach (Fieldable field in item.Value.GetFields(selector(model)))
                {
                    doc.Add(field);
                }
            }

            if (mappingService != null)
            {
                foreach (KeyValuePair<PropertyInfo, IReferenceConfiguration> item in mapping.References)
                {
                    dynamic propertyValue = item.Key.GetValue(model, null);
                    if (propertyValue != null)
                    {
                        Document subDoc = mappingService.GetDocument(propertyValue);
                        foreach (Fieldable subField in subDoc.GetFields())
                        {
                            doc.Add(subField);
                        }
                    }
                }
            }

            // Sets the document boosting
            if (mapping.Boost != null)
            {
                doc.SetBoost(mapping.Boost(model));
            }
            return doc;
        }

        public TModel GetModel<TModel>(DocumentMapping<TModel> mapping, Document document, IMappingsService mappingService) where TModel : new()
        {
            TModel model = new TModel();

            foreach (KeyValuePair<PropertyInfo, IFieldConfiguration> item in mapping.PropertyMaps)
            {
                string fieldName = item.Value.FieldName;
                string[] fieldValues = document.GetValues(fieldName);

                if (fieldValues != null && fieldValues.Length > 0)
                {
                    item.Key.SetValue(model, DataHelper.Parse(fieldValues, item.Key.PropertyType), null);
                }
            }

            foreach (KeyValuePair<CustomMap<TModel>, IFieldConfiguration> item in mapping.CustomMaps)
            {
                Action<TModel, IEnumerable<string>> setter = item.Key.Setter;
                if (setter != null)
                {
                    string fieldName = item.Value.FieldName;
                    string[] fieldValues = document.GetValues(fieldName);
                    if (fieldValues != null && fieldValues.Length > 0)
                    {
                        setter(model, fieldValues);
                    }
                }
            }

            if (mappingService != null)
            {
                foreach (KeyValuePair<PropertyInfo, IReferenceConfiguration> item in mapping.References)
                {
                    object subModel = mappingService.GetModel(document, item.Key.PropertyType);
                    item.Key.SetValue(model, subModel, null);
                }
            }

            return model;
        }
    }
}
