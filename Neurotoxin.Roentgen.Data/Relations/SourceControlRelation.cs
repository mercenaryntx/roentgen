using System.ComponentModel;
using Neurotoxin.Roentgen.Data.Attributes;
using Neurotoxin.Roentgen.Data.Entities;

namespace Neurotoxin.Roentgen.Data.Relations
{
    [DisplayName("Source Control")]
    [Icon("link.png")]
    [AllowedTargetEntityType(typeof(SourceControlEntity))]
    public class SourceControlRelation : RelationBase
    {
        public string SourceControlPath { get; set; }
    }
}