using System.Collections.Generic;
using Neurotoxin.Roentgen.CSharp.Models;

namespace Neurotoxin.Roentgen.CSharp.Analysis
{
    public class AnalysisResult
    {
        public Solution[] Solutions { get; set; }
        public LinkBase[] Links { get; set; }
        public List<LogMessage> Diagnostics { get; set; }
    }
}