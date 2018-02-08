using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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
    /// Interaction logic for ArmorySim.xaml
    /// </summary>
    public partial class ArmorySim : Window
    {
        public ArmorySim()
        {
            InitializeComponent();
        }

        private void servers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var armorySimData = (ArmorySimData)this.DataContext;
            armorySimData.SelectedServer = (String)((ComboBox)sender).SelectedValue;
        }

        private void regions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var armorySimData = (ArmorySimData)this.DataContext;
            var regionsDropdown = (ComboBox)sender;
            var value = (String)regionsDropdown.SelectedValue;
            armorySimData.SelectedRegion = value;
            BeginGetServersForRegion(armorySimData, value);
        }

        private void BeginGetServersForRegion(ArmorySimData model, String region)
        {
            ThreadPool.QueueUserWorkItem((_) =>
            {
                string JSON = new StreamReader(WebRequest.CreateHttp("https://" + region + ".api.battle.net/wow/realm/status?locale=en_US&apikey=y592n6pwqj7rypxbswa2x4ek5umg4462").GetResponse().GetResponseStream()).ReadToEnd();
                JObject jsonObject = (JObject)JsonConvert.DeserializeObject(JSON);
                List<string> servers = new List<string>();
                jsonObject.GetValue("realms").ToList().ForEach((realm) =>
                {
                    servers.Add(((JToken)realm).Value<string>("name"));
                });
                App.Current.Dispatcher.Invoke(() => {
                    model.PopulateServers(servers);
                });
            });
            
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var armorySimData = (ArmorySimData)this.DataContext;
            if (armorySimData.Name == null || armorySimData.Name.Length == 0)
            {
                return;
            }
            var window = new ArmoryPreloader();
            ((ArmoryPreloaderData)window.DataContext).LoadArmoryData(armorySimData.SelectedRegion, armorySimData.SelectedServer, armorySimData.Name);
            App.Current.MainWindow = window;
            this.Close();
            window.Show();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (SubMainWindow.isActive)
            {
                var window = new SubMainWindow();
                App.Current.MainWindow = window;
                this.Close();
                window.Show();
            } else
            {
                var window = new MainWindow();
                App.Current.MainWindow = window;
                this.Close();
                window.Show();
            }
        }
    }
}
