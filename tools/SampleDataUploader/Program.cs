
using System;
using System.IO;
using System.Linq;

using Nest;
using Newtonsoft.Json;
using SmartApartment.Common.Domains;
using SmartApartment.Common.Services;

using static System.Console;


string mgmntJsonFile = null;
string propertiesJsonFile = null;
string uri = null;
string indexName = null;

try
{
    mgmntJsonFile = args[0];
    propertiesJsonFile = args[1];
    uri = args[2];
    indexName = args[3];
}
catch
{
    Error.WriteLine("Incorrect program arguments.");
}

WriteLine("This program will parse and index your json data to Elasticsearch instance.");
WriteLine("Press any key to continue...");
ReadKey();

/* push properties and management entities to the provided index name */
ConnectionSettings connection = new ConnectionSettings()
    .PrettyJson()
    .DefaultIndex(indexName);

var client = new ElasticClient(connection);
var serializer = JsonSerializer.Create(new JsonSerializerSettings()
{
    FloatParseHandling = FloatParseHandling.Double
});

#region Parse JSON files
Write("Parsing files...");
var managementRoots = serializer.Deserialize<Document[]>(
    new JsonTextReader(
        new StringReader(File.ReadAllText(mgmntJsonFile))
    )
);

var propertyRoots = serializer.Deserialize<Document[]>(
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

// filter out data with empty id.
var managements = managementRoots?.Where(d => d.Id != null).ToArray();
var properties = propertyRoots.Where(d => d.Id != null).ToArray();

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

WriteLine("Tasks completed.");
ReadKey();
