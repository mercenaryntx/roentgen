using System.ComponentModel;
using Neurotoxin.ScOut.Data.Attributes;
using Neurotoxin.ScOut.Data.Entities;

namespace Neurotoxin.ScOut.Data.Relations
{
    [DisplayName("Source Control")]
    [Icon("link.png")]
    [AllowedTargetEntityType(typeof(SourceControlEntity))]
    public class SourceControlRelation : RelationBase
    {
        public string SourceControlPath { get; set; }
    }
}