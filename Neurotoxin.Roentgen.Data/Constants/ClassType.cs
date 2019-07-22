using System;

namespace Neurotoxin.Roentgen.Data.Constants
{
    [Flags]
    public enum ClassType
    {
        Default = 1,
        Generated = 2,
        PartiallyGenerated = 3
    }
}