using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

using Lucene.Net.Documents;


namespace Lucene.Net.Orm.Mapping
{
    public class FieldConfiguration : IFieldConfiguration
    {
        private const int DefaultPrecisionStep = 64;

        protected float _boost = 1.0f;
        protected Field.Index _index = Field.Index.NOT_ANALYZED;
        protected Field.Store _store = Field.Store.YES;


        public FieldConfiguration(string fieldName)
        {
            FieldName = fieldName;
        }


        #region IFieldConfigurationFluent Members

        public string FieldName { get; set; }


        public IFieldConfiguration Analyze()
        {
            _index = Field.Index.ANALYZED;
            return this;
        }

        public IFieldConfiguration NoNormsAnalyze()
        {
            _index = Field.Index.ANALYZED_NO_NORMS;
            return this;
        }

        public IFieldConfiguration NotIndex()
        {
            _index = Field.Index.NO;
            return this;
        }

        public IFieldConfiguration NotAnalyze()
        {
            _index = Field.Index.NOT_ANALYZED;
            return this;
        }

        public IFieldConfiguration NoNormsNotAnalyze()
        {
            _index = Field.Index.NOT_ANALYZED_NO_NORMS;
            return this;
        }


        public IFieldConfiguration Store()
        {
            _store = Field.Store.YES;
            return this;
        }

        public IFieldConfiguration Compress()
        {
            _store = Field.Store.COMPRESS;
            return this;
        }

        public IFieldConfiguration NotStore()
        {
            _store = Field.Store.NO;
            return this;
        }


        public IFieldConfiguration Boost(float boost)
        {
            _boost = boost;
            return this;
        }

        public IEnumerable<Fieldable> GetFields(object value)
        {
            if (value == null) return null;

            AbstractField field = null;

            if (value is String ||
                value is DateTime ||
                value is Enum ||
                value is Boolean)
            {
                string strValue = (value is IFormattable) ?
                    ((IFormattable)value).ToString(null, CultureInfo.InvariantCulture) : value.ToString();

                field = new Field(FieldName, strValue, _store, _index);
            }
            else if (value is IEnumerable)
            {
                IEnumerable values = value as IEnumerable;
                List<Fieldable> fields = new List<Fieldable>();
                foreach (object obj in values)
                {
                    fields.AddRange(GetFields(obj));
                }

                return fields;
            }
            else
            {
                NumericField numField = new NumericField(
                    FieldName, DefaultPrecisionStep,
                    _store, _index == Field.Index.ANALYZED);

                if (value is int)
                    numField.SetIntValue((int)value);
                else if (value is long)
                    numField.SetLongValue((long)value);
                else if (value is float)
                    numField.SetFloatValue((float)value);
                else if (value is double)
                    numField.SetDoubleValue((double)value);
                else
                    throw new Exception(String.Format("'{0}' data type is not supported", value));

                field = numField;
            }

            field.SetBoost(_boost);
            return new Fieldable[] { field };
        }

        #endregion
    }
}
