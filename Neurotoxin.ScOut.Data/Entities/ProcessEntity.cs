using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Neurotoxin.ScOut.Data.Attributes;

namespace Neurotoxin.ScOut.Data.Entities
{
    [Icon("process.png")]
    [DisplayName("Process")]
    [AllowedParentEntityType(typeof(SystemEntity))]
    public class ProcessEntity : EntityBase
    {
        [Column(TypeName = "ntext")]
        [MaxLength]
        public string ProcessSteps { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ProcessEntity)obj);
        }

        protected bool Equals(ProcessEntity other)
        {
            return base.Equals(other) && string.Equals(ProcessSteps, other.ProcessSteps);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode()*397) ^ (ProcessSteps != null ? ProcessSteps.GetHashCode() : 0);
            }
        }
    }
}