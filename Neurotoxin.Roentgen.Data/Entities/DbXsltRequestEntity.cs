using System.ComponentModel;
using Neurotoxin.Roentgen.Data.Attributes;

namespace Neurotoxin.Roentgen.Data.Entities
{
    [Icon("dbxslt.png")]
    [DisplayName("DBXSLT Request")]
    public class DbXsltRequestEntity : EntityBase    
    {
        public string GroupName { get; set; }
        public string ObjectKeyName { get; set; }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (GroupName != null ? GroupName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ObjectKeyName != null ? ObjectKeyName.GetHashCode() : 0);
                return hashCode;
            }
        }

        protected bool Equals(DbXsltRequestEntity other)
        {
            return base.Equals(other) && string.Equals(GroupName, other.GroupName) && string.Equals(ObjectKeyName, other.ObjectKeyName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DbXsltRequestEntity) obj);
        }
    }
}
