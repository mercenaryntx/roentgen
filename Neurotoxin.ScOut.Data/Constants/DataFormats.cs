using System;
using Neurotoxin.ScOut.Data.Attributes;

namespace Neurotoxin.ScOut.Data.Constants
{
    [Flags]
    public enum DataFormats
    {
        Unknown = 0,
        NotAvailable = 1,
        XML = 2,
        HTML = 4,
        CSV = 8,
        JSON = 0x10,
        SOAP = 0x20,
        [DisplayText("SP")]
        StoredProcedure = 0x40,
        Text = 0x80,
        SQL = 0x100,
        TDS = 0x200,
        WebApi = 0x400,
        Custom = 0x800,
        ASM = 0x1000,
        SSM = 0x2000,
        Typed = 0x4000
    }
}