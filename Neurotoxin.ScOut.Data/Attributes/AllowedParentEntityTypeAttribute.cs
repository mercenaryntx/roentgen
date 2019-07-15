using System;

namespace Neurotoxin.ScOut.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AllowedParentEntityTypeAttribute : Attribute
    {
        public Type EntityType { get; private set; }

        public AllowedParentEntityTypeAttribute(Type entityType)
        {
            EntityType = entityType;
        }
    }
}
