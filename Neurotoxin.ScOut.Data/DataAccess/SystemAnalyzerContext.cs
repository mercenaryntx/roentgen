using System.Data.Entity;
using Neurotoxin.ScOut.Data.Entities;
using Neurotoxin.ScOut.Data.Relations;

namespace Neurotoxin.ScOut.Data.DataAccess
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