using System;
using System.IO;

namespace ExaminationLib.Readers
{
    public class FileIO
    {
        public static bool ReadFile(string path, out string[] fileContent)
        {
            fileContent = new string[0];

            if (!File.Exists(path))
                return false;

            fileContent = File.ReadAllLines(path);
            return true;
        }

        public static void WriteFile(string path, byte[] content)
        {
            if (File.Exists(path))
            {
                var parts = path.Split('.');
                parts[0] = parts[0] + DateTime.UtcNow.ToString("yyyyMMddhhmmss");
                File.Move(path, string.Join(".", parts));
            }

            int i = 0;
            while (File.Exists(path) && i < 20)
            {
                System.Threading.Thread.Sleep(50);
                i++;
            }

            //If original file still exists overwrite
            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
              
                stream.Write(content, 0, content.Length);
                stream.Flush();
            }
        }
    }
}
