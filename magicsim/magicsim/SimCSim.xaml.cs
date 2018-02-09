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

namespace magicsim
{
    /// <summary>
    /// Interaction logic for SimCSim.xaml
    /// </summary>
    public partial class SimCSim : Window
    {
        public SimCSim()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (SubMainWindow.isActive)
            {
                var window = new SubMainWindow();
                window.Top = App.Current.MainWindow.Top;
                window.Left = App.Current.MainWindow.Left;
                App.Current.MainWindow = window;
                this.Close();
                window.Show();
            }
            else
            {
                var window = new MainWindow();
                window.Top = App.Current.MainWindow.Top;
                window.Left = App.Current.MainWindow.Left;
                App.Current.MainWindow = window;
                this.Close();
                window.Show();
            }
        }

        private void continueButton_Click(object sender, RoutedEventArgs e)
        {
            var simc = ((TextBox)this.FindName("SimC")).Text;
            if (simc == null || simc.Length == 0)
            {
                return;
            }
            var window = new SimcPreloader();
            window.Top = App.Current.MainWindow.Top;
            window.Left = App.Current.MainWindow.Left;
            ((SimcPreloaderData)window.DataContext).LoadArmoryData(simc);
            App.Current.MainWindow = window;
            this.Close();
            window.Show();
        }
    }
}
