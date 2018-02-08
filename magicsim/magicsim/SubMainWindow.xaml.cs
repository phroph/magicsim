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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace magicsim
{
    /// <summary>
    /// Interaction logic for SubMainWindow.xaml
    /// </summary>
    public partial class SubMainWindow : Window
    {
        public static Boolean isActive = false;

        public SubMainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var window = new ArmorySim();
            isActive = true;
            App.Current.MainWindow = window;
            this.Close();
            window.Show();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var window = new SimCSim();
            App.Current.MainWindow = window;
            isActive = true;
            this.Close();
            window.Show();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            var window = new SimQueue();
            App.Current.MainWindow = window;
            this.Close();
            window.Show();
        }
    }
}
