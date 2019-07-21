using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.CSharp;
using Neurotoxin.ScOut.Analysis;
using Neurotoxin.ScOut.Models;
using Neurotoxin.ScOut.Visitors;

namespace Neurotoxin.ScOut.Mappers
{
    public class ProjectMapper : IProjectMapper
    {
        private readonly ExcludingRules _excludingRules;
        private readonly AnalysisWorkspace _workspace;

        public ProjectMapper(ExcludingRules excludingRules, AnalysisWorkspace workspace)
        {
            _excludingRules = excludingRules;
            _workspace = workspace;
        }

        public Project Map(Microsoft.CodeAnalysis.Project proj)
        {
            var compilation = proj.GetCompilationAsync().GetAwaiter().GetResult();
            var trees = compilation.SyntaxTrees.ToArray();
            var sourceFiles = trees
                .Where(t => Path.GetExtension(t.FilePath) == ".cs")
                .Where(t => _excludingRules.ExcludeFiles.All(r => !new Regex(r).IsMatch(t.FilePath)));

            return new Project
            {
                FullName = proj.FilePath,
                Language = MapCSharpVersion(proj),
                TargetFramework = MapTargetFramework(proj),
                Children = sourceFiles.Select(file => new SourceFileVisitor().Discover(file, compilation)).ToList()
            };
        }

        private static string MapCSharpVersion(Microsoft.CodeAnalysis.Project proj)
        {
            //TODO: double check that this returns the right C# version
            var parseOptions = (CSharpParseOptions) proj.ParseOptions;
            var version = parseOptions.LanguageVersion.ToString();
            var m = new Regex(@"CSharp(?<major>\d)(?:_(?<minor>\d))?").Match(version);
            if (!m.Success) return version;

            var major = m.Groups["major"].Value;
            var minor = string.IsNullOrEmpty(m.Groups["minor"].Value) ? "0" : m.Groups["minor"].Value;
            return $"C# {major}.{minor}";
        }

        private static string MapTargetFramework(Microsoft.CodeAnalysis.Project proj)
        {
            var mscorlibVersion = new Regex(@"v([\d\.]+)\\.*?mscorlib\.dll$");
            var frameworkVersion = proj.MetadataReferences.Select(m => mscorlibVersion.Match(m.Display)).FirstOrDefault(m => m.Success)?.Groups[1].Value;
            return frameworkVersion != null ? $".NET Framework {frameworkVersion}" : null;
        }
    }
}