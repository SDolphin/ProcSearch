using SSearh.Common;
using SSearh.Helpers;
using SSearh.Interfaces;
using SSearh.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSearh.Search
{

    /// <summary>
    /// implements a search autorun files in the start menu
    /// </summary>
    public class SStartM : ISearchFactory
    {
        private string _sAutoRun;

        public string SAutoRun
        {
            get => _sAutoRun;
        }

        //public delegate void SearchEventDelegate(object sender, IFileExInfo message);
        public event SearchEventDelegate someSearchMessageEvent;

        private readonly object SearchMessageEventLock = new object();

        event SearchEventDelegate ISearchFactory.SearchMessageEvent
        {
            add
            {
                someSearchMessageEvent += value;
            }
            remove
            {
                someSearchMessageEvent -= value;
            }

        }


        public SStartM()
        {
            _sAutoRun = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu),
                @"Programs\StartUp");

        }

        public void Search()
        {
            string[] masOfFiles = Directory.GetFiles(SAutoRun);

            foreach (string path in masOfFiles)
            {
                string realPath = string.Empty;
                if (Path.GetExtension(path).ToLower() == ".lnk")
                {
                    realPath = Extra.GetShortcutTarget(path);
                }
                else
                {
                    realPath = path;
                }

                SStartMInfo sStartMInfo = new SStartMInfo()
                {

                    Name = Path.GetFileName(path),
                    PathF = new FileInfo(realPath),
                    CommandToRun = path
                };

                SearchOnHandler(sStartMInfo);
            }

        }


        private void SearchOnHandler(IFileExInfo message)
        {
            if (someSearchMessageEvent != null)
            {
                someSearchMessageEvent(this, message);
            }
        }

    }
}
