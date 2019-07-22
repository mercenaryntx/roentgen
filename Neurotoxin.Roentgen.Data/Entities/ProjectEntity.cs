using System.ComponentModel;
using Neurotoxin.Roentgen.Data.Attributes;

namespace Neurotoxin.Roentgen.Data.Entities
{
    [Icon("project.png")]
    [DisplayName("Project")]
    public class ProjectEntity : FileEntityBase
    {
        public string Language { get; set; }
        public string TargetFramework { get; set; }
    }
}