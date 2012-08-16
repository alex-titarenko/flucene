using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Lucene.Net.Documents;
using Lucene.Net.Odm.Helpers;
using Lucene.Net.Odm.Mapping.Configuration;
using Lucene.Net.Odm.Mapping.Providers;
using Lucene.Net.Odm.Mapping.Members;


namespace Lucene.Net.Odm.Mapping
{
    public class DocumentMap<TModel> : DocumentMapBase<TModel>, IDocumentMappingProvider<TModel> where TModel : new()
    {
        private ICollection<FieldConfiguration> _fieldConfigurations;
        private ICollection<EmbeddedConfiguration> _embededConfigurations;
        private ICollection<CustomAction<TModel>> _customActions;
        private Boosting<TModel> _boost;

        private IMemberFactory MemberFactory = new MemberFactory();


        public DocumentMap()
        {
            _fieldConfigurations = new List<FieldConfiguration>();
            _embededConfigurations = new List<EmbeddedConfiguration>();
            _customActions = new List<CustomAction<TModel>>();
        }


        protected override FieldConfiguration Map<TMember>(Expression<Func<TModel, TMember>> expression)
        {
            string propertyName = expression.GetPropertyName();
            return Map(expression, propertyName);
        }

        protected override FieldConfiguration Map<TMember>(Expression<Func<TModel, TMember>> expression, string fieldName)
        {
            FieldConfiguration field = new FieldConfiguration(fieldName, MemberFactory.GetMember(expression));
            _fieldConfigurations.Add(field);

            return field;
        }

        protected override FieldConfiguration Map<TMember>(Func<TModel, TMember> getter, Action<TModel, IEnumerable<string>> setter, string fieldName)
        {
            Member member = new CustomMember(getter, setter);
            FieldConfiguration field = new FieldConfiguration(fieldName, member);
            _fieldConfigurations.Add(field);

            return field;
        }

        protected override EmbeddedConfiguration Embedded<TEmbedded>(Expression<Func<TModel, TEmbedded>> property)
        {
            Member member = MemberFactory.GetMember(property);
            EmbeddedConfiguration embedded = new EmbeddedConfiguration(member);
            _embededConfigurations.Add(embedded);
            
            return embedded;
        }

        protected override EmbeddedCollectionConfiguration<TChild> HasMany<TChild>(Expression<Func<TModel, IEnumerable<TChild>>> property)
        {
            Member member = MemberFactory.GetMember(property);
            EmbeddedCollectionConfiguration<TChild> embedded = new EmbeddedCollectionConfiguration<TChild>(member);
            _embededConfigurations.Add(embedded);

            return embedded;
        }

        protected override void Custom(Action<TModel, Document> toDocument, Action<Document, TModel> toModel)
        {
            _customActions.Add(new CustomAction<TModel>(toDocument, toModel));
        }

        protected override void Boost(Boosting<TModel> documentBoost)
        {
            _boost = documentBoost;
        }


        #region IDocumentMappingProvider<TModel> Members

        public DocumentMapping<TModel> GetMapping()
        {
            DocumentMapping<TModel> mapping = new DocumentMapping<TModel>();

            foreach (var item in _fieldConfigurations)
            {
                mapping.Fields.Add(item.GetMapping());
            }

            foreach (var item in _embededConfigurations)
            {
                mapping.Embedded.Add(item.GetMapping());
            }

            mapping.CustomActions = _customActions;
            mapping.Boost = _boost;

            return mapping;
        }

        #endregion
    }
}
