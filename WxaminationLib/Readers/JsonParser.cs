using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ExaminationLib.Helpers;
using ExaminationLib.Models;
using Newtonsoft.Json;

namespace ExaminationLib.Readers
{
    public class JsonParser : IParser
    {

        public Message ParseFromFile(string path, out ICollection<Group> groups)
        {
            groups = new List<Group>();

            if (!FileIO.ReadFile(path, out string[] lines))
            {
                return new Message(false, $"File:{path} not found.");
            }
            groups = JsonConvert.DeserializeObject<List<Group>>(string.Join(string.Empty, lines));

            return new Message(true);
        }

        public string ParseToFile(IEnumerable<Group> groups, IEnumerable<MessageParser> errors, string fileName=Constants.Constants.DEFAULT_FILE_NAME, string fullPath = null)
        {
            ParseErrsToFile(errors, fileName, fullPath);

            if (groups == null || groups.Count() == 0)
                return null;

            var jsonString = JsonConvert.SerializeObject(groups);
            var bytes = Encoding.Unicode.GetBytes(jsonString);

            if (fullPath == null)
                fullPath = Path.Combine(Directory.GetCurrentDirectory(), "Files", $"{fileName}.json");

            FileIO.WriteFile(fullPath, bytes);
            return fullPath;
        }

        private void ParseErrsToFile(IEnumerable<MessageParser> errors, string fileName, string fullPath = null)
        {
            if (errors == null || errors.Count() == 0)
                return;

            var jsonString = JsonConvert.SerializeObject(errors);
            var bytes = Encoding.Unicode.GetBytes(jsonString);

            if (fullPath == null)
                fullPath = Path.Combine(Directory.GetCurrentDirectory(), "Files", $"{fileName}-{Constants.Constants.DEFAULT_REPORT_NAME}.json");

            FileIO.WriteFile(fullPath, bytes);
        }
    }
}
