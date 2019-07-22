using System.ComponentModel;
using Neurotoxin.Roentgen.Data.Attributes;
using Neurotoxin.Roentgen.Data.Entities;

namespace Neurotoxin.Roentgen.Data.Relations
{
    [DisplayName("SQL Insert")]
    [Icon("link.png")]
    [AllowedTargetEntityType(typeof(TableEntity))]
    public class InsertRelation : RelationBase
    {
    }
}
