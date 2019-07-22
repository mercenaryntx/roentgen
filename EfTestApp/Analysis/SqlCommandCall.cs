using Neurotoxin.Roentgen.CSharp.Analysis;
using Neurotoxin.Roentgen.CSharp.Models;

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