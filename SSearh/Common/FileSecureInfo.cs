using SSearh.Interfaces;
using SSearh.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSearh.Common
{
    public class FileSecureInfo : ISecureInfo
    {
        public StartType Type { get; set; }

        public string Name { get; set; }
        public FileInfo PathF { get; set; }
        public string CommandToRun { get; set; }

        public bool IsSignatureContains { get; set; }
        public bool IsCorrect { get; set; }
        public string SignCompany { get; set; }

    }
}
