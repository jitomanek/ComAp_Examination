using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExaminationLib.Helpers;
using ExaminationLib.Models;

namespace ExaminationLib
{
    public class Statistic
    {
        public static double StudentWeightAvg(Student student)
        {
            //Math 40%, Physics 35%, English 25%
            double math = 0.4;
            double physics = 0.35;
            double english = 0.25;

            double result =
                student.Subjects.FirstOrDefault(x => x.Name.ToLower() == "math").Rating * math +
                student.Subjects.FirstOrDefault(x => x.Name.ToLower() == "physics").Rating * physics +
                student.Subjects.FirstOrDefault(x => x.Name.ToLower() == "english").Rating * english;

            return result;
        }

        public static SubjectStatistic SubjectStatistic(IEnumerable<Subject> subjects, string group = null)
        {
            var values = subjects.Select(x => x.Rating).ToList();
            if (values.Count == 0)
            {
                return new SubjectStatistic();
            }

            var modus = Modus(values);
            var median = Median(values);
            var average = Average(values);

            return new SubjectStatistic { Average = average, Median = median, Modus = modus, SubjectName = subjects.FirstOrDefault().Name, GroupName = group };
        }

        private static double Average(List<int> values)
        {
            return values.Sum() / values.Count;
        }
        private static int Median(List<int> values)
        {
            values.Sort();
            int middle = values.Count / 2;

            if (values.Count % 2 == 0)
            {
                return (values[middle] + values[middle + 1]) / 2;
            }
            else
            {
                return values[(middle) + 1];
            }
        }

        private static int[] Modus(List<int> values)
        {
            var query = values.GroupBy(x => x)
               .OrderByDescending(x => x.Count());

            var highestCount = query.FirstOrDefault().Count();

            return query
                .Where(x => x.Count() == highestCount)
                .Select(x => x.Key)
                .ToArray();
        }
    }
}
