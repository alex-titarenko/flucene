using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;


namespace Lucene.Net.Odm.Mapping.Members
{
    public abstract class CollectionMember : Member
    {
        public readonly Member BaseMember;
        public int ItemsCount { get; private set; }

        public CollectionMember(Member baseMember, int itemsCount)
        {
            BaseMember = baseMember;
            ItemsCount = itemsCount;
        }

        public override void SetValue<TTarget, TValue>(TTarget target, TValue value)
        {
            BaseMember.SetValue(target, value);
        }

        public override bool CanWrite
        {
            get
            {
                return BaseMember.CanWrite;
            }
        }

        public override Type MemberType
        {
            get
            {
                return BaseMember.MemberType;
            }
        }

        public abstract Type CollectionType
        {
            get;
        }
    }

    public class CollectionMember<TChild> : CollectionMember
    {
        public Func<TChild, bool> Predicate { get; private set; }

        public CollectionMember(Member baseMember, Func<TChild, bool> predicate, int itemsCount)
            : base(baseMember, itemsCount)
        {
            Predicate = predicate;
        }

        public override object GetValue<TTarget>(TTarget target)
        {
            if (Predicate != null)
                return ((IEnumerable<TChild>)BaseMember.GetValue<TTarget>(target)).Where(Predicate).Take(ItemsCount).ToList();
            else
                return ((IEnumerable<TChild>)BaseMember.GetValue<TTarget>(target)).Take(ItemsCount).ToList();
        }

        public override Type CollectionType
        {
            get
            {
                return typeof(TChild);
            }
        }
    }
}
