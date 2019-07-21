using Neurotoxin.ScOut.Analysis;
using Neurotoxin.ScOut.Models;

namespace RoslynDemo
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