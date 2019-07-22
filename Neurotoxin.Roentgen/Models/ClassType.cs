using System;

namespace Neurotoxin.Roentgen.Models
{
    [Flags]
    public enum ClassType
    {
        Default = 1,
        Generated = 2,
        PartiallyGenerated = 3
    }
}