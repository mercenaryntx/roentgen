using System.ComponentModel;
using Neurotoxin.ScOut.Data.Attributes;

namespace Neurotoxin.ScOut.Data.Relations
{
    [DisplayName("Primary Key Relation")]
    [Icon("link.png")]
    public class PrimaryKeyRelation : RelationBase
    {
        public string PrimaryKeyName { get; set; }
    }
}