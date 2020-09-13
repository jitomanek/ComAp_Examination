using System;
using System.Collections.Generic;
using System.Text;
using ExaminationLib.Helpers;
using ExaminationLib.Models;

namespace ExaminationLib.Readers
{
    interface IParser
    {
        /// <summary>
        /// Parses data from file
        /// </summary>
        /// <param name="groups"></param>
        /// <param name="parseErrList"></param>
        /// <returns></returns>
        Message ParseFromFile(string path, out ICollection<Group> groups);

        /// <summary>
        /// Parses data to file. Returns fullPath to file
        /// </summary>
        /// <param name="groups"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        string ParseToFile(IEnumerable<Group> groups, IEnumerable<MessageParser> errors, string fileName = Constants.Constants.DEFAULT_FILE_NAME, string fullPath = null);
    }
}
