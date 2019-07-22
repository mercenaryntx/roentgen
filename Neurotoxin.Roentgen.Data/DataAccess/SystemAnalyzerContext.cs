using System.Data.Entity;
using Neurotoxin.Roentgen.Data.Entities;
using Neurotoxin.Roentgen.Data.Relations;

namespace Neurotoxin.Roentgen.Data.DataAccess
{
    public class SystemAnalyzerContext : DbContext
    {
        public virtual DbSet<EntityBase> Entities { get; set; }
        public virtual DbSet<RelationBase> Relations { get; set; }

        public SystemAnalyzerContext(string connectionName) : base(connectionName)
        {
            Database.SetInitializer(new DataAccessLayerInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(EntityBase.BaseConfig());
            modelBuilder.Configurations.Add(RelationBase.BaseConfig());
        }
    }
}