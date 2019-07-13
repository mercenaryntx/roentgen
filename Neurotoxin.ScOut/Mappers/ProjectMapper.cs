using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Neurotoxin.ScOut.Filtering;
using Neurotoxin.ScOut.Models;
using Project = Neurotoxin.ScOut.Models.Project;

namespace Neurotoxin.ScOut.Mappers
{
    public class ProjectMapper : IMapper<Microsoft.CodeAnalysis.Project, Project>
    {
        private readonly ISourceFileMapper _sourceFileMapper;
        private readonly ITargetFrameworkMapper _targetFrameworkMapper;
        private readonly Dictionary<string, string> _includeRules;
        private readonly Dictionary<string, string> _excludeRules;

        public ProjectMapper(ISourceFileMapper sourceFileMapper, ITargetFrameworkMapper targetFrameworkMapper, ISourceFileFiltering rules)
        {
            _sourceFileMapper = sourceFileMapper;
            _targetFrameworkMapper = targetFrameworkMapper;
            _includeRules = rules.IncludeRules;
            _excludeRules = rules.ExcludeRules;
        }

        public Project Map(Microsoft.CodeAnalysis.Project proj)
        {
            var compilation = proj.GetCompilationAsync().GetAwaiter().GetResult();
            var trees = compilation.SyntaxTrees;
            var sourceFiles = trees.Where(Included).Where(NotExcluded).Select(t => _sourceFileMapper.Map(t, compilation));

            return new Project
            {
                Path = proj.FilePath,
                Language = proj.Language,
                LanguageVersion = proj.ParseOptions.Language,
                TargetFramework = _targetFrameworkMapper.Map(trees),
                Classes = FlattenClasses(sourceFiles)
            };
        }

        private static Dictionary<string, Class> FlattenClasses(IEnumerable<SourceFile> sourceFiles)
        {
            return sourceFiles.SelectMany(file => file.Classes.Select(c => new
                                    {
                                        Class = c,
                                        File = file.Path,
                                        file.Usings
                                    }))
                              .GroupBy(t => t.Class.FullName)
                              .Select(g =>
                                  {
                                      var first = g.First();
                                      var fc = first.Class;
                                      if (g.Count() == 1)
                                      {
                                          fc.Usings = first.Usings;
                                          fc.SourceFiles = new[] {first.File};
                                          return fc;
                                      }
                                      return new Class
                                      {
                                          Name = fc.Name,
                                          Namespace = fc.Namespace,
                                          Model = fc.Model,
                                          SourceFiles = g.Select(v => v.File).ToArray(),
                                          Usings = g.SelectMany(v => v.Usings).ToArray(),
                                          Properties = g.SelectMany(v => v.Class.Properties).ToDictionary(p => p.Key, p => p.Value),
                                          Methods = g.SelectMany(v => v.Class.Methods.SelectMany(m => m.Value)).GroupBy(m => m.Name).ToDictionary(gg => gg.Key, gg => gg.ToArray())
                                      };
                                  })
                              .ToDictionary(c => c.FullName, c => c);
        }

        private bool Included(SyntaxTree tree)
        {
            var fileName = Path.GetFileName(tree.FilePath);
            return _includeRules.Values.Any(r => new Regex(r).IsMatch(fileName));
        }


        private bool NotExcluded(SyntaxTree tree)
        {
            var fileName = Path.GetFileName(tree.FilePath);
            return _excludeRules.Values.All(r => !new Regex(r).IsMatch(fileName));
        }

    }
}