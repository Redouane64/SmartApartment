namespace SampleDataUploader
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Nest;

    using Newtonsoft.Json;

    using SmartApartment.Common.Domains;
    using SmartApartment.Common.Services;

    using static System.Console;

    class Program
    {
        /*
         * 
         */
        static async Task Main(string[] args)
        {
            WriteLine("This program will parse and index your json data to Elasticsearch intance.");
            WriteLine("Press any key to continue...");
            ReadKey();

            ConnectionSettings connection = new ConnectionSettings()
                .PrettyJson()
                .DefaultIndex(args[2]);

            string mgmntJsonFile = args[0];
            string propertiesJsonFile = args[1];
            string indexName = args[2];

            /* push properties and management entities to the same index named `documents` */


            var client = new ElasticClient(connection);

            #region Parse JSON files
            Write("Parsing files...");
            var managementRoots = JsonSerializer.Create().Deserialize<Document[]>(
                new JsonTextReader(
                        new StringReader(File.ReadAllText(mgmntJsonFile))
                    )
                );

            var propertyRoots = JsonSerializer.Create(new JsonSerializerSettings()
            {
                FloatParseHandling = FloatParseHandling.Double
            }).Deserialize<Document[]>(
                new JsonTextReader(
                        new StringReader(File.ReadAllText(propertiesJsonFile))
                    )
                );
            WriteLine("DONE!");
            #endregion

            #region Upload data to Elastic instance
            WriteLine("Uploading Managements documents...");

            var indexingService = new IndexingService(client);

            Write($"Creating index '{indexName}'...");
            await indexingService.CreateIndex(indexName);
            WriteLine("DONE!");

            var managements = managementRoots?.Where(d => d.Id != null).ToArray();

            foreach (var item in managements)
            {
                var mResult = await indexingService.IndexDocument(item);

                if (mResult.IsValid)
                {
                    WriteLine($"Document Id '{mResult.Id}' uploaded successfully!");
                }
                else
                {
                    WriteLine($"Document Id '{mResult.Id}' FAILED!");
                }
            }

            Write("Uploading Properties documents...");

            var properties = propertyRoots.Where(d => d.Id != null).ToArray();

            foreach (var item in properties)
            {
                var mResult = await indexingService.IndexDocument(item);

                if (mResult.IsValid)
                {
                    WriteLine($"Document Id '{mResult.Id}' uploaded successfully!");
                }
                else
                {
                    WriteLine($"Document Id '{mResult.Id}' FAILED!");
                }
            }
            #endregion

            WriteLine("Tasks completed successfully.");
            ReadKey();

        }
    }
}
