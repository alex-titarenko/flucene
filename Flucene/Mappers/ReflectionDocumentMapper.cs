using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Lucene.Net.Documents;
using Lucene.Net.Odm.Helpers;
using Lucene.Net.Odm.Mapping;
using Lucene.Net.Odm.Mapping.Configuration;


namespace Lucene.Net.Odm.Mappers
{
    public class ReflectionDocumentMapper : IDocumentMapper
    {
        public Document GetDocument<TModel>(DocumentMapping<TModel> mapping, TModel model, IMappingsService mappingService)
        {
            Document doc = new Document();
            
            // Adds mapped fields to document
            foreach (PropertyMapping item in mapping.PropertyMappings)
            {
                object propertyValue = item.PropertyInfo.GetValue(model, null);
                if (propertyValue != null)
                {
                    IEnumerable<Fieldable> fields = item.FieldConfiguration.GetFields(propertyValue);
                    foreach (Fieldable field in fields)
                    {
                        doc.Add(field);
                    }
                }
            }

            // Adds custom fields to document.
            foreach (CustomMapping<TModel> item in mapping.CustomMappings)
            {
                Func<TModel, object> selector = item.Selector;
                foreach (Fieldable field in item.FieldConfiguration.GetFields(selector(model)))
                {
                    doc.Add(field);
                }
            }

            if (mappingService != null)
            {
                foreach (KeyValuePair<PropertyInfo, IReferenceConfiguration> item in mapping.ReferenceMappings)
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

            // Custom actions
            foreach (CustomAction<TModel> customAction in mapping.CustomActions)
            {
                customAction.ToDocument(model, doc);
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

            foreach (PropertyMapping item in mapping.PropertyMappings)
            {
                string fieldName = item.FieldConfiguration.FieldName;
                string[] fieldValues = document.GetValues(fieldName);

                if (fieldValues != null && fieldValues.Length > 0)
                {
                    item.PropertyInfo.SetValue(model, DataHelper.Parse(fieldValues, item.PropertyInfo.PropertyType), null);
                }
            }

            foreach (CustomMapping<TModel> item in mapping.CustomMappings)
            {
                Action<TModel, IEnumerable<string>> setter = item.Setter;
                
                if (setter != null)
                {
                    string fieldName = item.FieldConfiguration.FieldName;
                    string[] fieldValues = document.GetValues(fieldName);
                    if (fieldValues != null && fieldValues.Length > 0)
                    {
                        setter(model, fieldValues);
                    }
                }
            }

            if (mappingService != null)
            {
                foreach (KeyValuePair<PropertyInfo, IReferenceConfiguration> item in mapping.ReferenceMappings)
                {
                    object subModel = mappingService.GetModel(document, item.Key.PropertyType);
                    item.Key.SetValue(model, subModel, null);
                }
            }

            // Custom actions
            foreach (CustomAction<TModel> customAction in mapping.CustomActions)
            {
                if (customAction.ToModel != null)
                {
                    customAction.ToModel(document, model);
                }
            }

            return model;
        }
    }
}
