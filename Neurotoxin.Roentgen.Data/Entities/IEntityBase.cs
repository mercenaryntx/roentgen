
using System;

namespace Neurotoxin.Roentgen.Data.Entities
{
    public interface IEntityBase
    {
        int RowId { get; set; }
        Guid EntityId { get; set; }
        string Name { get; set; }
        int VersionNumber { get; set; }
        bool LatestVersion { get; set; }
        string Description { get; set; }
        /// <summary>
        /// Manual or automatic comments related to the given version.
        /// </summary>
        string Comment { get; set; }
        DateTime CreatedOn { get; set; }
        string CreatedBy { get; set; }
        string LockedBy { get; set; }
    }
}
