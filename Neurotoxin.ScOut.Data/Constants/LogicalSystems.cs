using System;
using Neurotoxin.ScOut.Data.Attributes;

namespace Neurotoxin.ScOut.Data.Constants
{
    [Flags]
    public enum LogicalSystems
    {
        Unknown = 0,

        [DisplayText("Flight Operations")]
        FlightOperations = 1,

        Commercial = 2,

        Batch = 4,

        [DisplayText("Pricing Service", "Optimisation Service")]
        Pricing = 8,

        Interactive = 0x10,

        [DisplayText("Optimisation Service")]
        Optimization = 0x20,

        [DisplayText("Flight Tracker")]
        FlightTracker = 0x40,

        [DisplayText("HALO")]
        HALO = 0x80
    }
}