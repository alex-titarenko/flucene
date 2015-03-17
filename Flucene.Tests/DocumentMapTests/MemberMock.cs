using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Odm.Mapping.Members;

namespace Lucene.Net.Orm.Tests.DocumentMapTests
{
    public class MemberMock : Member
    {
        protected readonly bool _canWrite;
        protected readonly Type _memberType;

        public MemberMock(bool canWrite, Type memberType)
        {
            _canWrite = canWrite;
            _memberType = memberType;
        }

        public override bool CanWrite
        {
            get { return _canWrite; }
        }

        public override Type MemberType
        {
            get { return _memberType; }
        }

        public override object GetValue<TTarget>(TTarget target)
        {
            throw new NotImplementedException();
        }

        public override void SetValue<TTarget, TValue>(TTarget target, TValue value)
        {
            throw new NotImplementedException();
        }
    }
}
