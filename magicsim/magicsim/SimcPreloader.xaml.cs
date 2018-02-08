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
