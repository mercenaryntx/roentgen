using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Neurotoxin.ScOut.Data.Extensions;

namespace Neurotoxin.ScOut.Data.Entities
{
    public abstract class EntityBase : IEntityBase
    {
        [Key]
        public int RowId { get; set; }
        [Index]
        public Guid EntityId { get; set; }
        [Index]
        [StringLength(1700)]
        public string Name { get; set; }
        [Index]
        public int VersionNumber { get; set; }
        [Index]
        public bool LatestVersion { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Manual or automatic comments related to the given version.
        /// </summary>
        public string Comment { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string LockedBy { get; set; }
        public int Loc { get; set; }
        public int Length { get; set; }

        protected EntityBase()
        {
            CreatedOn = DateTime.Now;
            EntityId = Guid.NewGuid();
            LatestVersion = true;
            CreatedBy = EnvironmentExtensions.GetCurrentUserName();
        }

        /// <summary>
        /// Entity base table configuration.
        /// </summary>
        /// <returns>Configuration class/</returns>
        public static EntityTypeConfiguration<EntityBase> BaseConfig()
        {
            var config = new EntityTypeConfiguration<EntityBase>();
            config.ToTable("Entities");
            return config;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EntityBase) obj);
        }

        protected bool Equals(EntityBase other)
        {
            return string.Equals(Name, other.Name) && string.Equals(Description, other.Description);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ (Description != null ? Description.GetHashCode() : 0);
            }
        }

        protected bool StringEquals(string a, string b)
        {
            return string.IsNullOrEmpty(a) && string.IsNullOrEmpty(b) || string.Equals(a, b);
        }
    }
}