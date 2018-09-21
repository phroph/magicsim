using magicsim.common;
using magicsim.objectModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Path = System.IO.Path;

namespace magicsim
{
    /// <summary>
    /// Interaction logic for SimQueue.xaml
    /// </summary>
    public partial class SimQueue : Window
    {
        public List<Model> models;
        public SimQueue()
        {
            SubMainWindow.isActive = false;
            InitializeComponent();
            using (StreamReader r = new StreamReader(Path.Combine("json", "Models.json")))
            {
                string json = r.ReadToEnd();
                var context = (SimQueueData)this.DataContext;
                
                List<Model> items = JsonConvert.DeserializeObject<List<Model>>(json);
                models = items;
                context.Models.Clear();
                items.ForEach((item) =>
                {
                    context.Models.Add(item.dispName);
                });
                context.SelectedModel = context.Models[0];
            }
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var context = (SimQueueData)this.DataContext;
            context.RemoveSim(((Sim)(((Button)sender)).DataContext).Name);
        }

        private void AddCharacter_Click(object sender, RoutedEventArgs e)
        {
            var window = new SubMainWindow();
            window.Top = App.Current.MainWindow.Top;
            window.Left = App.Current.MainWindow.Left;
            App.Current.MainWindow = window;
            this.Close();
            window.Show();
        }

        private void Simulate_Click(object sender, RoutedEventArgs e)
        { 
            var context = (SimQueueData)this.DataContext;
            Model selectedModel;
            var potentialMatch = models.Where((model) =>
            {
                return model.dispName.Equals(context.SelectedModel);
            });
            if(potentialMatch.Count()>0)
            {
                selectedModel = potentialMatch.ElementAt(0);
            } else
            {
                return;
            }
            var guid = Guid.NewGuid().ToString().Substring(0, 8);
            var simList = new List<string>();
            foreach (var sim in selectedModel.model.Keys)
            {
                if (selectedModel.timeModel.Count != 0)
                {
                    foreach (var time in selectedModel.timeModel.Keys)
                    {
                        simList.Add(SimHelper.CreateSimForModel(sim, time, context.PTRMode, context.FixedIterationOrError != 0, context.Sims.ToList(), !context.DisableStatWeights,
                            context.ReforgeEnabled, context.ReforgeHaste, context.ReforgeCrit, context.ReforgeVers, context.ReforgeMastery, context.ReforgeAmount,
                            context.ReforgeStepSize, context.Threads, guid, false, true));
                    }
                }
                else
                {
                    simList.Add(SimHelper.CreateSimForModel(sim, null, context.PTRMode, context.FixedIterationOrError != 0, context.Sims.ToList(), !context.DisableStatWeights,
                        context.ReforgeEnabled, context.ReforgeHaste, context.ReforgeCrit, context.ReforgeVers, context.ReforgeMastery, context.ReforgeAmount,
                        context.ReforgeStepSize, context.Threads, guid, false, true));
                }
            }
            var window = new SimcRunner();
            window.Top = App.Current.MainWindow.Top;
            window.Left = App.Current.MainWindow.Left;
            App.Current.MainWindow = window;
            LoadMagicsimLoadout((SimcRunnerData)window.DataContext, simList, context.Processes, selectedModel);
            this.Close();
            window.Show();
        }

        private void LoadMagicsimLoadout(SimcRunnerData runner, List<string> profileset, int processCount, Model model)
        {
            var filenameRegex = new Regex(String.Format("json2=results\\{0}([^\\{0}]+\\{0}[^\\{0}]+).json", System.IO.Path.DirectorySeparatorChar));
            runner.model = model;
            runner.ExecuteSimRun(profileset, filenameRegex, processCount, Context_RunningFailed, Context_RunningComplete, "sims", "results", true);
        }

        private void Context_RunningComplete(object sender, EventArgs e)
        {
            var simcData = (SimcRunnerData)sender;
            var window = new ResultsWindow();
            window.Top = App.Current.MainWindow.Top;
            window.Left = App.Current.MainWindow.Left;
            ((ResultsData)window.DataContext).LoadResultPath("results" + System.IO.Path.DirectorySeparatorChar + simcData.guid);

            var htmls = Directory.EnumerateFiles("results" + System.IO.Path.DirectorySeparatorChar + simcData.guid, "*.html");
            ((ResultsData)window.DataContext).MergeResults(simcData.model, simcData.guid, htmls);
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

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult value = MessageBox.Show("This will wipe all sim setup done so far. Are you sure you want to continue?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (value.Equals(MessageBoxResult.Yes))
            {
                SimDataManager.ResetSimData();
            }
            else
            {
                return;
            }

            var window = new MainWindow();
            window.Top = App.Current.MainWindow.Top;
            window.Left = App.Current.MainWindow.Left;
            App.Current.MainWindow = window;
            this.Close();
            window.Show();
        }
    }
}
