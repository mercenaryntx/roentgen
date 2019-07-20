using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using Dapper;
using Neurotoxin.ScOut;
using Neurotoxin.ScOut.Analysis;
using Neurotoxin.ScOut.Data;
using Neurotoxin.ScOut.Data.DataAccess;
using Neurotoxin.ScOut.Data.Entities;
using Neurotoxin.ScOut.Data.Relations;
using Neurotoxin.ScOut.Mappers;
using Neurotoxin.ScOut.Models;
using RoslynDemo.Extensions;
using RoslynDemo.Patterns;

namespace RoslynDemo
{
    public class Application
    {
        private readonly RoslynAnalyzer _analyzer;
        //private readonly IMapper<Solution, SolutionEntity> _solutionMapper;
        //private readonly IMapper<Project, ProjectEntity> _projectMapper;
        //private readonly IMapper<Class, ClassEntity> _classMapper;
        //private readonly IMapper<Method, MethodEntity> _methodMapper;

        public Application(RoslynAnalyzer analyzer)
            //,
            //IMapper<Solution, SolutionEntity> solutionMapper,
            //IMapper<Project, ProjectEntity> projectMapper,
            //IMapper<Class, ClassEntity> classMapper,
            //IMapper<Method, MethodEntity> methodMapper)
        {
            _analyzer = analyzer;
            //_solutionMapper = solutionMapper;
            //_projectMapper = projectMapper;
            //_classMapper = classMapper;
            //_methodMapper = methodMapper;
        }

        public void Run()
        {
            var sw = new Stopwatch();
            sw.Start();

            //var context = new SystemAnalyzerContext("TargetConnection");
            //var c = context.Entities.Count();
            //Console.WriteLine($"[{sw.Elapsed}] DB Schema check.");

            var solutions = new[]
            {
                @"c:\Work\NTX-SCT\EF\elektra-wpf-ls\EFLT.EO.sln",
                //@"e:\Work\EF\github\elektra-wpf-ls\EFLT.EO.sln",
                //@"e:\Work\EF\github\elektra-services-restapi\ElektraProjects.sln",
                //@"e:\Work\EF\github\elektra-ls-hfvisitapp\HFVisitApp\ElektraService.sln",
                //@"e:\Work\EF\github\elektra-wpf-lt\EFLT.EO.sln",
                //@"e:\Work\EF\github\elektra-ls-studentidcard\EF.Language.Elektra\EFLT.Elektra.sln",
                //@"e:\Work\EF\github\Poseidon-EFSMSApp\EFSMSApp.sln",
                //@"e:\Work\EF\github\elektra-wpf-rti\AccommodationStatus\AccommodationStatus.sln",
                //@"e:\Work\EF\github\elektra-wpf-rti\Capacity Manager\ResidenceCapacity - WithUI Changes\ResidenceCapacity.sln",
                //@"e:\Work\EF\github\elektra-wpf-rti\CapacityManagerService\CapacityManagerService.sln",
                //@"e:\Work\EF\github\elektra-wpf-rti\RTIForGlobalnet\RTIForGlobalnet.sln",
                //@"e:\Work\EF\github\elektra-ls-capacitymanager\ResidenceCapacity\ResidenceCapacity.sln",
            };

            var result = _analyzer.AddSolutions(solutions).Analyze();

            //foreach (var path in solutions)
            //{
            //    //Console.WriteLine($"[{sw.Elapsed}] Loading {path}...");
            //    //var analysisResult = _analyzer.LoadSolution(path).Analyze();
            //    //Console.WriteLine($"[{sw.Elapsed}] Solution loaded.");

            //    //var depAnal = new ExternalDepencencyAnalyzer();
            //    //var sqlQueries = depAnal.Analyze<SqlCommandExecutionScanner>(analysisResult);
            //    //Console.WriteLine($"[{sw.Elapsed}] SQL Command analysis finished.");

            //    //Persist(analysisResult, sqlQueries);
            //    //Console.WriteLine($"[{sw.Elapsed}] Data persisted.");
            //}
        }

        //public void Persist(AnalysisResult result, Dictionary<Method, string> sqlQueries)
        //{
        //    var methodCache = new Dictionary<Method, EntityBase>();
        //    var entities = new List<EntityBase>();
        //    var relations = new List<RelationBase>();
        //    var solution = _solutionMapper.Map(result.Solution).Defaults(entities);
        //    foreach (var p in result.Solution.Projects)
        //    {
        //        var project = _projectMapper.Map(p).Defaults(entities).RelateTo<ParentChildRelation>(solution, relations);
        //        foreach (var c in p.Classes.Values)
        //        {
        //            var @class = _classMapper.Map(c).Defaults(entities).RelateTo<ParentChildRelation>(project, relations);
        //            foreach (var m in c.Methods.Values.SelectMany(v => v))
        //            {
        //                var method = _methodMapper.Map(m).Defaults(entities).RelateTo<ParentChildRelation>(@class, relations);
        //                if (sqlQueries.ContainsKey(m))
        //                {
        //                    method.Comment = $"SQL Calls:\r\n{sqlQueries[m]}";
        //                }
        //                methodCache.Add(m, method);
        //            }
        //        }
        //    }

        //    foreach (var method in result.Methods.Values)
        //    {
        //        foreach (var call in method.InternalCalls)
        //        {
        //            methodCache[method].RelateTo<CallRelation>(methodCache[call.Callee], relations);
        //        }
        //    }

        //    using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["TargetConnection"].ConnectionString))
        //    {
        //        db.Open();
        //        using (var transaction = db.BeginTransaction())
        //        {
        //            foreach (var entity in entities)
        //            {
        //                var query = EntityMap.GetInsertQuery("Entities", entity.GetType());
        //                db.Execute(query, entity, transaction);
        //            }

        //            foreach (var relation in relations)
        //            {
        //                var query = EntityMap.GetInsertQuery("Relations", relation.GetType());
        //                db.Execute(query, relation, transaction);
        //            }
        //            transaction.Commit();
        //        }
        //    }
        //}
    }
}