using System;

namespace Neurotoxin.Roentgen.CSharp.Models
{
    [Flags]
    public enum ClassType
    {
        Default = 1,
        Generated = 2,
        PartiallyGenerated = 3
    }
}