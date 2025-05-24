using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CTTE
{



    /// <summary>
    /// 
    /// </summary>
    public class DirectoryManager
    {
        const string upperDirectoryPattern = @"^.+(?=\\[^\\]+$)";  // this can be not cross-platform
        const string cdPattern = @"(^[.]{2}|\\\.\.)";
        const string lastDirectoryPattern = @"[^\\.]+$";

        readonly RegexOptions lastDirectoryOptions = RegexOptions.RightToLeft;
        public string ChangeDirectory(string path)
        {
            //    ....\.. cd's 2 up

            Console.Write("directory: ");
            string? dir = Console.ReadLine().Trim();

            if (dir == "") return path;
            if (dir.Length >= 2 && dir.Substring(0, 2) == "..")
            {
                int occurences = Regex.Matches(dir, cdPattern).Count;
                if (dir.Length > 2 && occurences == 1) Console.WriteLine("such directory doesnt exist");
                if (path != "C:") for (int i = 0; i < occurences; i++) path = Regex.Match(path, upperDirectoryPattern).Value;

                Match match = Regex.Match(dir, lastDirectoryPattern, lastDirectoryOptions);
                if (match.Success && Directory.Exists(Path.Combine(path, match.Value))) path = Path.Combine(path, match.Value);
            }
            else if (Directory.Exists(Path.Combine(path, dir))) path = Path.Combine(path, dir);
            else Console.WriteLine("such directory doesnt exist");

            return path;
        }
    }
}
