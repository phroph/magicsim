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

namespace magicsim
{
    /// <summary>
    /// Interaction logic for BulkSimLoader.xaml
    /// </summary>
    public partial class BulkSimLoader : Window
    {
        public BulkSimLoader()
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
            var window = new SimcRunner();
            window.Top = App.Current.MainWindow.Top;
            window.Left = App.Current.MainWindow.Left;
            App.Current.MainWindow = window;
            var nameCache = new HashSet<string>();
            LoadCharacterSet((SimcRunnerData)window.DataContext, 
                SimC.Text.Split(new string[] { "\n\n" , "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .Where(x => x.Length > 0)
                    .Select(x => {
                        var nameClassRegex = new Regex("([^=]+)=\"?([^\r\n\"]+)\"?");
                        var name = nameClassRegex.Match(x).Groups[2].Value;
                        var tempName = name;
                        int i = 0;
                        while(nameCache.Contains(tempName))
                        {
                            tempName = name + i++;
                        }
                        name = tempName;
                        nameCache.Add(name);
                        if (!nameCache.Contains(name))
                        {
                            nameCache.Add(name);
                        }
                        var path = "./characters/"+ name + ".simc";
                        var profile = x + "\nsave=" + path;
                        if(!Directory.Exists("characters"))
                        {
                            Directory.CreateDirectory("characters");
                        }
                        File.WriteAllText(path, profile);
                        return path;
                     }).ToList(), 
                1);
            this.Close();
            window.Show();
        }

        private void LoadCharacterSet(SimcRunnerData runner, List<string> characters, int processCount)
        {
            var filenameRegex = new Regex("save=./characters/([^.]*).simc");
            runner.ExecuteSimRun(characters, filenameRegex, processCount, Context_RunningFailed, Unbound_RunningComplete(characters), "characters", "characters", false);
        }

        Func<List<string>,EventHandler> Unbound_RunningComplete = (List<string> profiles) => (object sender, EventArgs e) =>
        {
            var window = new BulkCustomizationWindow();
            window.Top = App.Current.MainWindow.Top;
            window.Left = App.Current.MainWindow.Left;
            ((BulkCustomizationData)window.DataContext).LoadProfilePaths(profiles);
            App.Current.MainWindow = window;
            window.Show();
        };

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
