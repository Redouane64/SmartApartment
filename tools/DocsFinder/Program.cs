namespace DocsFinder
{
    using System;
    using System.Threading.Tasks;

    using Elasticsearch.Net;

    using Nest;

    using SmartApartment.Common.Domains;

    using static System.Console;

    class Program
    {
        static async Task Main(string[] args)
        {
            ConnectionSettings connection = new ConnectionSettings();

            if (args.Length == 1)
            {
                connection = new ConnectionSettings(new Uri(args[0]));
            }


            /* push properties and management entities to the same index named `default` */
            connection.DefaultIndex("default")
                      .PrettyJson();

            var client = new ElasticClient(connection);

            string keyword = "oak";
            string market = "San Francisco";

            var result = await client.SearchAsync<Document>(
                s => s.Index("documents")
                        .Size(25)
                        .Query(
                            q => q.MultiMatch(
                                m => m.Fields(
                                    f => f.Field(e => e.Management.Name)
                                          .Field(e => e.Property.Name)
                                ).Query(keyword)
                            )
                        ));


            /*
            var result = await client.SearchAsync<Document>(s =>
                s.Query(
                    q => q.MultiMatch(
                        m => m.Query("oak")
                    )
                )
            );
            */

            if (!result.IsValid)
            {
                WriteLine("Error.");
            }

            WriteLine($"Found {result.Documents.Count} document(s).");

            
        }
    }
}
