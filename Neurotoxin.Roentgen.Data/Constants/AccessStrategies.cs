using System;

namespace Neurotoxin.Roentgen.Data.Constants
{
    [Flags]
    public enum AccessStrategies
    {
        Unknown = 0,
        None = 1,
        Read = 2,
        Write = 4
    }
}