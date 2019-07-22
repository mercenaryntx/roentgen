using System.ComponentModel;
using Neurotoxin.Roentgen.Data.Attributes;

namespace Neurotoxin.Roentgen.Data.Entities
{
    [Icon("table.png")]
    [DisplayName("Table")]
    [AllowedParentEntityType(typeof(DatabaseEntity))]
    public class TableEntity : EntityBase
    {
        public int RowCount { get; set; }
    }
}