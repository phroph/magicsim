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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var window = new ArmorySim();
            App.Current.MainWindow = window;
            this.Close();
            window.Show();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var window = new SimCSim();
            App.Current.MainWindow = window;
            this.Close();
            window.Show();
        }
    }
}
