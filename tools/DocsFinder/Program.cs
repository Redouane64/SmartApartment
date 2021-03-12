using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Nest;
using Newtonsoft.Json;
using SmartApartment.Common.Domains;

using static System.Console;

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

string keyword = "oak hill";
string[] markets = { "Atlanta", "Sacramento", "Los Angeles" };


// convert market terms to queries.
IEnumerable<Func<QueryContainerDescriptor<Document>, QueryContainer>>
 marketsQueryGenerator(string[] markets)
{
    foreach (var market in markets)
    {
        yield return new Func<QueryContainerDescriptor<Document>, QueryContainer>(
            q => q.MultiMatch(
                f => f.Fields(
                    m => m.Field(e => e.Property.Market)
                        .Field(e => e.Management.Market)
                ).Query(market)
            )
        );
    }
}

// search fields selector.
Func<FieldsDescriptor<Document>, IPromise<Fields>> fieldsSelector =
    f => f.Field(e => e.Property.Name)
                    .Field(e => e.Management.Name)
                    .Field(e => e.Property.FormerName)
                    .Field(e => e.Property.StreetAddress)
                    .Field(e => e.Property.State)
                    .Field(e => e.Management.State);

// keyword query.
Func<QueryContainerDescriptor<Document>, QueryContainer> keywordQuery =
    q => q.SimpleQueryString(
        s => s.Fields(fieldsSelector).Query(keyword)
    );

Func<SearchDescriptor<Document>, ISearchRequest> selector = s =>
s.Size(size)
 .Query(
     q => q.Bool(
         b => b.Should(
             keywordQuery
         ).Must(
             t => t.Bool(
                 b => b.Should(marketsQueryGenerator(markets))
             )
         )
     )
);

var result = await client.SearchAsync<Document>(selector);

if (!result.IsValid)
{
    WriteLine("Error.");
}

var sb = new StringBuilder();
var settings = new JsonSerializerSettings()
{
    NullValueHandling = NullValueHandling.Ignore
};

foreach (var document in result.Documents)
{
    sb.AppendLine(JsonConvert.SerializeObject(
        document,
        Formatting.Indented,
        settings
    ));
}

// WriteLine(sb.ToString());

System.IO.File.WriteAllText("search_output.txt", sb.ToString());
