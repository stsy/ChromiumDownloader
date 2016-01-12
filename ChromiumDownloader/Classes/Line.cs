using System.IO;

namespace ChromiumDownloader.Classes
{
    class Line
    {
        static public void Write(string input, string path, string filename)
        {
            using (var sw = new StreamWriter(path + filename, false))
            {
                sw.WriteLine(input);
            }
        }

        static public string Read(string filePath)
        {
            if (File.Exists(filePath))
            {
                using (var sr = new StreamReader(filePath))
                {
                    return sr.ReadLine();
                }
            }
            else
            {
                return null;
            }

        }
    }
}
