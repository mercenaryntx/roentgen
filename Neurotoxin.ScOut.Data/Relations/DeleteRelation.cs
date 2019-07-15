using System.ComponentModel;
using Neurotoxin.ScOut.Data.Attributes;
using Neurotoxin.ScOut.Data.Entities;

namespace Neurotoxin.ScOut.Data.Relations
{
    [DisplayName("SQL Delete")]
    [Icon("link.png")]
    [AllowedTargetEntityType(typeof(TableEntity))]
    public class DeleteRelation : RelationBase
    {
    }
}
