using SSearh.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSearh.Common
{
    public class SServicesInfo : IFileExInfo
    {
        private string _name;
        private FileInfo _pathF;
        private string _commandToRun;

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public FileInfo PathF
        {
            get => _pathF;
            set => _pathF = value;
        }

        public string CommandToRun
        {
            get => _commandToRun;
            set => _commandToRun = value;
        }
    }
}
