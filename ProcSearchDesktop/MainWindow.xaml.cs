using SSearh;
using SSearh.Common;
using SSearh.Helpers;
using SSearh.Interfaces;
using SSearh.Search;
using SSearh.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProcSearchDesktop
{

    public class SomeInfo : ISecureInfo
    {
        public ImageSource IconImage { get; set; }
        public string Name { get; set; }
        public FileInfo PathF { get; set; }
        public StartType Type { get ; set ; }
        public bool IsSignatureContains { get ;set; }
        public bool IsCorrect { get; set; }
        public string SignCompany { get; set; }
        public string CommandToRun { get; set; }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window 
    {

        private ObservableCollection<SomeInfo> _fileSList = new ObservableCollection<SomeInfo>();

        public ObservableCollection<SomeInfo> FileSList
        {
            get { return _fileSList; }
        }

        public MainWindow()
        {
            

        }

        private void ObservableCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            int i = 1;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this;
            InitializeComponent();

            SRegistry sRegistry = new SRegistry();
            SStartM sStart = new SStartM();
            SServices sServices = new SServices();

            ISearchFactory searchFactory = sRegistry;
            ISearchFactory searchFactorySt = sStart;
            ISearchFactory searchFactoryServ = sServices;

            List<ISearchFactory> ll = new List<ISearchFactory> { searchFactory, searchFactorySt, searchFactoryServ };
            SearchFactoryContol searchFactoryContol = new SearchFactoryContol(ll);
            searchFactoryContol.observableCollection.CollectionChanged += ObservableCollection_CollectionChanged;

            searchFactoryContol.SecureEventHandler += SearchFactoryContol_SecureEventHandler; ;
            searchFactoryContol.StartSearch();

        }

        


        private void SearchFactoryContol_SecureEventHandler(object sender, ISecureInfo message)
        {
            if (message == null)
            {
                MessageBox.Show("Done");
                return;
            }

            SomeInfo someInfo = new SomeInfo();

            if (message.PathF!=null)
            {
                Icon ico = Extra.IconFromFilePath(message.PathF.FullName);
                if (ico != null)
                    //

                    App.Current.Dispatcher.Invoke((System.Action)delegate
                    {
                        someInfo.IconImage = ExtraImage.ToImageSource(ico);
                    });

                
            }
            

            someInfo.Name = message.Name;
            someInfo.PathF = message.PathF;
            someInfo.Type = message.Type;
            someInfo.IsSignatureContains = message.IsSignatureContains;
            someInfo.IsCorrect = message.IsCorrect;
            someInfo.SignCompany = message.SignCompany;
            someInfo.CommandToRun = message.CommandToRun;

            App.Current.Dispatcher.Invoke((System.Action)delegate
            {
                _fileSList.Add(someInfo);
            });

            
        }

        private object GetListViewItemObject(ListView LV, object originalSource)
        {
            DependencyObject dep = (DependencyObject)originalSource;
            while ((dep != null) && !(dep.GetType() == typeof(ListViewItem)))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }
            if (dep == null)
                return null;
            object obj = (Object)LV.ItemContainerGenerator.ItemFromContainer(dep);
            return obj;
        }

        private void lvwStartProc_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            if (item != null)
            {
                SomeInfo someInfo = item as SomeInfo;

                if (Directory.Exists(someInfo.PathF.DirectoryName))
                {
                    Process.Start("explorer.exe", someInfo.PathF.DirectoryName);
                }
                else
                {
                    MessageBox.Show("File path no detected");
                }

                //use the item here and pass to the new window
                // s = new NewModal(Email)item);
            }
        }
    }



}
