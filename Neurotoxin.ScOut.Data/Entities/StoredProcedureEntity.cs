using System.ComponentModel;
using Neurotoxin.ScOut.Data.Attributes;

namespace Neurotoxin.ScOut.Data.Entities
{
    [Icon("sp.png")]
    [DisplayName("Stored Procedure")]
    [AllowedParentEntityType(typeof(DatabaseEntity))]
    public class StoredProcedureEntity : CodeEntityBase
    {
    }
}
