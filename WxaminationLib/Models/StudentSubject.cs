using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationLib.Models
{
    public class Student
    {
        public string StudentFullName { get; set; }
        public List<Subject> Subjects { get; set; }
    }
}
