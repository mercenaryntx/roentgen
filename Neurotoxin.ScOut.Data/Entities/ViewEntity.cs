using System.ComponentModel;
using Neurotoxin.ScOut.Data.Attributes;

namespace Neurotoxin.ScOut.Data.Entities
{
    [Icon("table.png")]
    [DisplayName("View")]
    [AllowedParentEntityType(typeof(DatabaseEntity))]
    public class ViewEntity : EntityBase
    {
    }
}