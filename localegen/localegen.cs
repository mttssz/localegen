using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace localegen
{
    internal static class localegen
    {
        private static void WaitForKey()
        {
#if DEBUG
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
#endif
        }

        private static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("You must specify an input, an output file and a format. Aborting.");
                WaitForKey();
                return;
            }

            string inputFile = args[0];
            string outputFile = args[1];

            if (!inputFile.EndsWith(".xls") && !inputFile.EndsWith(".xlsx"))
            {
                Console.WriteLine("The input file must be an .xls or an .xlsx file. Aborting.");
                WaitForKey();
                return;
            }

            if (!outputFile.EndsWith(".js") && !outputFile.EndsWith(".json"))
            {
                Console.WriteLine("The input file must be a .js or a .json file. Aborting.");
                WaitForKey();
                return;
            }

            if(!File.Exists(inputFile))
            {
                Console.WriteLine($"The file {Path.GetFullPath(inputFile)} does not exist. Aborting.");
                WaitForKey();
                return;
            }

            Console.WriteLine($"Input file: {Path.GetFullPath(inputFile)}");
            Console.WriteLine($"Output file: {Path.GetFullPath(outputFile)}");

            var localeString = new Dictionary<string, Dictionary<string, string>>();

            Console.WriteLine("Starting generation.");

            using (var wb = new XLWorkbook(inputFile))
            {
                var ws = wb.Worksheet(1);
                int width = ws.LastColumnUsed().ColumnNumber();
                int height = ws.LastRowUsed().RowNumber();

                var countryCodes = new List<string>();

                for (int i = 2; i <= width; i++)
                    countryCodes.Add((string)ws.Cell(1, i).Value);

                for (int y = 2; y <= height; y++)
                {
                    string identifier = (string)ws.Cell(y, 1).Value;

                    if(localeString.TryGetValue(identifier, out _))
                        Console.WriteLine($"WARNING: Key {identifier} already exists; Replacing.");

                    localeString[identifier] = new Dictionary<string, string>();

                    for (int x = 2; x <= width; x++)
                    {
                        localeString[identifier][countryCodes[x - 2]] = (string)ws.Cell(y, x).Value;
                    }
                }
            }

            bool isJson = outputFile.EndsWith(".json");

            using (var file = File.CreateText(outputFile))
            {
                file.WriteLine(isJson ? "{" : "module.exports = {");

                int idCount = 0;
                foreach (var id in localeString)
                {
                    idCount++;
                    file.Write("    ");
                    file.Write($"'{id.Key}': {{\r\n");
                    int count = 0;
                    foreach (var lang in id.Value)
                    {
                        count++;
                        file.Write("        ");
                        file.Write($"'{lang.Key}': '{lang.Value}'");

                        if (count != id.Value.Count)
                            file.Write(",");

                        file.Write("\r\n");
                    }
                    file.Write("    }");
                    if (idCount != localeString.Count)
                        file.Write(",");

                    file.Write("\r\n");
                }

                file.WriteLine("}");
            }

            Console.WriteLine("Locale file was generated successfully.");

            WaitForKey();
        }
    }
}
