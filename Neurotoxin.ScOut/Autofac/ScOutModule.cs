using Autofac;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Neurotoxin.ScOut.Mappers;
using Neurotoxin.ScOut.Models;

namespace Neurotoxin.ScOut.Autofac
{
    public class ScOutModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CallMapper>().As<IMapper<InvocationExpressionSyntax, Call>>().SingleInstance();
            builder.RegisterType<ClassMapper>().As<IClassMapper>().SingleInstance();
            builder.RegisterType<MethodMapper>().As<IMethodMapper>().SingleInstance();
            builder.RegisterType<ProjectMapper>().As<IMapper<Microsoft.CodeAnalysis.Project, Project>>().SingleInstance();
            builder.RegisterType<PropertyMapper>().As<IPropertyMapper>().SingleInstance();
            builder.RegisterType<SolutionMapper>().As<IMapper<Microsoft.CodeAnalysis.Solution, Solution>>().SingleInstance();
            builder.RegisterType<SourceFileMapper>().As<IMapper<Microsoft.CodeAnalysis.SyntaxTree, SourceFile>>().SingleInstance();
            builder.RegisterType<TargetFrameworkMapper>().As<ITargetFrameworkMapper>().SingleInstance();
            builder.RegisterType<UsingMapper>().As<IMapper<UsingDirectiveSyntax, Using>>().SingleInstance();

            builder.RegisterType<RoslynAnalyzer>();
        }
    }
}