using System.ComponentModel;
using Neurotoxin.Roentgen.Data.Attributes;

namespace Neurotoxin.Roentgen.Data.Relations
{
    [DisplayName("Primary Key Relation")]
    [Icon("link.png")]
    public class PrimaryKeyRelation : RelationBase
    {
        public string PrimaryKeyName { get; set; }
    }
}