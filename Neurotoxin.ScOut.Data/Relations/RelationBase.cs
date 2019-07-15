using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Neurotoxin.ScOut.Data.Extensions;

namespace Neurotoxin.ScOut.Data.Relations
{
    public abstract class RelationBase
    {
        [Key]
        public int Id { get; set; }
        [Index]
        public Guid LeftEntityId { get; set; }
        [Index]
        public Guid RightEntityId { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string Comment { get; set; }

        protected RelationBase()
        {
            CreatedOn = DateTime.Now;
            CreatedBy = EnvironmentExtensions.GetCurrentUserName();
        }

        /// <summary>
        /// RelationBase base table configuration.
        /// </summary>
        /// <returns>Configuration class/</returns>
        public static EntityTypeConfiguration<RelationBase> BaseConfig()
        {
            var config = new EntityTypeConfiguration<RelationBase>();
            config.ToTable("Relations");

            return config;
        }
    }
}