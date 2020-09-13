using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExaminationLib.Constants;
using ExaminationLib.Readers;

namespace Examination
{
    public class CommandLineInterface
    {
        private static ParserSelector ParserSelector;
        public static void StartMessage()
        {
            Console.WriteLine($"Examination v1.0 {DateTime.Today.ToString("dd.MM.yyyy")}");
            Console.WriteLine($"For command examples write help.");
            Console.WriteLine($"To quit press enter.");
        }

        public static void ProccesCommand(string command)
        {
            var parts = command.Split(' ');
            if (!Enum.TryParse(parts[0].Trim().ToLower(), out Commands commands))
            {
                Console.WriteLine(string.Format("Command:\"{0}\" not recognized", parts[0]));
                Console.WriteLine($"Supported commands:[{GetEnumValues<Commands>()}] <action>");
            }

            switch (commands)
            {
                case Commands.get:
                    ProccesGet(parts);
                    break;
                case Commands.help:
                    ProccessHelp();
                    break;
                case Commands.save:
                    ProccesSave(parts);
                    break;
                case Commands.none:
                    break;
                default:
                    Console.WriteLine();
                    break;
            }
        }

        private static void ProccesGet(string[] parts)
        {
            if (CheckMultipleCommand<StatisticAction>(parts))
            {

            }
        }

        private static void ProccesSave(string[] parts)
        {
            if (CheckMultipleCommand<Formats>(parts))
            {
                if (ParserSelector == null)
                    ParserSelector = new ParserSelector();

                if (Enum.TryParse(parts[1], out Formats action))
                {
                    var message = ParserSelector.ParseToFile(action, Cache.GetInstance().GetGroups(), Cache.GetInstance().GetParseErrors());
                    Program.PrintMessage(message);
                }
                else
                {
                    Console.WriteLine($"Format:{parts[1]} unknown, supported formats:{GetEnumValues<Formats>()}");
                }
            }
        }

        private static bool CheckMultipleCommand<T>(string[] parts) where T : Enum
        {
            if (parts.Length < 2)
            {
                Console.WriteLine("Command does not contain any action.");
                return false;
            }
            else if (!Enum.IsDefined(typeof(T), parts[1]))
            {
                Console.WriteLine(string.Format("Command action:\"{0}\" not recognized", parts[1]));
                Console.WriteLine($"Supported command actions:{GetEnumValues<T>()}");
                return false;
            }

            return true;
        }

        private static void ProccessHelp()
        {
            Console.WriteLine("For student weight average type: get student <studentFullName>");
            Console.WriteLine("For subject statistic type: get subject <subjectName> [<groupName>:optional]");
            Console.WriteLine("For file write type: save <fileFormat[json/xml]>");
        }

        private static string GetEnumValues<T>() where T : Enum
        {
            StringBuilder builder = new StringBuilder();
            foreach (var com in Enum.GetValues(typeof(T)))
                builder.Append(", " + com);

            return builder.ToString().Remove(0, 2);
        }
    }
}
