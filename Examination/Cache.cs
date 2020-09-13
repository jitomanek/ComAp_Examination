using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExaminationLib.Helpers;
using ExaminationLib.Models;

namespace Examination
{
    public class Cache
    {
        private static Cache Instance;
        private static List<Group> Groups;
        private static List<MessageParser> ParserErrors;

        private Cache()
        {
            Groups = new List<Group>();
            ParserErrors = new List<MessageParser>();
        }

        public static Cache GetInstance()
        {
            if (Instance == null)
                Instance = new Cache();

            return Instance;
        }

        public void AddGroups(IEnumerable<Group> groups)
        {
            if (groups != null)
                Groups.AddRange(groups);
        }

        public void AddParserErrors(IEnumerable<MessageParser> parseErrors)
        {
            if (parseErrors != null)
                ParserErrors.AddRange(parseErrors);
        }

        public ICollection<Group> GetGroups()
        {
            return Groups;
        }

        public ICollection<MessageParser> GetParseErrors()
        {
            return ParserErrors;
        }
    }
}
