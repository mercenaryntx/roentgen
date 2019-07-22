using System;

namespace Neurotoxin.Roentgen.Data.Attributes
{
    public class AllowedTargetEntityTypeAttribute : Attribute
    {
        public Type[] EntityType { get; private set; }

        public AllowedTargetEntityTypeAttribute(params Type[] entityType)
        {
            EntityType = entityType;
        }
    }
}