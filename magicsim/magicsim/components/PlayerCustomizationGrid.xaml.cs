using System;
using System.Windows;
using System.Windows.Controls;

namespace magicsim.components
{
    /// <summary>
    /// Interaction logic for PlayerCustomizationGrid.xaml
    /// </summary>
    public partial class PlayerCustomizationGrid : Grid
    {
        public PlayerCustomizationGrid()
        {
            InitializeComponent();
        }

        private void Tier_Change(object sender, RoutedEventArgs e)
        {
            var context = (CustomizationData)this.DataContext;
        }
    }
}
