using System.ComponentModel;
using Neurotoxin.Roentgen.Data.Attributes;
using Neurotoxin.Roentgen.Data.Entities;

namespace Neurotoxin.Roentgen.Data.Relations
{
    [DisplayName("SQL Update")]
    [Icon("link.png")]
    [AllowedTargetEntityType(typeof(ColumnEntity))]
    public class UpdateRelation : RelationBase
    {
    }
}
