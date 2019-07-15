using System;

namespace Neurotoxin.ScOut.Data.Constants
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