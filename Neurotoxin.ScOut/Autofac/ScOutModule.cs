using Autofac;
using Neurotoxin.ScOut.Mappers;
using Neurotoxin.ScOut.Models;

namespace Neurotoxin.ScOut.Autofac
{
    public class ScOutModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ClassMapper>().As<IClassMapper>().SingleInstance();
            builder.RegisterType<MethodMapper>().As<IMethodMapper>().SingleInstance();
            builder.RegisterType<ProjectMapper>().As<IMapper<Microsoft.CodeAnalysis.Project, Project>>().SingleInstance();
            builder.RegisterType<PropertyMapper>().As<IPropertyMapper>().SingleInstance();
            builder.RegisterType<SolutionMapper>().As<IMapper<Microsoft.CodeAnalysis.Solution, Solution>>().SingleInstance();
            builder.RegisterType<SourceFileMapper>().As<ISourceFileMapper>().SingleInstance();
            builder.RegisterType<TargetFrameworkMapper>().As<ITargetFrameworkMapper>().SingleInstance();

            builder.RegisterType<RoslynAnalyzerOld>();
            builder.RegisterType<RoslynAnalyzer>();
        }
    }
}