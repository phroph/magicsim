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
    /// Interaction logic for SimcPreloader.xaml
    /// </summary>
    public partial class SimcPreloader : Window
    {
        public SimcPreloader()
        {
            InitializeComponent();
            var context = (SimcPreloaderData)this.DataContext;
            context.PreloadingComplete += Context_PreloadingComplete;
            context.PreloadingFailed += Context_PreloadingFailed;
        }

        private void Context_PreloadingFailed(object sender, EventArgs e)
        {
            if (SubMainWindow.isActive)
            {
                var window = new SimQueue();
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

        private void Context_PreloadingComplete(object sender, EventArgs e)
        {
            var simcData = (SimcPreloaderData)sender;
            var window = new CustomizationWindow();
            ((CustomizationData)window.DataContext).LoadProfilePath("characters/" + simcData.charName + ".simc");
            App.Current.MainWindow = window;
            this.Close();
            window.Show();
        }
    }
}
