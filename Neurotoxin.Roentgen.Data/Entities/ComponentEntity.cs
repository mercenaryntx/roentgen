using System.ComponentModel;
using Neurotoxin.Roentgen.Data.Attributes;
using Neurotoxin.Roentgen.Data.Constants;

namespace Neurotoxin.Roentgen.Data.Entities
{
    [Icon("component.png")]
    [DisplayName("Component")]
    [AllowedParentEntityType(typeof(SystemEntity))]
    [AllowedParentEntityType(typeof(ComponentEntity))]
    public class ComponentEntity : EntityBase
    {
        public string Purpose { get; set; }
        public ComponentType ComponentType { get; set; }
        public LogicalSystems LogicalSystem { get; set; }
        
        public string Category { get; set; }
        public TransportTypes Transport { get; set; }
        public DataFormats DataFormat { get; set; }
        public string Frequency { get; set; }
        public SecurityTypes Security { get; set; }
        public AccessStrategies AccessStrategy { get; set; }
        public string Notes { get; set; }
        public string Interface { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ComponentEntity)obj);
        }

        protected bool Equals(ComponentEntity other)
        {
            return base.Equals(other) && 
                   string.Equals(Purpose, other.Purpose) && 
                   ComponentType == other.ComponentType &&
                   LogicalSystem == other.LogicalSystem && 
                   string.Equals(Category, other.Category) && 
                   Transport == other.Transport &&
                   DataFormat == other.DataFormat && 
                   string.Equals(Frequency, other.Frequency) &&
                   Security == other.Security && 
                   AccessStrategy == other.AccessStrategy &&
                   string.Equals(Notes, other.Notes) &&
                   string.Equals(Interface, other.Interface);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode*397) ^ (Purpose != null ? Purpose.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (int) ComponentType;
                hashCode = (hashCode*397) ^ (int) LogicalSystem;
                hashCode = (hashCode*397) ^ (Category != null ? Category.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (int) Transport;
                hashCode = (hashCode*397) ^ (int) DataFormat;
                hashCode = (hashCode*397) ^ (Frequency != null ? Frequency.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (int) Security;
                hashCode = (hashCode*397) ^ (int) AccessStrategy;
                hashCode = (hashCode*397) ^ (Notes != null ? Notes.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Interface != null ? Interface.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}