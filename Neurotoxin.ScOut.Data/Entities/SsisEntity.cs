using System.ComponentModel;
using Neurotoxin.ScOut.Data.Attributes;

namespace Neurotoxin.ScOut.Data.Entities
{
    [Icon("ssis.png")]
    [DisplayName("SSIS package")]
    [AllowedParentEntityType(typeof(SystemEntity))]
    public class SsisEntity : ProcessEntity
    {
    }
}