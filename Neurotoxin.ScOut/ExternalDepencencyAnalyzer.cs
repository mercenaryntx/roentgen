using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neurotoxin.ScOut.Analysis;
using Neurotoxin.ScOut.Models;
using Neurotoxin.ScOut.Patterns;

namespace Neurotoxin.ScOut
{
    public class ExternalDepencencyAnalyzer
    {
        public string[] Analyze<T>(AnalysisResult analysisResult) where T : IDependencyScanner, new()
        {
            var rule = new T();
            var calls = analysisResult.Methods.Values.SelectMany(m => m.ExternalCalls.Where(c => rule.MethodCall.Any(p => c.TargetSymbol?.ToString().Contains(p) == true))).ToArray();
            return calls.Select(call => rule.Scan(call)).Where(res => !string.IsNullOrEmpty(res)).ToArray();
        }
    }
}