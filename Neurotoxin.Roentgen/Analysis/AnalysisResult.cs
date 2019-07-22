using System.Collections.Generic;
using Neurotoxin.Roentgen.Models;

namespace Neurotoxin.Roentgen.Analysis
{
    public class AnalysisResult
    {
        public Solution[] Solutions { get; set; }
        public LinkBase[] Links { get; set; }
        public List<LogMessage> Diagnostics { get; set; }
    }
}