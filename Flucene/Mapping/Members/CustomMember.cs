using System;
using System.Reflection;

namespace Lucene.Net.Odm.Mapping.Members
{
    public class CustomMember : Member
    {
        public Delegate Getter { get; private set; }

        public Delegate Setter { get; private set; }

        private MethodInfo _setterInfo;
        private dynamic _dynamicGetter;
        private Type _memberType;
        private object[] _params = new object[2];


        public CustomMember(Delegate getter, Delegate setter)
        {
            Getter = getter;
            _dynamicGetter = getter;

            Setter = setter;
            if (setter != null)
            {
                _setterInfo = setter.Method;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return Setter != null;
            }
        }

        public override Type MemberType
        {
            get
            {
                if (_memberType == null)
                {
                    if (Setter != null)
                        _memberType = Setter.Method.GetParameters()[1].ParameterType;
                }

                return _memberType;
            }
        }

        public override object GetValue<TTarget>(TTarget target)
        {
            return _dynamicGetter(target);
        }

        public override void SetValue<TTarget, TValue>(TTarget target, TValue value)
        {
            _params[0] = target;
            _params[1] = value;
            _setterInfo.Invoke(null, _params);
            //_setterInfo.Invoke(target, new object [] { value });
        }
    }
}
