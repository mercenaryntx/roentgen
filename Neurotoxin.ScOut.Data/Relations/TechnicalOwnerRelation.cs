using System.ComponentModel;
using Neurotoxin.ScOut.Data.Attributes;
using Neurotoxin.ScOut.Data.Entities;

namespace Neurotoxin.ScOut.Data.Relations
{
    [DisplayName("Technical Owner")]
    [Icon("link.png")]
    [AllowedTargetEntityType(typeof(PersonEntity), typeof(GroupEntity))]
    public class TechnicalOwnerRelation : RelationBase
    {
    }
}