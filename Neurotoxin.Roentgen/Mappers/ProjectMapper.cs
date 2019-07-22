using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Autofac;
using Autofac.Core;
using Microsoft.CodeAnalysis.CSharp;
using Neurotoxin.Roentgen.Analysis;
using Neurotoxin.Roentgen.Visitors;
using Neurotoxin.Roentgen.Models;

namespace Neurotoxin.Roentgen.Mappers
{
    public class ProjectMapper : IProjectMapper
    {
        private readonly ExcludingRules _excludingRules;
        private readonly ILogger<ProjectMapper> _logger;
        private readonly ILifetimeScope _lifetimeScope;

        public ProjectMapper(ExcludingRules excludingRules, ILogger<ProjectMapper> logger, ILifetimeScope lifetimeScope)
        {
            _excludingRules = excludingRules;
            _logger = logger;
            _lifetimeScope = lifetimeScope;
        }

        public Project Map(Microsoft.CodeAnalysis.Project proj)
        {
            var compilation = proj.GetCompilationAsync().GetAwaiter().GetResult();
            var sourceFiles = compilation.SyntaxTrees
                                         .Where(t => Path.GetExtension(t.FilePath) == ".cs")
                                         .Where(NotExcluded);

            return new Project
            {
                FullName = proj.FilePath,
                Language = MapCSharpVersion(proj),
                TargetFramework = MapTargetFramework(proj),
                Children = sourceFiles.Select(file => _lifetimeScope.Resolve<SourceFileVisitor>().Discover(file, compilation)).ToList()
            };
        }

        private string MapCSharpVersion(Microsoft.CodeAnalysis.Project proj)
        {
            //TODO: double check that this returns the right C# version
            var parseOptions = (CSharpParseOptions) proj.ParseOptions;
            var version = parseOptions.LanguageVersion.ToString();
            var m = new Regex(@"CSharp(?<major>\d)(?:_(?<minor>\d))?").Match(version);
            if (!m.Success)
            {
                _logger.Warning($"C# version of the following project couldn't be determnined: {proj.FilePath}");
                return version;
            }

            var major = m.Groups["major"].Value;
            var minor = string.IsNullOrEmpty(m.Groups["minor"].Value) ? "0" : m.Groups["minor"].Value;
            return $"C# {major}.{minor}";
        }

        private string MapTargetFramework(Microsoft.CodeAnalysis.Project proj)
        {
            var mscorlibVersion = new Regex(@"v([\d\.]+)\\.*?mscorlib\.dll$");
            var frameworkVersion = proj.MetadataReferences.Select(m => mscorlibVersion.Match(m.Display)).FirstOrDefault(m => m.Success)?.Groups[1].Value;
            if (frameworkVersion != null) return $".NET Framework {frameworkVersion}";

            _logger.Warning($"Target framework of the folloing project couldn't be determined: {proj.FilePath}");
            return null;
        }

        private bool NotExcluded(Microsoft.CodeAnalysis.SyntaxTree tree)
        {
            var rule = _excludingRules.ExcludeFiles.FirstOrDefault(r => new Regex(r).IsMatch(tree.FilePath));
            if (rule != null) _logger.Info($"{tree.FilePath} excluded because of the following rule: {rule}");
            return rule == null;
        }
    }
}