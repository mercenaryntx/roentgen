using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Neurotoxin.ScOut.Autofac;
using Neurotoxin.ScOut.Filtering;

namespace RoslynDemo
{
    class Program
    {
        static private IContainer CompositionRoot()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Application>();
            builder.RegisterType<DefaultSourceFileFiltering>().As<ISourceFileFiltering>().SingleInstance();
            builder.RegisterType<DefaultDependencyFiltering>().As<IDependencyFiltering>().SingleInstance();
            builder.RegisterModule<ScOutModule>();
            return builder.Build();
        }

        static void Main(string[] args)
        {
            CompositionRoot().Resolve<Application>().Run();
            Console.WriteLine("--DONE--");
            Console.ReadKey();
        }
    }
}