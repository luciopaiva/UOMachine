using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using Microsoft.Win32;

namespace UOMachine
{
    /// <summary>
    /// Interaction logic for AssemblyPicker.xaml
    /// </summary>
    internal partial class AssemblyPicker : Window
    {
        public class AssemblyData
        {
            public string assembly { get; set; }
            public string version { get; set; }
            public string path { get; set; }

            public AssemblyData() { }

            public AssemblyData(string assembly, string version, string path)
            {
                this.assembly = assembly;
                this.version = version;
                this.path = path;
            }
        }

        private ObservableCollection<AssemblyData> myAssemblyDataCollection = new ObservableCollection<AssemblyData>();
        public ObservableCollection<AssemblyData> assemblyDataCollection { get { return myAssemblyDataCollection; } }
        public delegate void dAssemblySelected(string fileName);
        public event dAssemblySelected AssemblySelectedEvent;
        private RoutedEventHandler rLoaded;

        private bool myCancelEnabled = true;
        public bool CancelEnabled
        {
            get { return Utility.ThreadHelper.VolatileRead<bool>(ref myCancelEnabled); }
            internal set { myCancelEnabled = value; }
        }

        private void OnAssemblySelected(string fileName)
        {
            dAssemblySelected handler = AssemblySelectedEvent;
            if (handler != null) handler(fileName);
        }

        public AssemblyPicker()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            foreach (object o in listView1.SelectedItems)
            {
                AssemblyData data = (AssemblyData)o;
                OnAssemblySelected(data.assembly);
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.CheckFileExists = true;
            OFD.CheckPathExists = true;
            OFD.Multiselect = true;
            OFD.ValidateNames = true;
            OFD.Filter = "(*.dll, *.exe)|*.dll;*.exe|(*.*)|*.*";
            OFD.FileOk += new System.ComponentModel.CancelEventHandler(OFD_FileOk);
            OFD.ShowDialog();
        }

        private void OFD_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            OpenFileDialog OFD = (OpenFileDialog)sender;
            if (OFD.SafeFileNames != null)
            {
                foreach (string fileName in OFD.SafeFileNames)
                {
                    OnAssemblySelected(fileName);
                }
            }
        }

        private void AssemblyPickerWindow_Loaded(object sender, RoutedEventArgs e)
        {
            AssemblyHelper.GetAllAssemblies(myAssemblyDataCollection);
        }

        private void listView1_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            foreach (object o in listView1.SelectedItems)
            {
                AssemblyData data = (AssemblyData)o;
                OnAssemblySelected(data.assembly);
            }
            this.Hide();
            listView1.UnselectAll();
        }

        private void AssemblyPickerWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.CancelEnabled)
            {
                e.Cancel = true;
                this.Hide();
            }
        }
    }
}
