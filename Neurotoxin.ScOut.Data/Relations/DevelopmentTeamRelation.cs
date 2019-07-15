using System.ComponentModel;
using Neurotoxin.ScOut.Data.Attributes;
using Neurotoxin.ScOut.Data.Entities;

namespace Neurotoxin.ScOut.Data.Relations
{
    [DisplayName("Development Team")]
    [Icon("link.png")]
    [AllowedTargetEntityType(typeof(GroupEntity))]
    public class DevelopmentTeamRelation : RelationBase
    {
    }
}