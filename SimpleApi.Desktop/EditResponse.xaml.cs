using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace SimpleApi.Desktop
{
    /// <summary>
    /// Interaction logic for EditResponse.xaml
    /// </summary>
    public partial class EditResponse : Window
    {
        private Refreshable _parentWindow = null;
        public EditResponse(Refreshable window)
        {
            InitializeComponent();
            _parentWindow = window;
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            var endpoint = new SimpleApi.Server.EndpointConfiguration
            {
                Url = this.Url.Text
            };

            if (Common.WriteNewFile("new",".json",endpoint))
            {
                _parentWindow.Refresh();
                this.Close();
            }
            else
            {
                //show error
                
            }
        }
    }
}
