namespace FixJson
{

    using Newtonsoft.Json;

    using SampleDataUploader;

    using System;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using static System.Console;

    class Program
    {

        /*
         * FixJSON is program to fix mgmnt.json and properties.json files.
         * 
         * usage example: FixJson.exe path/to/input.json path/to/output.json -m or -p
         * -m for Management type
         * -p for Properties type
         */
        static async Task Main(string[] args)
        {
            try
            {
                WriteLine("This program will your json data.");
                WriteLine("Press any key to continue...");
                ReadKey();

                var filename = args[0];

                // replace new line (\r\n) with space and then replace \r with empty character. 
                var json = File.ReadAllText(filename).Replace(Environment.NewLine, " ").Replace("\r", "");


                // after replcing, some mgmtID field value end up with spaces, like 12 345, which is invalid json
                // replace spaces between numbers with empty characters.
                json = Regex.Replace(json, "(?<=\\d)\\s(?=\\d)", "", RegexOptions.Multiline);


                // remove space in properties.json file at lan and lat fields values
                var regex = "(?<=\\+|\\-|\\.|\\d|e|E)\\s(?=(\\d|\\+|\\.|e|E))";
                json = Regex.Replace(json, regex, "", RegexOptions.Multiline);

                // write to output file
                var outFile = args[1];
                using (var outFileStream = File.CreateText(outFile))
                {
                    await outFileStream.WriteAsync(json);
                }

                // verify if json is valid and parsable by deserializing the text with JSON.NET
                if (args[2] == "-m") // for managments data json file
                {
                    _ = JsonSerializer.Create().Deserialize<ManagementRoot[]>(new JsonTextReader(new StringReader(json)));
                } else if (args[2] == "-p") // for properties data json file
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
