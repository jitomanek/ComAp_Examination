using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationLib.Models
{
    public class SubjectStatistic
    {
        public string SubjectName { get; set; }
        public string GroupName { get; set; }
        public double Average { get; set; }
        public double Median { get; set; }
        public int[] Modus { get; set; }
    }
}
