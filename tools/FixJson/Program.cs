using Newtonsoft.Json;

using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FixJson
{

    using static System.Console;

    class Program
    {

        /*
         * FixJSON is program to fix mgmnt.json and properties.json files.
         * 
         * usage example: FixJson.exe path/to/input.json path/to/output.json -m or -p
         * 
         */
        static async Task Main(string[] args)
        {
            var filename = args[0];

            try
            {

                // replace new line (\r\n) with space and then replace \r with empty character. 
                var json = File.ReadAllText(filename).Replace(Environment.NewLine, " ").Replace("\r", "");


                // after replcing, some mgmtID field value end up with spaces, like 12 345, which is invalid json
                // replace spaces between numbers with empty characters.
                json = Regex.Replace(json, "(?<=\\d)\\s(?=\\d)", "", RegexOptions.Multiline);

                // json = Regex.Replace(json, "(?<=\\d)\\s(?=e)", "", RegexOptions.Multiline);

                // remove space in properties.json file at lan and lat fields values
                json = Regex.Replace(json, "(?<=(\\+|\\-|.|\\d))\\s(?=(\\d|\\+|.|e))", "", RegexOptions.Multiline);

                // write to output file
                var outFile = args[1];
                using (var outFileStream = File.CreateText(outFile))
                {
                    await outFileStream.WriteAsync(json);
                }

                // verify if json is valid and parsable by deserializing the text with JSON.NET
                if (args[2] == "-m") // for managments data json file
                {
                    _ = JsonSerializer.Create().Deserialize<MgmtRoot[]>(new JsonTextReader(new StringReader(json)));
                }

                if (args[2] == "-p") // for properties data json file
                {
                    _ = JsonSerializer.Create().Deserialize<PropertyRoot[]>(new JsonTextReader(new StringReader(json)));
                }
            }
            catch (Exception ex)
            {
                WriteLine($"Error: {ex.Message}");
            }
        }
    }

}
