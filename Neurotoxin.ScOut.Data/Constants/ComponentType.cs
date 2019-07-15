using Neurotoxin.ScOut.Data.Attributes;

namespace Neurotoxin.ScOut.Data.Constants
{
    public enum ComponentType
    {
        Unknown,

        [DisplayText("EXE")]
        Exe,

        [DisplayText("NT Service")]
        NtService,

        [DisplayText("COM+")]
        COM,

        [DisplayText("SSIS")]
        SSIS,

        [DisplayText("Windows Service")]
        WindowsService,

        [DisplayText("Web Service")]
        WebService,

        [DisplayText("WCF")]
        WcfService,

        [DisplayText("Web/HTML")]
        HtmlWebsite,

        [DisplayText("Web/MVC")]
        MvcWebsite,

        [DisplayText("Azure Service")]
        AzureService,

        [DisplayText("RESTful Web Service")]
        RestfulWebService,

        [DisplayText("Library")]
        Library,

        [DisplayText("External")]
        External,

        [DisplayText("File Reader")]
        FileReader,

        [DisplayText("HTTP Handler")]
        HttpHandler,

        [DisplayText("Event Handler")]
        EventHandler,

        [DisplayText("Command Handler")]
        CommandHandler,

        [DisplayText("Assembly")]
        Assembly,

        [DisplayText("Web API Controller")]
        WebApiController,

        [DisplayText("Service")]
        Service
    }
}