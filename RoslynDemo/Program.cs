using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Neurotoxin.ScOut.Data.Entities;
using Neurotoxin.ScOut.Models;

namespace RoslynDemo
{
    class Program
    {
        //private static IContainer CompositionRoot()
        //{
        //    var builder = new ContainerBuilder();
        //    builder.RegisterType<Application>();
        //    builder.RegisterType<DefaultSourceFileFiltering>().As<ISourceFileFiltering>().SingleInstance();
        //    builder.RegisterType<DefaultDependencyFiltering>().As<IDependencyFiltering>().SingleInstance();
        //    builder.RegisterModule<ScOutModule>();
        //    builder.RegisterType<SolutionEntityMapper>().As<IMapper<Solution, SolutionEntity>>().SingleInstance();
        //    builder.RegisterType<ProjectEntityMapper>().As<IMapper<Project, ProjectEntity>>().SingleInstance();
        //    builder.RegisterType<ClassEntityMapper>().As<IMapper<Class, ClassEntity>>().SingleInstance();
        //    builder.RegisterType<MethodEntityMapper>().As<IMapper<Method, MethodEntity>>().SingleInstance();
        //    return builder.Build();
        //}

        static void Main(string[] args)
        {
            //CompositionRoot().Resolve<Application>()
            new Application().Run();
            Console.WriteLine("--DONE--");
            Console.ReadKey();
        }
    }
}