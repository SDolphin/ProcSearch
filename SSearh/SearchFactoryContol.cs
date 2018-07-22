using SSearh.Common;
using SSearh.Helpers;
using SSearh.Interfaces;
using SSearh.Search;
using SSearh.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SSearh
{

    /// <summary>
    /// init task in constructor an then run searh. finish when u get null
    /// </summary>
    public class SearchFactoryContol
    {

        public ObservableCollection<ISecureInfo> observableCollection;
        private List<ISearchFactory> searchFactoriesList;

        public SearchFactoryContol(List<ISearchFactory> searchFactoriesList)
        {
            observableCollection = new ObservableCollection<ISecureInfo>();
            this.searchFactoriesList = searchFactoriesList;

        }

        public event SecureEventDelegate SecureEventHandler;

        public void StartSearch()
        {
            List<Task> taskList = new List<Task>();
            foreach (ISearchFactory aasearchFactory in searchFactoriesList)
            {
                aasearchFactory.SearchMessageEvent += SearchFactory_SearchMessageEvent1;
                Task task = Task.Factory.StartNew(() => { aasearchFactory.Search(); });
                taskList.Add(task);
            }

             Task.WhenAll(taskList.ToArray()).ContinueWith(_ => SearchOnHandler(null)); //all tasks done
      
        }


        private void SearchFactory_SearchMessageEvent1(object sender, IFileExInfo message)
        {
            var type = GetStartType(sender);

            FileSecureInfo fileSecureInfo = new FileSecureInfo();

            fileSecureInfo.Type = type;

            fileSecureInfo.Name = message.Name;
            fileSecureInfo.PathF = message.PathF;
            fileSecureInfo.CommandToRun = message.CommandToRun;

            if (fileSecureInfo.PathF != null)
            {
                Signature sign = FileSecure.AuthenticodeSignature(message.PathF.FullName);
                if (sign != null && (sign.Status == SignatureStatus.Valid ||
                    sign.Status == SignatureStatus.NotTrusted))
                {
                    fileSecureInfo.IsSignatureContains = true;
                }
                else
                {
                    fileSecureInfo.IsSignatureContains = false;
                }

                if (sign != null && sign.Status == SignatureStatus.Valid)
                {
                    fileSecureInfo.IsCorrect = true;
                }
                else
                {
                    fileSecureInfo.IsCorrect = false;
                }

                if (fileSecureInfo.IsSignatureContains == true)
                {
                    fileSecureInfo.SignCompany = FileSecure.GetSignatureOrganization(sign);
                }
                else
                {
                    fileSecureInfo.SignCompany = "NULL";
                }
            }
            else
            {
                fileSecureInfo.IsSignatureContains = false;
                fileSecureInfo.IsCorrect = false;
                fileSecureInfo.SignCompany = "NULL";
            }
            observableCollection.Add(fileSecureInfo);
            SearchOnHandler(fileSecureInfo);
        }

        protected virtual void SearchOnHandler(ISecureInfo message)
        {
            if (SecureEventHandler != null)
            {
                SecureEventHandler(this, message);
            }
        }

        private static StartType GetStartType(object sender)
        {
            if (sender is SRegistry)
            {
                return StartType.Registry;
            }
            else if (sender is SStartM)
            {
                return StartType.StartMenu;
            }
            else
            {
                return StartType.Sheduler;
            }
        }

    }
}
