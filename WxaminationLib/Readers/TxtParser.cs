using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ExaminationLib.Helpers;
using ExaminationLib.Models;

namespace ExaminationLib.Readers
{
    public class TxtParser
    {
        private static ConfigurationModel ConfigurationModel;

        public TxtParser(ConfigurationModel configurationModel)
        {
            ConfigurationModel = configurationModel;
        }

        public Message ParseFromFile(out ICollection<Group> groups, out ICollection<MessageParser> parseErrList)
        {
            groups = new List<Group>();
            parseErrList = new List<MessageParser>();
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", "examination.txt");
            var success = FileIO.ReadFile(filePath, out string[] lines);

            if (!success)
                return new Message(false, $"File is either empty or does not exists. Path:{filePath}");

            Group group = null;
            foreach (var line in lines)
            {
                if (line.Contains(ConfigurationModel.Group))
                {
                    if (group != null && group.StudentSubjects.Count > 0)
                        groups.Add(group);

                    group = new Group(line.Trim());
                }
                else if (group != null && !String.IsNullOrWhiteSpace(line))
                {
                    var parseMessage = ParseGroup(line, ref group);
                    if (!parseMessage.Success)
                        parseErrList.Add(parseMessage);
                }
            }
            //last Group
            if (group != null && group.StudentSubjects.Count > 0)
            {
                groups.Add(group);
            }

            return new Message(true); ;
        }

        private MessageParser ParseGroup(string line, ref Group group)
        {
            var message = new MessageParser(true);
            foreach (var subject in ConfigurationModel.Subjects)
            {
                if (!line.Contains(subject))
                {
                    message.Success = false;
                    message.AddParseError($"Line missing subject:{subject}");
                }
            }

            var lineParts = line.Split(ConfigurationModel.PartingChar);
            if (lineParts.Length != ConfigurationModel.Subjects.Count() + 1)
            {
                message.Success = false;
                message.AddParseError($"Line has wrong number of items. Expected:{ConfigurationModel.Subjects.Count() + 1} Current:{lineParts.Length}");
                return message;
            }

            if (message.Success)
                ParseLine(ref group, ref message, lineParts);


            if (!message.Success)
            {
                message.AddErrorData(line);
            }

            return message;
        }

        private void ParseLine(ref Group group, ref MessageParser message, string[] lineParts)
        {
            var student = new Student();
            student.StudentFullName = lineParts[0];

            for (int i = 1; i < lineParts.Length; i++)
            {
                var subjectParts = lineParts[i].Trim().Split(ConfigurationModel.SubjectPartingChar);
                if (!ConfigurationModel.Subjects.Contains(subjectParts[0].Trim()))
                {
                    message.Success = false;
                    message.AddParseError($"Subject:{subjectParts[0].Trim()} is not in {string.Join(",", ConfigurationModel.Subjects)}");
                }
                if (subjectParts.Length != 2)
                {
                    message.Success = false;
                    message.AddParseError($"Parsing subject and rating unsucsessfull. Expected character between:{ConfigurationModel.SubjectPartingChar}");
                }

                int.TryParse(subjectParts[1], out int rating);
                if (rating < Constants.Constants.RATING_MIN || rating > Constants.Constants.RATING_MAX)
                {
                    message.Success = false;
                    message.AddParseError($"The rating value must be between {Constants.Constants.RATING_MIN} and {Constants.Constants.RATING_MAX}");
                }



                try
                {
                    if (student.Subjects == null)
                        student.Subjects = new List<Subject>();

                    student.Subjects.Add(new Subject { Name = subjectParts[0], Rating = rating });
                }
                catch { }

                if (message.Success)
                {
                    group.AddStudent(student);
                }
            }
        }


    }
}
