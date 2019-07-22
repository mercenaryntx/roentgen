using System.ComponentModel;

namespace Neurotoxin.Roentgen.Data.Entities
{
    [DisplayName("Parameter")]
    public class ParameterEntity : ColumnEntity
    {
        public bool IsOutput { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ParameterEntity)obj);
        }

        protected bool Equals(ParameterEntity other)
        {
            return base.Equals(other) && IsOutput.Equals(other.IsOutput);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode()*397) ^ IsOutput.GetHashCode();
            }
        }
    }
}