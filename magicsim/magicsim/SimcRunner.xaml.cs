using System;
using System.Linq;
using System.IO;
using System.Windows;
using System.Collections.Generic;
using static magicsim.SimQueue;
using System.Text.RegularExpressions;

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

            ((SimcRunnerData)this.DataContext).WindowCleanup += SimcRunner_WindowCleanup;
        }

        private void SimcRunner_WindowCleanup(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
