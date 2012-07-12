using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

using Lucene.Net.Documents;


namespace Lucene.Net.Orm.Mapping.Configuration
{
    public class FieldConfiguration<TInput> : IFieldConfiguration<TInput>, IFieldConfiguration
    {
        private const int DefaultPrecisionStep = 64;

        protected Boosting<TInput> _boost;
        protected Field.Index _index = Field.Index.NOT_ANALYZED;
        protected Field.Store _store = Field.Store.YES;


        public FieldConfiguration(string fieldName)
        {
            FieldName = fieldName;
        }


        #region IFieldConfigurationFluent Members

        public string FieldName { get; set; }


        IFieldConfiguration IFieldConfiguration.Analyze()
        {
            return Analyze();
        }

        IFieldConfiguration IFieldConfiguration.NoNormsAnalyze()
        {
            return NoNormsAnalyze();
        }

        IFieldConfiguration IFieldConfiguration.NotIndex()
        {
            return NotIndex();
        }

        IFieldConfiguration IFieldConfiguration.NotAnalyze()
        {
            return NotAnalyze();
        }

        IFieldConfiguration IFieldConfiguration.NoNormsNotAnalyze()
        {
            return NoNormsAnalyze();
        }


        IFieldConfiguration IFieldConfiguration.Store()
        {
            return Store();
        }

        IFieldConfiguration IFieldConfiguration.Compress()
        {
            return Compress();
        }

        IFieldConfiguration IFieldConfiguration.NotStore()
        {
            return NotStore();
        }


        IFieldConfiguration IFieldConfiguration.Boost(Boosting<object> boost)
        {
            return Boost((TInput input) => boost(input));
        }


        public IEnumerable<Fieldable> GetFields(Object value)
        {
            if (value == null) return null;

            IEnumerable<AbstractField> fields = GetFieldsInternal(value);

            if (_boost != null)
            {
                foreach (AbstractField field in fields)
                {
                    field.SetBoost(_boost((TInput)value));
                }
            }

            return fields;
        }

        internal IEnumerable<AbstractField> GetFieldsInternal(Object value)
        {
            AbstractField field = null;

            if (value is String ||
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
                List<AbstractField> fields = new List<AbstractField>();
                foreach (object obj in values)
                {
                    fields.AddRange(GetFieldsInternal(obj));
                }

                return fields;
            }
            else
            {
                field = CreateNumericField((ValueType)value);
            }

            return new AbstractField[] { field };
        }

        #endregion

        #region IFieldConfigurationFluent<TInput> Members

        public IFieldConfiguration<TInput> Analyze()
        {
            _index = Field.Index.ANALYZED;
            return this;
        }

        public IFieldConfiguration<TInput> NoNormsAnalyze()
        {
            _index = Field.Index.ANALYZED_NO_NORMS;
            return this;
        }

        public IFieldConfiguration<TInput> NotIndex()
        {
            _index = Field.Index.NO;
            return this;
        }

        public IFieldConfiguration<TInput> NotAnalyze()
        {
            _index = Field.Index.NOT_ANALYZED;
            return this;
        }

        public IFieldConfiguration<TInput> NoNormsNotAnalyze()
        {
            _index = Field.Index.NOT_ANALYZED_NO_NORMS;
            return this;
        }


        public IFieldConfiguration<TInput> Store()
        {
            _store = Field.Store.YES;
            return this;
        }

        public IFieldConfiguration<TInput> Compress()
        {
            _store = Field.Store.COMPRESS;
            return this;
        }

        public IFieldConfiguration<TInput> NotStore()
        {
            _store = Field.Store.NO;
            return this;
        }


        public IFieldConfiguration<TInput> Boost(Boosting<TInput> boost)
        {
            _boost = boost;
            return this;
        }

        #endregion


        private AbstractField CreateNumericField(ValueType value)
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
            else if (value is decimal)
                numField.SetDoubleValue((double)(decimal)value);
            else if (value is DateTime)
                numField.SetLongValue(((DateTime)value).ToBinary());
            else
                throw new Exception(String.Format(Properties.Resources.EXC_TYPE_NOT_SUPPORTED, value));

            return numField;
        }
    }
}
