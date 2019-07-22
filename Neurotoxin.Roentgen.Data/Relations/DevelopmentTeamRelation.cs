using System.ComponentModel;
using Neurotoxin.Roentgen.Data.Attributes;
using Neurotoxin.Roentgen.Data.Entities;

namespace Neurotoxin.Roentgen.Data.Relations
{
    [DisplayName("Development Team")]
    [Icon("link.png")]
    [AllowedTargetEntityType(typeof(GroupEntity))]
    public class DevelopmentTeamRelation : RelationBase
    {
    }
}