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
    /// Interaction logic for SimcRunner.xaml
    /// </summary>
    public partial class SimcRunner : Window
    {
        public SimcRunner()
        {
            InitializeComponent();
            var context = (SimcRunnerData)this.DataContext;
            context.RunningComplete += Context_RunningComplete;
        }
        
        private void Context_RunningComplete(object sender, EventArgs e)
        {
            var simcData = (SimcRunnerData)sender;
            var window = new ResultsWindow();
            ((ResultData)window.DataContext).LoadResultPath("results/" + simcData.guid);
            ((ResultData)window.DataContext).MergeResults(simcData.model, simcData.guid);
            App.Current.MainWindow = window;
            this.Close();
            window.Show();
        }
    }
}
