using magicsim.objectModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace magicsim
{
    /// <summary>
    /// Interaction logic for CustomizationWindow.xaml
    /// </summary>
    public partial class CustomizationWindow : Window
    {
        private static Dictionary<string, Dictionary<string, List<Azerite>>> azeriteMapping = null;

        public CustomizationWindow()
        {
            InitializeComponent();

            var context = (CustomizationData) this.DataContext;

            // Azerite is fairly big in comparison so we cache it.
            if (azeriteMapping == null)
            {
                using (StreamReader r = new StreamReader(Path.Combine("json", "Azerite.json")))
                {
                    string json = r.ReadToEnd();
                    azeriteMapping = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, List<Azerite>>>>(json);
                }
            }
            context.AzeriteMapping = azeriteMapping;
            using (StreamReader r = new StreamReader(Path.Combine("json", "Food.json")))
            {
                string json = r.ReadToEnd();
                context.FoodNameMapping = JsonConvert.DeserializeObject<List<Consumable>>(json);
                context.FoodNameMapping.ForEach(x => context.Foods.Add(x.name));
                context.Food = context.Foods[0];
            }
            using (StreamReader r = new StreamReader(Path.Combine("json", "Flasks.json")))
            {
                string json = r.ReadToEnd();
                context.FlaskNameMapping = JsonConvert.DeserializeObject<List<Consumable>>(json);
                context.FlaskNameMapping.ForEach(x => context.Flasks.Add(x.name));
                context.Flask = context.Flasks[0];
            }
            using (StreamReader r = new StreamReader(Path.Combine("json", "Augmentations.json")))
            {
                string json = r.ReadToEnd();
                context.AugmentNameMapping = JsonConvert.DeserializeObject<List<Consumable>>(json);
                context.AugmentNameMapping.ForEach(x => context.Runes.Add(x.name));
                context.Rune = context.Runes[0];
            }
            using (StreamReader r = new StreamReader(Path.Combine("json", "Potions.json")))
            {
                string json = r.ReadToEnd();
                context.PotionNameMapping = JsonConvert.DeserializeObject<List<Consumable>>(json);
                context.PotionNameMapping.ForEach(x => context.Potions.Add(x.name));
                context.Potion = context.Potions[0];
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var context = (CustomizationData)this.DataContext;
            var profile = context.ConstructFinalProfile();
            var window = new SimQueue();
            window.Top = App.Current.MainWindow.Top;
            window.Left = App.Current.MainWindow.Left;
            ((SimQueueData) window.DataContext).AddSim(profile,context.Name);
            App.Current.MainWindow = window;
            this.Close();
            window.Show();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
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
    }
}
