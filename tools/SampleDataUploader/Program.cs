namespace SampleDataUploader
{
    using System;
    using System.IO;

    using Nest;

    using Newtonsoft.Json;

    using static System.Console;

    class Program
    {
        /*
         * 
         */
        static void Main(string[] args)
        {
            WriteLine("This program will parse and index your json data to Elasticsearch intance.");
            WriteLine("Press any key to continue...");
            ReadKey();

            ConnectionSettings connection = new ConnectionSettings();

            string mgmntJsonFile = args[0];
            string propertiesJsonFile = args[1];

            if (args.Length > 2)
            {
                connection = new ConnectionSettings(new Uri(args[2]));
            }
            

            /* push properties and management entities to the same index named `default` */
            connection.DefaultIndex("default")
                      .PrettyJson();

            try
            {
                var client = new ElasticClient(connection);

                var managements = JsonSerializer.Create().Deserialize<ManagementRoot[]>(
                    new JsonTextReader(
                            new StringReader(File.ReadAllText(mgmntJsonFile))
                        )
                    );

                var properties = JsonSerializer.Create(new JsonSerializerSettings() { 
                    FloatParseHandling = FloatParseHandling.Double
                }).Deserialize<PropertyRoot[]>(
                    new JsonTextReader(
                            new StringReader(File.ReadAllText(propertiesJsonFile))
                        )
                    );

                ReadKey();
            }
            catch (Exception ex)
            {
                Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
