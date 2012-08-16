using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lucene.Net.Odm.Mapping.Members
{
    public abstract class Member
    {
        public abstract bool CanWrite { get; }

        public abstract Type MemberType { get; }

        public abstract object GetValue<TTarget>(TTarget target);

        public abstract void SetValue<TTarget, TValue>(TTarget target, TValue value);


        private bool? _isEnumerable;
        public bool IsEnumerable
        {
            get
            {
                if (_isEnumerable == null)
                    _isEnumerable = Helpers.DataHelper.IsGenericEnumerable(MemberType);

                return _isEnumerable.Value;
            }
        }
    }
}
