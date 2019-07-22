using System.ComponentModel;
using Neurotoxin.Roentgen.Data.Attributes;
using Neurotoxin.Roentgen.Data.Entities;

namespace Neurotoxin.Roentgen.Data.Relations
{
    [DisplayName("Business Owner")]
    [Icon("link.png")]
    [AllowedTargetEntityType(typeof(PersonEntity), typeof(GroupEntity))]
    public class BusinessOwnerRelation : RelationBase
    {
    }
}