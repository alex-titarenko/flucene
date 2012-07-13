using System;

using Lucene.Net.Documents;


namespace Lucene.Net.Odm.Mapping
{
    public class CustomAction<TModel>
    {
        public Action<TModel, Document> ToDocument { get; set; }

        public Action<Document, TModel> ToModel { get; set; }


        public CustomAction(Action<TModel, Document> toDocument, Action<Document, TModel> toModel)
        {
            ToDocument = toDocument;
            ToModel = toModel;
        }
    }
}
