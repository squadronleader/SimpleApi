using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimpleApiDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, Refreshable
    {
        public MainWindow()
        {
            InitializeComponent();
            var server = SimpleApiServer.Server.Start();

           

            Refresh();
        }

        private IEnumerable<FileInfo> GetContent()
        {
            foreach (string file in Directory.EnumerateFiles("Content", "*.json"))
            {
                yield return new FileInfo(file);
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var createWindow = new EditResponse(this);
            createWindow.ShowDialog();
        }

        public void Refresh()
        {
            listView.ItemsSource = GetContent().Select(i => i.Name);
            listView.Items.Refresh();
        }
    }
}
