using magicsim.common;
using magicsim.objectModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace magicsim
{
    /// <summary>
    /// Interaction logic for BulkSimLoader.xaml
    /// </summary>
    public partial class QuickSimLoader : Window
    {
        public List<Model> models;
        public QuickSimLoader()
        {
            InitializeComponent();
            using (StreamReader r = new StreamReader(Path.Combine("json", "Models.json")))
            {
                string json = r.ReadToEnd();
                var context = (QuickSimCustomizationData) this.DataContext;

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

        private void regions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var armorySimData = (QuickSimCustomizationData)this.DataContext;
            var regionsDropdown = (ComboBox)sender;
            var value = (String)regionsDropdown.SelectedValue;
            //BeginGetServersForRegion(armorySimData, value);
        }

        private void BeginGetServersForRegion(QuickSimCustomizationData model, String region)
        {
            ThreadPool.QueueUserWorkItem((_) =>
            {
                string JSON = new StreamReader(WebRequest.CreateHttp("https://" + region + ".api.battle.net/wow/realm/status?locale=en_US&apikey=y592n6pwqj7rypxbswa2x4ek5umg4462").GetResponse().GetResponseStream()).ReadToEnd();
                JObject jsonObject = (JObject) JsonConvert.DeserializeObject(JSON);
                List<string> servers = new List<string>();
                jsonObject.GetValue("realms").ToList().ForEach((realm) =>
                {
                    servers.Add(realm.Value<string>("name"));
                });
                App.Current.Dispatcher.Invoke(() => {
                    model.PopulateServers(servers);
                });
            });
        }

        private void continueButton_Click(object sender, RoutedEventArgs e)
        {
            var guid = Guid.NewGuid().ToString().Substring(0, 8);
            var window = new SimcRunner();
            var context = (QuickSimCustomizationData) this.DataContext;
            window.Top = App.Current.MainWindow.Top;
            window.Left = App.Current.MainWindow.Left;
            App.Current.MainWindow = window;
            var nameCache = new HashSet<string>();
            var simList = new List<string>();
            var threads = Math.Min(8, Environment.ProcessorCount);
            var profile = "";

            Model selectedModel;
            var potentialMatch = models.Where((model) =>
            {
                return model.dispName.Equals(context.SelectedModel);
            });
            if (potentialMatch.Count() > 0)
            {
                selectedModel = potentialMatch.ElementAt(0);
            }
            else
            {
                return;
            }


            if (CharacterSourceControl.SelectedItem as TabItem == SimC)
            {
                profile = SimCInput.Text;
            }
            else
            {
                Properties.Settings.Default.characterName = context.Name;
                Properties.Settings.Default.realmName = context.SelectedServer;
                Properties.Settings.Default.regionName = context.SelectedRegion;
                profile = String.Format("armory=" + context.SelectedRegion + "," + context.SelectedServer + "," + context.Name);
            }

            foreach (var sim in selectedModel.model.Keys)
            {
                if (selectedModel.timeModel.Count != 0)
                {
                    foreach (var time in selectedModel.timeModel.Keys)
                    {

                        simList.Add(SimHelper.CreateSimForModel(sim, time, false, true, new List<Sim>() { new Sim() { Name = guid, Profile = profile } }, !context.DisableStatWeights, 
                            false, false, false, false, false, 0, 0, threads, guid, context.DisableBuffs, false));
                    }
                }
                else
                {
                    simList.Add(SimHelper.CreateSimForModel(sim, null, false, true, new List<Sim>() { new Sim() { Name = guid, Profile = profile } }, !context.DisableStatWeights, 
                        false, false, false, false, false, 0, 0, threads, guid, context.DisableBuffs, false));
                }
            }
            
            LoadMagicsimLoadout((SimcRunnerData)window.DataContext, simList, 1, selectedModel);
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
            var window = new MainWindow();
            window.Top = App.Current.MainWindow.Top;
            window.Left = App.Current.MainWindow.Left;
            App.Current.MainWindow = window;
            window.Show();
        }
    }
}
