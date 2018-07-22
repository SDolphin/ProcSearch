using SSearh.Common;
using SSearh.Helpers;
using SSearh.Interfaces;
using SSearh.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace SSearh.Search
{

    /// <summary>
    /// implements a search autorun files in the services
    /// </summary>
    public class SServices : ISearchFactory
    {
        //public delegate void SearchEventDelegate(object sender, IFileExInfo message);
        public event SearchEventDelegate someSearchMessageEvent;

        ServiceController[] scServices;

        public SServices()
        {
            scServices = ServiceController.GetServices();
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
            foreach (var service in scServices)
            {
                if (service.StartType == ServiceStartMode.Automatic)
                {
                    string commandPath = GetServiceExePath(service.ServiceName);
                    SServicesInfo sServicesInfo;
                    if (commandPath == null)
                    {
                        sServicesInfo = new SServicesInfo()
                        {
                            Name = service.ServiceName,
                            CommandToRun = null,
                            PathF = null,
                        };
                    }
                    else
                    {

                        string pathAndArgs = commandPath;
                        FileInfo path;

                        if (StringHelper.IsCommandLine(commandPath))
                        {
                            path = StringHelper.GetPathFromCommand(commandPath);
                        }
                        else
                        {
                            path = StringHelper.GetPathFromCommand(commandPath);
                        }

                        sServicesInfo = new SServicesInfo()
                        {
                            Name = service.ServiceName,
                            CommandToRun = pathAndArgs,
                            PathF = path,
                        };
                    }
                    SearchOnHandler(sServicesInfo);
                }
            }
        }




        private string GetServiceExePath(string serviceName)
        {

            string currentserviceExePath = string.Empty;
            using (ManagementObject wmiService = new ManagementObject("Win32_Service.Name='" + serviceName + "'"))
            {
                try
                {
                    wmiService.Get();
                    currentserviceExePath = wmiService["PathName"].ToString();
                }
                catch (Exception ex)
                {
                    currentserviceExePath = null;
                }
                return currentserviceExePath;
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
