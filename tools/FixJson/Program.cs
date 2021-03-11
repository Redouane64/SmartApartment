using System;
using System.IO;
using System.Text.RegularExpressions;

using Newtonsoft.Json;
using SmartApartment.Common.Domains;

using static System.Console;

/*
 * FixJSON is program to fix mgmnt.json and properties.json files.
 * -m for Management type
 * -p for Properties type
 * 
 * USAGE: 
 *      FixJson.exe path/to/input.json path/to/output.json -m
 *      dotnet run -p ./tools/FixJson -- path/to/input.json path/to/output.json -m
 */
string filename = null;
string outFile = null;
string type = null;

try
{
    filename = args[0];
    outFile = args[1];
    // input JSON data type: -p = Properties, -m = Managements
    type = args[3];
}
catch
{
    Error.WriteLine("Incorrect program arguments.");
}

WriteLine("This program will fix json sample data.");
WriteLine("Press any key to continue...");
ReadKey();

// replace new line (\r\n) with space and then replace \r with empty character. 
var json = File.ReadAllText(filename).Replace(Environment.NewLine, " ").Replace("\r", "");


// some mgmtID field value end up with spaces, like "12 345"
// which is invalid json int value.
// replace spaces between numbers with empty characters.
var spaceBetweenNumbersRegex = "(?<=\\d)\\s(?=\\d)";
json = Regex.Replace(json, spaceBetweenNumbersRegex, String.Empty, RegexOptions.Multiline);


// remove space in properties.json file at lan and lat fields values
var spaceInLatAndLngValuesRegex = "(?<=\\+|\\-|\\.|\\d|e|E)\\s(?=(\\d|\\+|\\.|e|E))";
json = Regex.Replace(json, spaceInLatAndLngValuesRegex, String.Empty, RegexOptions.Multiline);

// write to output file
using (var outFileStream = File.CreateText(outFile))
{
    await outFileStream.WriteAsync(json);
}

// verify if json is valid and parsable by deserializing the text with JSON.NET
var serializer = JsonSerializer.Create();

try
{
    // for managements data json file
    if (type.Equals("-m", StringComparison.OrdinalIgnoreCase))
    {
        _ = serializer.Deserialize<ManagementRoot[]>(new JsonTextReader(new StringReader(json)));

    }
    // for properties data json file
    else if (type.Equals("-p", StringComparison.OrdinalIgnoreCase))
    {
        _ = serializer.Deserialize<PropertyRoot[]>(new JsonTextReader(new StringReader(json)));
    }
}
catch(Exception ex)
{
    Error.WriteLine($"Unable to parse JSON files: {ex.Message}");
}

