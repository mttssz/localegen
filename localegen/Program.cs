using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace localegen
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("You must specify an input, an output file and a format");
                return;
            }

            string inputFile = args[0];
            string outputFile = args[1];
            string format = args[2];

            using (var wb = new XLWorkbook(inputFile))
            {
                var ws = wb.Worksheet(1);
                int width = ws.LastColumnUsed().ColumnNumber();
                int height = ws.LastRowUsed().RowNumber();

                var countryCodes = new List<string>();
                var localeString = new Dictionary<string, Dictionary<string, string>>();

                for(int i = 2; i <= width; i++)
                {
                    countryCodes.Add((string)ws.Cell(1, i).Value);
                }

                for(int y = 2; y <= height; y++)
                {
                    string identifier = (string)ws.Cell(y, 1).Value;
                    localeString[identifier] = new Dictionary<string, string>();

                    for(int x = 2; x <= width; x++)
                    {
                        localeString[identifier][countryCodes[x - 2]] = (string)ws.Cell(y, x).Value;
                    }
                }
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
