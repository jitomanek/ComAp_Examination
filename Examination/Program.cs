using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExaminationLib;
using ExaminationLib.Helpers;
using ExaminationLib.Models;
using ExaminationLib.Readers;

namespace Examination
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ConfigurationModel configurationModel = null;
                var message = Configuration.GetConfiguration(out configurationModel);
                if (!message.Success)
                {
                    PrintMessage(message);
                }
                else
                {
                    var txtParser = new TxtParser(configurationModel);
                    var msg = txtParser.ParseFromFile(out ICollection<Group> groups, out ICollection<MessageParser> parseErrs);
                    Cache.GetInstance().AddGroups(groups);
                    Cache.GetInstance().AddParserErrors(parseErrs);
                }

                CommandLineInterface.StartMessage();
                string command = Commands.none.ToString();
                while (!String.IsNullOrEmpty(command))
                {
                    command = Console.ReadLine();
                    CommandLineInterface.ProccesCommand(command);
                }

            }
            catch (Exception ex)
            {
                PrintExceptions(ex);
                Console.ReadKey();
            }
        }

        public static void PrintMessage(Message message)
        {
            Console.WriteLine(message.Content);
            if (message.Exception != null)
                PrintExceptions(message.Exception);
        }

        private static void PrintExceptions(Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
            if (ex.InnerException != null)
                PrintExceptions(ex.InnerException);

        }
    }
}
