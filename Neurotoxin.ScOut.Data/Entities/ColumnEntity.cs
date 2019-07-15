using System.ComponentModel;
using Neurotoxin.ScOut.Data.Attributes;

namespace Neurotoxin.ScOut.Data.Entities
{
    [Icon("column.png")]
    [DisplayName("Column")]
    [AllowedParentEntityType(typeof(TableEntity))]
    public class ColumnEntity : EntityBase
    {
        public string DefaultValue { get; set; }
        public bool IsNullable { get; set; }
        public string DataType { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ColumnEntity)obj);
        }

        protected bool Equals(ColumnEntity other)
        {
            return base.Equals(other) && string.Equals(DefaultValue, other.DefaultValue) && IsNullable.Equals(other.IsNullable) && string.Equals(DataType, other.DataType);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (DefaultValue != null ? DefaultValue.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IsNullable.GetHashCode();
                hashCode = (hashCode * 397) ^ (DataType != null ? DataType.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}