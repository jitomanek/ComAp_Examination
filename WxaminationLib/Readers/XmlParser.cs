using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using ExaminationLib.Helpers;
using ExaminationLib.Models;

namespace ExaminationLib.Readers
{
    public class XmlParser : IParser
    {
        public Message ParseFromFile(string path, out ICollection<Group> groups)
        {
            throw new NotImplementedException();
        }

        public string ParseToFile(IEnumerable<Group> groups, IEnumerable<MessageParser> errors, string fileName, string fullPath)
        {
            ParseErrsToFile(errors, fileName, fullPath);

            if (groups == null || groups.Count() == 0)
                return null;

            var serializer = new XmlSerializer(typeof(Group[]));
            byte[] bytes;
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, groups.ToArray());
                bytes = Encoding.Unicode.GetBytes(writer.ToString());
            }

            if (fullPath == null)
                fullPath = Path.Combine(Directory.GetCurrentDirectory(), "Files", $"{fileName}.xml");

            FileIO.WriteFile(fullPath, bytes);
            return fullPath;
        }

        private void ParseErrsToFile(IEnumerable<MessageParser> errors, string fileName, string fullPath = null)
        {
            if (errors == null || errors.Count() == 0)
                return;

            var serializer = new XmlSerializer(typeof(MessageParser[]));
            byte[] bytes;
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, errors.ToArray());
                bytes = Encoding.Unicode.GetBytes(writer.ToString());
            }

            if (fullPath == null)
                fullPath = Path.Combine(Directory.GetCurrentDirectory(), "Files", $"{fileName}-{Constants.Constants.DEFAULT_REPORT_NAME}.xml");

            FileIO.WriteFile(fullPath, bytes);
        }
    }
}
