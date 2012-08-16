using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Lucene.Net.Documents;
using Lucene.Net.Odm.Helpers;
using Lucene.Net.Odm.Mapping;
using Lucene.Net.Odm.Mapping.Configuration;
using Lucene.Net.Odm.Mapping.Members;


namespace Lucene.Net.Odm.Mappers
{
    public class ReflectionDocumentMapper : IDocumentMapper
    {
        public Document GetDocument<TModel>(DocumentMapping<TModel> mapping, TModel model, IMappingsService mappingService, string prefix = null)
        {
            Document doc = new Document();
            
            // Adds mapped fields to document
            foreach (FieldMapping item in mapping.Fields)
            {
                object propertyValue = item.Member.GetValue(model);
                
                if (item.IsRequired && propertyValue == null)
                    throw new ArgumentNullException();

                string fieldName = prefix + item.FieldName;
                if (propertyValue != null)
                {
                    IEnumerable<Fieldable> fields = MappingHelper.GetFields(item, propertyValue, prefix);
                    if (fields.Count() > 1)
                        doc.AddItemsCount(fieldName, fields.Count());
                    foreach (Fieldable field in fields)
                    {
                        doc.Add(field);
                    }
                }
                else
                {
                    doc.AddEmpty(fieldName);
                }
            }

            if (mappingService != null)
            {
                foreach (EmbeddedMapping embeddedItem in mapping.Embedded)
                {
                    dynamic propertyValue = embeddedItem.Member.GetValue(model);
                    if (propertyValue != null)
                    {
                        string newPrefix = GetNewPrefix(prefix, embeddedItem);
                        if (DataHelper.IsGenericEnumerable(propertyValue.GetType()))
                        {
                            string fieldName = prefix + GetPropertyName(embeddedItem.Member);
                            int count = Enumerable.Count(propertyValue);

                            doc.AddItemsCount(fieldName, count);
                            foreach (dynamic item in propertyValue)
                            {
                                AddEmbeddedFields(doc, item, mappingService, newPrefix);
                            }
                        }
                        else
                        {
                            AddEmbeddedFields(doc, propertyValue, mappingService, newPrefix);
                        }
                    }
                }
            }

            // Custom actions
            foreach (CustomAction<TModel> customAction in mapping.CustomActions.Where(x => x.ToDocument != null))
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

        public TModel GetModel<TModel>(DocumentMapping<TModel> mapping, Document document, IMappingsService mappingService, string prefix = null) where TModel : new()
        {
            TModel model = new TModel();

            foreach (FieldMapping item in mapping.Fields.Where(x => x.Member.CanWrite))
            {
                string fieldName = prefix + item.FieldName;

                if (item.Member.IsEnumerable)
                {
                    IList<string> fieldValues = document.ExtractValues(fieldName);

                    if (item.IsRequired && (fieldValues.Count == 0))
                        throw new ArgumentException();

                    if (fieldValues.Count > 0)
                    {
                        item.Member.SetValue(model, DataHelper.Parse(fieldValues, item.Member.MemberType));
                    }
                }
                else
                {
                    string fieldValue = document.Extract(fieldName);
                    if (item.IsRequired && String.IsNullOrEmpty(fieldValue))
                        throw new ArgumentException();

                    if (!String.IsNullOrEmpty(fieldValue))
                    {
                        item.Member.SetValue(model, DataHelper.Parse(fieldValue, item.Member.MemberType));
                    }
                }
            }

            if (mappingService != null)
            {
                foreach (EmbeddedMapping item in mapping.Embedded)
                {
                    string fieldName = GetNewPrefix(prefix, item);
                    object subModel = null;

                    if (item.Member.IsEnumerable)
                    {
                        CollectionMember collMember = (CollectionMember)item.Member;
                        int count = document.ExtractItemsCount(prefix + GetPropertyName(item.Member));

                        IList list = DataHelper.MakeGenericList(collMember.MemberType, collMember.CollectionType);
                        for (int i = 0; i < count; i++)
                        {
                            list.Add(mappingService.GetModel(document, collMember.CollectionType, fieldName));
                        }
                        subModel = list;
                    }
                    else
                    {
                        subModel = mappingService.GetModel(document, item.Member.MemberType, fieldName);
                    }
                    
                    item.Member.SetValue(model, subModel);
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


        private static void AddEmbeddedFields(Document doc, dynamic model, IMappingsService mappingService, string prefix)
        {
            Document subDoc = mappingService.GetDocument(model, prefix);
            IList fields = subDoc.GetFields();
            if (fields.Count == 1)
            {
                Fieldable field = fields[0] as Fieldable;
                field.SetBoost(subDoc.GetBoost());
                doc.Add(field);
            }
            else
            {
                foreach (Fieldable subField in fields)
                {
                    doc.Add(subField);
                }
            }
        }

        #region Helpers

        private static string GetPropertyName(Member member)
        {
            if (member is PropertyMember)
            {
                return ((PropertyMember)member).PropertyInfo.Name;
            }
            else if (member is CollectionMember)
            {
                return GetPropertyName(((CollectionMember)member).BaseMember);
            }
            else
            {
                return String.Empty;
            }
        }

        private static string GetNewPrefix(string basePrefix, EmbeddedMapping mapping)
        {
            return basePrefix + (mapping.Prefix ?? GetDefaultPrefix(mapping.Member));
        }

        private static string GetDefaultPrefix(Member member)
        {
            return (member is PropertyMember) ? ((PropertyMember)member).PropertyInfo.Name + "." : String.Empty;
        }

        #endregion
    }
}
