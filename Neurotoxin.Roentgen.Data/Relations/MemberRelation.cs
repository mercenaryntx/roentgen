using System.ComponentModel;
using Neurotoxin.Roentgen.Data.Attributes;
using Neurotoxin.Roentgen.Data.Entities;

namespace Neurotoxin.Roentgen.Data.Relations
{
    [DisplayName("Group Member")]
    [Icon("link.png")]
    [AllowedParentEntityType(typeof(GroupEntity))]
    [AllowedTargetEntityType(typeof(PersonEntity))]
    public class MemberRelation : RelationBase
    {
    }
}
