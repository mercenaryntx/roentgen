using System.ComponentModel;
using Neurotoxin.ScOut.Data.Attributes;

namespace Neurotoxin.ScOut.Data.Entities
{
    [Icon("table.png")]
    [DisplayName("Table")]
    [AllowedParentEntityType(typeof(DatabaseEntity))]
    public class TableEntity : EntityBase
    {
        public int RowCount { get; set; }
    }
}