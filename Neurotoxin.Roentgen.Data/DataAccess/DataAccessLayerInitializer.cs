using System.Data.Entity;

namespace Neurotoxin.Roentgen.Data.DataAccess
{
    public class DataAccessLayerInitializer : DropCreateDatabaseIfModelChanges<SystemAnalyzerContext>
    {
        public override void InitializeDatabase(SystemAnalyzerContext context)
        {
            base.InitializeDatabase(context);

            context.Database.ExecuteSqlCommand("IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_Discriminator' AND object_id = OBJECT_ID('Entities')) BEGIN CREATE NONCLUSTERED INDEX [IX_Discriminator] ON [dbo].[Entities] ([Discriminator] ASC) END");
            context.Database.ExecuteSqlCommand("IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_Discriminator' AND object_id = OBJECT_ID('Relations')) BEGIN CREATE NONCLUSTERED INDEX [IX_Discriminator] ON [dbo].[Relations] ([Discriminator] ASC) END");

            ////SP DDL

            //var r = new Regex("[\n\r]GO[\n\r]", RegexOptions.Singleline);
            //var assembly = GetType().Assembly;
            //var manifestNames = assembly.GetManifestResourceNames();
            //foreach (var name in manifestNames)
            //{
            //    using (var stream = assembly.GetManifestResourceStream(name))
            //    {
            //        using (var sr = new StreamReader(stream))
            //        {
            //            var sql = sr.ReadToEnd();
            //            foreach (var sqlCommand in r.Split(sql).Where(s => !string.IsNullOrWhiteSpace(s)))
            //            {

            //                context.Database.ExecuteSqlCommand(sqlCommand);
            //            }
            //        }
            //    }
            //}
        }
    }
}