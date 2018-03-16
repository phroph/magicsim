using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for ResultsWindow.xaml
    /// </summary>
    public partial class ResultsWindow : Window
    {
        public ResultsWindow()
        {
            InitializeComponent();
            var context = (ResultsData)this.DataContext;
            context.RunningFailed += Context_RunningFailed;
            PawnBox.PreviewMouseLeftButtonDown += SelectivelyIgnoreMouseButton;
            PawnBox.GotKeyboardFocus += SelectAllText;
            PawnBox.MouseDoubleClick += SelectAllText;
        }

        private void Context_RunningFailed(object sender, EventArgs e)
        {
            var window = new MainWindow();
            window.Top = App.Current.MainWindow.Top;
            window.Left = App.Current.MainWindow.Left;
            App.Current.MainWindow = window;
            this.Close();
            window.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();
            window.Top = App.Current.MainWindow.Top;
            window.Left = App.Current.MainWindow.Left;
            App.Current.MainWindow = window;
            this.Close();
            window.Show();
        }

        void SelectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
        {
            // Find the TextBox
            DependencyObject parent = e.OriginalSource as UIElement;
            while (parent != null && !(parent is TextBox))
                parent = VisualTreeHelper.GetParent(parent);

            if (parent != null)
            {
                var textBox = (TextBox)parent;
                if (!textBox.IsKeyboardFocusWithin)
                {
                    // If the text box is not yet focused, give it the focus and
                    // stop further processing of this click event.
                    textBox.Focus();
                    e.Handled = true;
                }
            }
        }

        void SelectAllText(object sender, RoutedEventArgs e)
        {
            var textBox = e.OriginalSource as TextBox;
            if (textBox != null)
                textBox.SelectAll();
        }

        private void Rename_Click(object sender, RoutedEventArgs e)
        {
            var context = (ResultsData)this.DataContext;

            // Looks quirky, but the internal setting on Label can fail due to name conflicts and bad input
            // This reverts to known working and also satisfies the implicit expectation from the next line that Label==Text at the end of this block.
            // If not quite the equality that was expected at first glance.
            if (context.Tag == null)
            {
                MessageBox.Show("Please select a sim to rename.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            context.Tag = RenameBox.Text;
            if (context.Tag != RenameBox.Text)
            {
                RenameBox.Text = context.Tag;
            }
        }
    }
}
