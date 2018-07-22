using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SSearh.Helpers
{
    public static class StringHelper
    {
        public static bool IsCommandLine(string s)
        {
            if (s.Contains(" -") || s.Contains(" /") || s.Contains(" C:\\"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static FileInfo GetPathFromCommand(string command)
        {
            string pattern = @".:\\.*\.exe";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(command);

            if (match.Success)
            {
                return new FileInfo(match.Value);
            }
            else
            {
                return null;
            }

        }

    }
}
