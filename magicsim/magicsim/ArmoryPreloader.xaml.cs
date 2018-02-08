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
    /// Interaction logic for ArmoryPreloader.xaml
    /// </summary>
    public partial class ArmoryPreloader : Window
    {
        public ArmoryPreloader()
        {
            InitializeComponent();
            var context = (ArmoryPreloaderData)this.DataContext;
            context.PreloadingComplete += Context_PreloadingComplete;
        }


        private void Context_PreloadingComplete(object sender, EventArgs e)
        {
            var armoryData = (ArmoryPreloaderData)sender;
            var window = new CustomizationWindow();
            ((CustomizationData)window.DataContext).LoadProfilePath("characters/" + armoryData.charName + ".simc");
            App.Current.MainWindow = window;
            this.Close();
            window.Show();
        }
    }
}
