using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExaminationLib.Models
{
    public class Group
    {
        public string Name { get; set; }
        public List<Student> StudentSubjects { get; set; }

        public Group() { }
        public Group(string name)
        {
            this.Name = name;
            this.StudentSubjects = new List<Student>();
        }

        public Group(Group group)
        {
            this.Name = group.Name;
            this.StudentSubjects = new List<Student>();
        }

        public bool AddStudent(Student studentSubject)
        {
            var check = StudentSubjects.Any(x => x.StudentFullName.ToLower() == studentSubject.StudentFullName.Trim().ToLower());

            if (!check)
            {
                StudentSubjects.Add(new Student
                {
                    StudentFullName = studentSubject.StudentFullName.Trim(),
                    Subjects = studentSubject.Subjects
                });
            }


            return check;
        }
    }
}
