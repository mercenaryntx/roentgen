using System.ComponentModel;
using Neurotoxin.Roentgen.Data.Attributes;

namespace Neurotoxin.Roentgen.Data.Entities
{
    [Icon("owner.png")]
    [DisplayName("Person")]
    public class PersonEntity : EntityBase
    {
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (Email != null ? Email.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Phone != null ? Phone.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Role != null ? Role.GetHashCode() : 0);
                return hashCode;
            }
        }

        protected bool Equals(PersonEntity other)
        {
            return base.Equals(other) && StringEquals(Email, other.Email) && StringEquals(Phone, other.Phone) && StringEquals(Role, other.Role);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PersonEntity) obj);
        }
    }
}