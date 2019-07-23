using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using Dapper;
using EfTestApp.Analysis;
using Neurotoxin.Roentgen.CSharp;
using Neurotoxin.Roentgen.CSharp.Analysis;
using Neurotoxin.Roentgen.CSharp.Extensions;
using Neurotoxin.Roentgen.CSharp.Models;
using Neurotoxin.Roentgen.CSharp.PostProcessors;
using Neurotoxin.Roentgen.Data;
using Neurotoxin.Roentgen.Data.DataAccess;
using Neurotoxin.Roentgen.Data.Entities;
using Neurotoxin.Roentgen.Data.Relations;

namespace EfTestApp
{
    public class Application
    {
        public void Run()
        {
            var sw = new Stopwatch();
            sw.Start();

            var solutions = new[]
            {
                //@"c:\Work\NTX-SCT\EF\elektra-wpf-ls\EFLT.EO.sln",
                @"e:\Work\EF\github\elektra-wpf-ls\EFLT.EO.sln",
                @"e:\Work\EF\github\elektra-services-restapi\ElektraProjects.sln",
                @"e:\Work\EF\github\elektra-ls-hfvisitapp\HFVisitApp\ElektraService.sln",
                @"e:\Work\EF\github\elektra-wpf-lt\EFLT.EO.sln",
                @"e:\Work\EF\github\elektra-ls-studentidcard\EF.Language.Elektra\EFLT.Elektra.sln",
                @"e:\Work\EF\github\Poseidon-EFSMSApp\EFSMSApp.sln",
                @"e:\Work\EF\github\elektra-wpf-rti\AccommodationStatus\AccommodationStatus.sln",
                @"e:\Work\EF\github\elektra-wpf-rti\Capacity Manager\ResidenceCapacity - WithUI Changes\ResidenceCapacity.sln",
                @"e:\Work\EF\github\elektra-wpf-rti\CapacityManagerService\CapacityManagerService.sln",
                @"e:\Work\EF\github\elektra-wpf-rti\RTIForGlobalnet\RTIForGlobalnet.sln",
                @"e:\Work\EF\github\elektra-ls-capacitymanager\ResidenceCapacity\ResidenceCapacity.sln",
            };

            foreach (var path in solutions)
            {
                Console.WriteLine($"[{sw.Elapsed}] Analyzing {path}...");
                var result = new RoslynAnalyzer()
                    .AddSolutions(solutions)
                    .RegisterPostProcessor<MethodInvocationsFinder>()
                    .RegisterPostProcessor<SqlCommandExecutionFinder>()
                    .Analyze();
                Console.WriteLine($"[{sw.Elapsed}] Solution analyzed.");

                Persist(result);
                Console.WriteLine($"[{sw.Elapsed}] Data persisted.");
            }
        }

        private static void Persist(AnalysisResult result)
        {
            var relationMapper = new RelationMapper();
            var entities = MapEntities(result, (codePart, entity) => relationMapper.RegisterEntity(codePart, entity)).ToArray();
            var relations = MapRelations(result, relationMapper).ToArray();

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["TargetConnection"].ConnectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    foreach (var entity in entities)
                    {
                        var query = EntityMap.GetInsertQuery("Entities", entity.GetType());
                        db.Execute(query, entity, transaction);
                    }

                    foreach (var relation in relations)
                    {
                        var query = EntityMap.GetInsertQuery("Relations", relation.GetType());
                        db.Execute(query, relation, transaction);
                    }
                    transaction.Commit();
                }
            }
        }

        private static IEnumerable<EntityBase> MapEntities(AnalysisResult result, Func<ICodePart, EntityBase, EntityBase> afterMap)
        {
            var entityMapper = new EntityMapper();

            foreach (var sln in result.Solutions)
            {
                var solution = entityMapper.Map(sln);
                yield return afterMap(sln, solution);
                foreach (var p in sln.Projects)
                {
                    var project = entityMapper.Map(p);
                    yield return afterMap(p, project);
                    foreach (var c in p.AllClasses)
                    {
                        var @class = entityMapper.Map(c);
                        yield return afterMap(c, @class);
                        foreach (var m in c.Methods.Values.SelectMany(v => v))
                        {
                            var method = entityMapper.Map(m);
                            yield return afterMap(m, method);
                        }
                    }
                }
            }
        }

        private static IEnumerable<RelationBase> MapRelations(AnalysisResult result, RelationMapper relationMapper)
        {
            var context = new SystemAnalyzerContext("TargetConnection");
            var storedProcedures = context.Entities.OfType<StoredProcedureEntity>();
            var tables = context.Entities.OfType<TableEntity>();

            storedProcedures.ForEach(relationMapper.RegisterEntity);
            tables.ForEach(relationMapper.RegisterEntity);
            return result.Links.SelectMany(relationMapper.Map);
        }
    }
}