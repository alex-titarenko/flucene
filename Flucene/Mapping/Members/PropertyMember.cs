using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Lucene.Net.Odm.Mapping.Members
{
    public class PropertyMember : Member
    {
        public PropertyInfo PropertyInfo { get; private set; }


        public PropertyMember(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
        }

        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }

        public override Type MemberType
        {
            get
            {
                return PropertyInfo.PropertyType;
            }
        }

        public override object GetValue<TTarget>(TTarget target)
        {
            return PropertyInfo.GetValue(target, null);
        }

        public override void SetValue<TTarget, TValue>(TTarget target, TValue value)
        {
            PropertyInfo.SetValue(target, value, null);
        }
    }
}
