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

namespace RCP_Sys.Views
{
    /// <summary>
    /// Logika interakcji dla klasy EditProjectDialog.xaml
    /// </summary>
    public partial class EditProjectDialog : Window
    {
        public EditProjectDialog()
        {
            InitializeComponent();
        }

        private void closeButton(object sender, RoutedEventArgs e)
        {
            Close();    
        }
    }
}
