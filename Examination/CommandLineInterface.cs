using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExaminationLib;
using ExaminationLib.Constants;
using ExaminationLib.Models;
using ExaminationLib.Readers;

namespace Examination
{
    public class CommandLineInterface
    {
        private static ParserSelector ParserSelector;
        private static ConfigurationModel ConfigurationModel;
        public static void StartMessage()
        {
            Console.WriteLine($"Examination v1.0 {DateTime.Today.ToString("dd.MM.yyyy")}");
            Console.WriteLine($"For command examples write help.");
            Console.WriteLine($"To quit press enter.");
        }

        public static void ProccesCommand(string command, ConfigurationModel configurationModel)
        {
            if (ConfigurationModel == null)
            {
                ConfigurationModel = configurationModel;
            }

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
                if (Enum.TryParse(parts[1], out StatisticAction action))
                    switch (action)
                    {
                        case StatisticAction.student:
                            ProccesStudent(parts);
                            break;
                        case StatisticAction.subject:
                            ProccessSubject(parts);
                            break;
                        default:
                            Console.WriteLine($"Invalid command!");
                            break;
                    }
            }
        }

        private static void ProccessSubject(string[] parts)
        {
            var data = Cache.GetInstance().GetGroups();

            var subjectsStatistic = new List<SubjectStatistic>();
            foreach (var subject in ConfigurationModel.Subjects)
            {
                var subjectData = Cache.GetInstance().GetGroups().Select(x => new { GroupName = x.Name, StudentSubject = x.StudentSubjects.SelectMany(q => q.Subjects.Where(z => z.Name == subject)) });
                if (parts.Length == 2)
                {
                    var subjectStatistic = Statistic.SubjectStatistic(subjectData.SelectMany(x => x.StudentSubject));
                    subjectsStatistic.Add(subjectStatistic);
                }
                else if (parts.Length == 3)
                {
                    foreach (var group in subjectData)
                    {
                        var subjectStatistic = Statistic.SubjectStatistic(subjectData.FirstOrDefault(q => q.GroupName == group.GroupName).StudentSubject, group.GroupName);
                        subjectsStatistic.Add(subjectStatistic);
                    }
                }
            }

            subjectsStatistic = subjectsStatistic.OrderBy(x => x.GroupName).ToList();
            foreach (var subjectStat in subjectsStatistic)
            {
                string group = subjectStat.GroupName != null ? $"Group:{subjectStat.GroupName}" : string.Empty;
                Console.WriteLine($"Subject:{subjectStat.SubjectName} {group} Average:{subjectStat.Average} Median:{subjectStat.Median} Modus:{string.Join(", ", subjectStat.Modus)}");
            }
        }

        private static void ProccesStudent(string[] parts)
        {
            if (parts.Length < 3)
                Console.WriteLine("Command has no parameter.");
            if (parts.Length != 4)
            {
                Console.WriteLine("Student must have First and LastName");
                return;
            }

            var student = Cache.GetInstance().GetGroups()
                .Select(x => x.StudentSubjects.FirstOrDefault(q => q.StudentFullName.ToLower() == $"{parts[2]} {parts[3]}"))
                .FirstOrDefault();

            if (student != null)
            {
                var avg = Statistic.StudentWeightAvg(student);
                Console.WriteLine($"Student:\"{student.StudentFullName}\" weight average:{avg}");
            }
            else
            {
                Console.WriteLine($"Student:{parts[2]} {parts[3]} not found.");
            }
        }

        private static void ProccesSave(string[] parts)
        {
            if (ParserSelector == null)
                ParserSelector = new ParserSelector();

            if (parts.Length == 1)
            {
                var message = ParserSelector.ParseToFile(Formats.xml, Cache.GetInstance().GetGroups(), Cache.GetInstance().GetParseErrors());
                Program.PrintMessage(message);
                return;
            }
            else if (Enum.TryParse(parts[1], out Formats action))
            {
                var message = ParserSelector.ParseToFile(action, Cache.GetInstance().GetGroups(), Cache.GetInstance().GetParseErrors());
                Program.PrintMessage(message);
            }
            else
            {
                Console.WriteLine($"Format:{parts[1]} unknown, supported formats:{GetEnumValues<Formats>()}");
            }

        }

        private static bool CheckMultipleCommand<T>(string[] parts, int partsCount = 2) where T : Enum
        {
            if (parts.Length < partsCount)
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
            Console.WriteLine("For subjects statistics type: get subject [group]");
            Console.WriteLine("For file write type: save [json/xml]");
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
