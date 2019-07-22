using System.ComponentModel;
using Neurotoxin.Roentgen.Data.Attributes;

namespace Neurotoxin.Roentgen.Data.Entities
{
    [Icon("table.png")]
    [DisplayName("View")]
    [AllowedParentEntityType(typeof(DatabaseEntity))]
    public class ViewEntity : EntityBase
    {
    }
}