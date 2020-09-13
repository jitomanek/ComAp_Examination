using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationLib
{
    /// <summary>
    /// Represents data needed for reading input TxtFile
    /// </summary>
    public class ConfigurationModel
    {
        public string Group { get; set; }
        public char PartingChar { get; set; }
        public char SubjectPartingChar { get; set; }
        public IEnumerable<string> Subjects { get; set; }
    }
}
