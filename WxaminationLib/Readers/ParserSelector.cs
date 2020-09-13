using System;
using System.Collections.Generic;
using System.Text;
using ExaminationLib.Constants;
using ExaminationLib.Helpers;
using ExaminationLib.Models;

namespace ExaminationLib.Readers
{
    public class ParserSelector
    {
        private readonly IParser xmlParser;
        private readonly IParser jsonParser;

        public ParserSelector()
        {
            xmlParser = new XmlParser();
            jsonParser = new JsonParser();
        }

        public Message ParseFromFile(Formats format, string path, out ICollection<Group> groups)
        {
            switch (format)
            {
                case Formats.json:
                    return jsonParser.ParseFromFile(path, out groups);
                case Formats.xml:
                    return xmlParser.ParseFromFile(path, out groups);
                default:
                    groups = new Group[0];
                    return new Message(false, $"Format:{format} not implemented!");
            }
        }
        public Message ParseToFile(Formats format, IEnumerable<Group> groups, IEnumerable<MessageParser> errorMessages)
        {
            string fullpath = null;
            switch (format)
            {
                case Formats.json:
                    fullpath = jsonParser.ParseToFile(groups, errorMessages);
                    break;
                case Formats.xml:
                    fullpath = xmlParser.ParseToFile(groups, errorMessages);
                    break;
                default:
                    return new Message(false, $"Format:{format} not implemented!");
            }

            if (fullpath == null)
                return new Message(false, $"No file created - empty input!");

            return new Message(true, $"File created:\"{fullpath}\"");
        }
    }
}
