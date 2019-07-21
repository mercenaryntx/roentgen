using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Core;
using Microsoft.Build.Locator;
using Neurotoxin.ScOut.Analysis;
using Neurotoxin.ScOut.Extensions;
using Neurotoxin.ScOut.Mappers;

namespace Neurotoxin.ScOut
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

        public RoslynAnalyzer RegisterPostProcessor<T>() where T : PostProcessor
        {
            _containerBuilder.RegisterType<T>();
            return this;
        }

        public AnalysisResult Analyze()
        {
            var container = BuildContainer();

            var solutionMapper = container.Resolve<ISolutionMapper>();
            _solutions.ForEach(s => solutionMapper.Map(s));

            var postProcessorType = typeof(PostProcessor);
            container.ComponentRegistry
                     .Registrations
                     .Select(r => r.Services.First())
                     .OfType<TypedService>()
                     .Select(s => s.ServiceType)
                     .Where(t => postProcessorType.IsAssignableFrom(t))
                     .Select(t => container.Resolve(t))
                     .Cast<PostProcessor>()
                     .ForEach(pp => pp.Process());

            var workspace = container.Resolve<AnalysisWorkspace>();
            return new AnalysisResult
            {
                Solutions = workspace.Solutions.Values.ToArray(),
                Links = workspace.Links.ToArray()
            };
        }

        private IContainer BuildContainer()
        {
            _containerBuilder.RegisterType<AnalysisWorkspace>().As<AnalysisWorkspace>().SingleInstance().IfNotRegistered(typeof(AnalysisWorkspace));
            _containerBuilder.RegisterType<SolutionMapper>().As<ISolutionMapper>().SingleInstance().IfNotRegistered(typeof(ISolutionMapper));
            _containerBuilder.RegisterType<ProjectMapper>().As<IProjectMapper>().SingleInstance().IfNotRegistered(typeof(IProjectMapper));
            _containerBuilder.RegisterInstance(_excludingRules);
            return _containerBuilder.Build();
        }
    }
}