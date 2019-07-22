using System.ComponentModel;
using Neurotoxin.Roentgen.Data.Attributes;
using Neurotoxin.Roentgen.Data.Entities;

namespace Neurotoxin.Roentgen.Data.Relations
{
    [DisplayName("Business Group")]
    [Icon("link.png")]
    [AllowedTargetEntityType(typeof(GroupEntity))]
    public class BusinessGroupRelation : RelationBase
    {
    }
}
