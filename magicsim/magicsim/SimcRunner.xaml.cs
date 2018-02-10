using System;
using System.Linq;
using System.IO;
using System.Windows;

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
            context.RunningFailed += Context_RunningFailed;
        }
        
        private void Context_RunningComplete(object sender, EventArgs e)
        {
            var simcData = (SimcRunnerData)sender;
            var window = new ResultsWindow();
            window.Top = App.Current.MainWindow.Top;
            window.Left = App.Current.MainWindow.Left;
            ((ResultsData)window.DataContext).LoadResultPath("results" + Path.DirectorySeparatorChar + simcData.guid);
            ((ResultsData)window.DataContext).MergeResults(simcData.model, simcData.guid);
            App.Current.MainWindow = window;
            this.Close();
            window.Show();
        }

        private void Context_RunningFailed(object sender, EventArgs e)
        {
            var window = new SimQueue();
            window.Top = App.Current.MainWindow.Top;
            window.Left = App.Current.MainWindow.Left;
            App.Current.MainWindow = window;
            this.Close();
            window.Show();
        }
    }
}
