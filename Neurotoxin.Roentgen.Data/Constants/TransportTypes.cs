using System;
using Neurotoxin.Roentgen.Data.Attributes;

namespace Neurotoxin.Roentgen.Data.Constants
{
    [Flags]
    public enum TransportTypes
    {
        Unknown = 0,
        
        NotAvailable = 1,

        [DisplayText(DisplayTextOption.SkipMemberName, "HTTP[^S]|HTTP$")]
        HTTP = 2,
        
        HTTPS = 4,
        
        FTP = 8,
        
        SNTP = 0x10,
        
        Telnet = 0x20,
        
        MSMQ = 0x40,
        
        [DisplayText("SQL Service Broker")]
        ServiceBroker = 0x80,
        
        [DisplayText("Azure ServiceBus")]
        AzureServiceBus = 0x100,
        
        [DisplayText("Azure Topic")]
        AzureTopic = 0x200,
        
        NServiceBus = 0x400,
        
        RabbitMq = 0x800,
        
        File = 0x1000,
        
        [DisplayText("On demand")]
        RPC = 0x2000,
        
        [DisplayText("T-SQL")]
        TSQL = 0x4000,
        
        [DisplayText("Exchange server")]
        ExchangeServer = 0x8000,
        
        Internal = 0x10000,
        
        [DisplayText("In-proc")]
        InProc = 0x20000
    }
}