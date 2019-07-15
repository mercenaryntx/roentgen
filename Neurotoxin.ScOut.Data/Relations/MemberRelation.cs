using System.ComponentModel;
using Neurotoxin.ScOut.Data.Attributes;
using Neurotoxin.ScOut.Data.Entities;

namespace Neurotoxin.ScOut.Data.Relations
{
    [DisplayName("Group Member")]
    [Icon("link.png")]
    [AllowedParentEntityType(typeof(GroupEntity))]
    [AllowedTargetEntityType(typeof(PersonEntity))]
    public class MemberRelation : RelationBase
    {
    }
}
