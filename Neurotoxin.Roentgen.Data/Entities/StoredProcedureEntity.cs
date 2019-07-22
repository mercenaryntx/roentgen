using System.ComponentModel;
using Neurotoxin.Roentgen.Data.Attributes;

namespace Neurotoxin.Roentgen.Data.Entities
{
    [Icon("sp.png")]
    [DisplayName("Stored Procedure")]
    [AllowedParentEntityType(typeof(DatabaseEntity))]
    public class StoredProcedureEntity : CodeEntityBase
    {
    }
}
