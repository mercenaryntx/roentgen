using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Core;
using Microsoft.Build.Locator;
using Neurotoxin.Roentgen.CSharp.Analysis;
using Neurotoxin.Roentgen.CSharp.Extensions;
using Neurotoxin.Roentgen.CSharp.Mappers;
using Neurotoxin.Roentgen.CSharp.PostProcessors;
using Neurotoxin.Roentgen.CSharp.Visitors;

namespace Neurotoxin.Roentgen.CSharp
{
    public class RoslynAnalyzer
    {
        private readonly List<string> _solutions = new List<string>();
        private readonly ExcludingRules _excludingRules = new ExcludingRules();
        private readonly ContainerBuilder _containerBuilder = new ContainerBuilder();

        static RoslynAnalyzer()
        {
            MSBuildLocator.RegisterDefaults();
        }

        public RoslynAnalyzer AddSolution(string path)
        {
            _solutions.Add(path);
            return this;
        }

        public RoslynAnalyzer AddSolutions(IEnumerable<string> path)
        {
            _solutions.AddRange(path);
            return this;
        }

        public RoslynAnalyzer ExcludeFiles(params string[] rule)
        {
            _excludingRules.ExcludeFiles.AddRange(rule);
            return this;
        }

        public RoslynAnalyzer RegisterWorkspace<T>() where T : AnalysisWorkspace
        {
            _containerBuilder.RegisterType<T>().As<AnalysisWorkspace>().SingleInstance();
            return this;
        }

        public RoslynAnalyzer RegisterPostProcessor<T>() where T : PostProcessorBase
        {
            _containerBuilder.RegisterType<T>();
            return this;
        }

        public AnalysisResult Analyze()
        {
            var container = BuildContainer();

            container.Resolve<ISolutionMapper>().Map(_solutions);

            var postProcessorType = typeof(PostProcessorBase);
            container.ComponentRegistry
                     .Registrations
                     .Select(r => r.Services.First())
                     .OfType<TypedService>()
                     .Select(s => s.ServiceType)
                     .Where(t => postProcessorType.IsAssignableFrom(t))
                     .Select(t => container.Resolve(t))
                     .Cast<PostProcessorBase>()
                     .ForEach(pp => pp.Process());

            var workspace = container.Resolve<AnalysisWorkspace>();
            return new AnalysisResult
            {
                Solutions = workspace.Solutions.Values.ToArray(),
                Links = workspace.Links.ToArray(),
                Diagnostics = workspace.Diagnostics
            };
        }

        private IContainer BuildContainer()
        {
            _containerBuilder.RegisterType<FindVariableVisitor>();
            _containerBuilder.RegisterType<FindLiteralVisitor>();
            _containerBuilder.RegisterType<SourceFileVisitor>();
            _containerBuilder.RegisterGeneric(typeof(WorkspaceLogger<>)).As(typeof(ILogger<>)).SingleInstance();
            _containerBuilder.RegisterType<AnalysisWorkspace>().As<AnalysisWorkspace>().SingleInstance().IfNotRegistered(typeof(AnalysisWorkspace));
            _containerBuilder.RegisterType<SolutionMapper>().As<ISolutionMapper>().SingleInstance().IfNotRegistered(typeof(ISolutionMapper));
            _containerBuilder.RegisterType<ProjectMapper>().As<IProjectMapper>().SingleInstance().IfNotRegistered(typeof(IProjectMapper));
            _containerBuilder.RegisterInstance(_excludingRules);
            return _containerBuilder.Build();
        }
    }
}