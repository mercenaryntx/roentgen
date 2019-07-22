using System.ComponentModel;
using Neurotoxin.Roentgen.Data.Attributes;

namespace Neurotoxin.Roentgen.Data.Entities
{
    [Icon("ssis.png")]
    [DisplayName("SSIS package")]
    [AllowedParentEntityType(typeof(SystemEntity))]
    public class SsisEntity : ProcessEntity
    {
    }
}