using System.ComponentModel;
using Neurotoxin.ScOut.Data.Attributes;
using Neurotoxin.ScOut.Data.Entities;

namespace Neurotoxin.ScOut.Data.Relations
{
    [DisplayName("SQL Insert")]
    [Icon("link.png")]
    [AllowedTargetEntityType(typeof(TableEntity))]
    public class InsertRelation : RelationBase
    {
    }
}
