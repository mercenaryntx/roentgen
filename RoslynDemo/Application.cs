using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Neurotoxin.ScOut;
using Neurotoxin.ScOut.Extensions;
using Neurotoxin.ScOut.Models;
using RoslynDemo.Patterns;

namespace RoslynDemo
{
    public class Application
    {
        private readonly RoslynAnalyzer _analyzer;

        public Application(RoslynAnalyzer analyzer)
        {
            _analyzer = analyzer;
        }

        public void Run()
        {
            var analysisResult = _analyzer.LoadSolution(@"c:\Work\NTX-SCT\EF\elektra-wpf-ls\EFLT.EO.sln").Analyze();
            var depAnal = new ExternalDepencencyAnalyzer();
            var sqlQueries = depAnal.Analyze<SqlCommandExecutionScanner>(analysisResult);
            
            Debugger.Break();
        }
    }
}