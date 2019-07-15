using System.ComponentModel;
using Neurotoxin.ScOut.Data.Attributes;
using Neurotoxin.ScOut.Data.Entities;

namespace Neurotoxin.ScOut.Data.Relations
{
    [DisplayName("Business Owner")]
    [Icon("link.png")]
    [AllowedTargetEntityType(typeof(PersonEntity), typeof(GroupEntity))]
    public class BusinessOwnerRelation : RelationBase
    {
    }
}