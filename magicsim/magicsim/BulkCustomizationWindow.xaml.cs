using System.Linq;
using System.Windows;

namespace magicsim
{
    /// <summary>
    /// Interaction logic for CustomizationWindow.xaml
    /// </summary>
    public partial class BulkCustomizationWindow : Window
    {
        public BulkCustomizationWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var context = (BulkCustomizationData)this.DataContext;
            var window = new SimQueue();
            window.Top = App.Current.MainWindow.Top;
            window.Left = App.Current.MainWindow.Left;
            context.CustomizationDataList.ToList().ForEach((customizationData) =>
            {
                var profile = customizationData.ConstructFinalProfile();
                ((SimQueueData)window.DataContext).AddSim(profile, customizationData.Name);
            });
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
