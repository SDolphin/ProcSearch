using Microsoft.Win32;
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
    /// implements a search autorun files in the registry
    /// </summary>
    public class SRegistry : ISearchFactory
    {
        private const string AUTORUNKEY_1 = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        private const string AUTORUNKEY_2 = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

        public event SearchEventDelegate someSearchMessageEvent;

        public SRegistry()
        {

        }

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

        public void Search()
        {
            RegistryHive[] hives = { RegistryHive.LocalMachine/*, RegistryHive.CurrentUser*/ };
            string[] autorunKeys = { AUTORUNKEY_1, AUTORUNKEY_2 };

            foreach (var hive in hives)
            {
                using (var baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64))
                {
                    foreach (var keyName in autorunKeys)
                    {
                        using (var key = baseKey.OpenSubKey(keyName))
                        {
                            if (key != null)
                            {
                                foreach (var valueName in key.GetValueNames())
                                {

                                    string name = valueName;

                                    string command = key.GetValue(valueName).ToString();

                                    FileInfo path;
                                    if (StringHelper.IsCommandLine(command))
                                    {
                                        path = StringHelper.GetPathFromCommand(command);
                                    }
                                    else
                                    {
                                        path = new FileInfo(command);
                                    }




                                    SRegInfo sRegInfo = new SRegInfo
                                    {
                                        Name = valueName,
                                        PathF = path,
                                        CommandToRun = command
                                    };

                                    SearchOnHandler(sRegInfo);
                                }
                            }
                        }
                    }
                }
            }
        }

        protected virtual void SearchOnHandler(IFileExInfo message)
        {
            if (someSearchMessageEvent != null)
            {
                someSearchMessageEvent(this, message);
            }
        }
    }
}
