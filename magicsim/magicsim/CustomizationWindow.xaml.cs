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
    /// Interaction logic for CustomizationWindow.xaml
    /// </summary>
    public partial class CustomizationWindow : Window
    {
        public CustomizationWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var context = (CustomizationData)this.DataContext;
            var profile = context.ConstructFinalProfile();
            var window = new SimQueue();
            ((SimQueueData)window.DataContext).AddSim(profile,context.Name);
            App.Current.MainWindow = window;
            this.Close();
            window.Show();
        }

        private void Tier_Change(object sender, RoutedEventArgs e)
        {
            var context = (CustomizationData)this.DataContext;
            var t212pc = (CheckBox)this.FindName("T212pc");
            var t214pc = (CheckBox)this.FindName("T214pc");
            var t202pc = (CheckBox)this.FindName("T202pc");
            var t204pc = (CheckBox)this.FindName("T204pc");
            var t192pc = (CheckBox)this.FindName("T192pc");
            var t194pc = (CheckBox)this.FindName("T194pc");
            t212pc.IsEnabled = context.ModifyTier;
            t214pc.IsEnabled = context.ModifyTier;
            t202pc.IsEnabled = context.ModifyTier;
            t204pc.IsEnabled = context.ModifyTier;
            t192pc.IsEnabled = context.ModifyTier;
            t194pc.IsEnabled = context.ModifyTier;
            if (t212pc.IsChecked.Value && t202pc.IsChecked.Value && t192pc.IsChecked.Value)
            {
                t214pc.IsEnabled = false;
                t204pc.IsEnabled = false;
                t194pc.IsEnabled = false;
            }
            if(t214pc.IsChecked.Value || t204pc.IsChecked.Value || t194pc.IsChecked.Value)
            {
                if (!t214pc.IsChecked.Value)
                {
                    t214pc.IsEnabled = false;
                }
                else
                {
                    context.T212pc = true;
                    t212pc.IsEnabled = false;
                }
                if (!t204pc.IsChecked.Value)
                {
                    t204pc.IsEnabled = false;
                }
                else
                {
                    context.T202pc = true;
                    t202pc.IsEnabled = false;
                }
                if (!t194pc.IsChecked.Value)
                {
                    t194pc.IsEnabled = false;
                }
                else
                {
                    context.T192pc = true;
                    t192pc.IsEnabled = false;
                }
                int twoPcCount = 0;
                if(t212pc.IsChecked.Value)
                {
                    twoPcCount++;
                }
                if(t202pc.IsChecked.Value)
                {
                    twoPcCount++;
                }
                if (t192pc.IsChecked.Value)
                {
                    twoPcCount++;
                }
                if(twoPcCount==2)
                {
                    if(!t212pc.IsChecked.Value)
                    {
                        t212pc.IsEnabled = false;
                    }
                    if (!t202pc.IsChecked.Value)
                    {
                        t202pc.IsEnabled = false;
                    }
                    if (!t192pc.IsChecked.Value)
                    {
                        t192pc.IsEnabled = false;
                    }
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (SubMainWindow.isActive)
            {
                var window = new SimQueue();
                App.Current.MainWindow = window;
                this.Close();
                window.Show();
            }
            else
            {
                var window = new MainWindow();
                App.Current.MainWindow = window;
                this.Close();
                window.Show();
            }
        }
    }
}
