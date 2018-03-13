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
    /// Interaction logic for ResultLoader.xaml
    /// </summary>
    public partial class ResultLoader : Window
    {
        public ResultLoader()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var context = (ResultLoaderData) this.DataContext;

            // Looks quirky, but the internal setting on Label can fail due to name conflicts and bad input
            // This reverts to known working and also satisfies the implicit expectation from the next line that Label==Text at the end of this block.
            // If not quite the equality that was expected at first glance.
            if (context.SelectedTag == null)
            {
                MessageBox.Show("Please select a sim to rename.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            context.SelectedTag.Label = RenameBox.Text;
            if(context.SelectedTag.Label != RenameBox.Text)
            {
                RenameBox.Text = context.SelectedTag.Label;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var window = new ResultsWindow();
            var context = (ResultLoaderData) this.DataContext;
            var windowContext = (ResultsData) window.DataContext;
            if (!windowContext.LoadResults(context.SelectedTag.Label))
            {
                this.Close();
                return;
            }
            window.Top = App.Current.MainWindow.Top;
            window.Left = App.Current.MainWindow.Left;
            App.Current.MainWindow = window;
            this.Close();
            window.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();
            window.Top = App.Current.MainWindow.Top;
            window.Left = App.Current.MainWindow.Left;
            App.Current.MainWindow = window;
            this.Close();
            window.Show();
        }
    }
}
