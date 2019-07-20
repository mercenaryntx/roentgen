using System.ComponentModel;
using Neurotoxin.ScOut.Data.Attributes;

namespace Neurotoxin.ScOut.Data.Entities
{
    [Icon("project.png")]
    [DisplayName("Project")]
    public class ProjectEntity : FileEntityBase
    {
        public string Language { get; set; }
        public string TargetFramework { get; set; }
    }
}