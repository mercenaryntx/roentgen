using Neurotoxin.Roentgen.Analysis;
using Neurotoxin.Roentgen.Models;

namespace EfTestApp.Analysis
{
    public class SqlCommandCall : LinkBase
    {
        public Method Caller { get; }
        public string Command { get; }

        public SqlCommandCall(Method caller, string command) : base(caller, command)
        {
            Caller = caller;
            Command = command;
        }
    }
}