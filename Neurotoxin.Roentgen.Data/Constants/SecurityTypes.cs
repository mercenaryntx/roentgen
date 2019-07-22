using System;

namespace Neurotoxin.Roentgen.Data.Constants
{
    [Flags]
    public enum SecurityTypes
    {
        Unknown = 0,
        None = 1,
        SSL = 2,
        ACS = 4
    }
}