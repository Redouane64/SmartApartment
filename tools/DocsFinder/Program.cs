using System;
using System.Text;

using Nest;
using Newtonsoft.Json;
using SmartApartment.Common.Services;

string uri = "http://localhost:9200";
string index = "documents";
int size = 25;

if (args.Length > 0)
{
    try
    {
        uri = args[0];
        index = args[1];
    }
    catch { }
}

var connection = new ConnectionSettings(new Uri(uri))
    .DefaultIndex(index)
    .PrettyJson();

var client = new ElasticClient(connection);
var searchService = new SearchService(client);

string keyword = "oak and hill";
string markets = "Atlanta, Sacramento,Los Angeles";

var documents = await searchService.Search(keyword, markets, size);

var sb = new StringBuilder();
var settings = new JsonSerializerSettings()
{
    NullValueHandling = NullValueHandling.Ignore
};

sb.AppendLine(JsonConvert.SerializeObject(
    documents,
    Formatting.Indented,
    settings
));

System.IO.File.WriteAllText("search_output.txt", sb.ToString());
