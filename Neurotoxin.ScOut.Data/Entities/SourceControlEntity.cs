using System.ComponentModel;
using Neurotoxin.ScOut.Data.Attributes;
using Neurotoxin.ScOut.Data.Constants;

namespace Neurotoxin.ScOut.Data.Entities
{
    [Icon("server.png")]
    [DisplayName("Source Control Server")]
    public class SourceControlEntity : EntityBase
    {
        public SourceControlType SourceControlType { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((SourceControlEntity)obj);
        }

        protected bool Equals(SourceControlEntity other)
        {
            return base.Equals(other) && SourceControlType == other.SourceControlType;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ (int)SourceControlType;
            }
        }

    }
}
