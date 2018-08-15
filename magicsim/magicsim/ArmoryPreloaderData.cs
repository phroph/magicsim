using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace magicsim
{
    public class ArmoryPreloaderData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler PreloadingComplete;
        public event EventHandler PreloadingFailed;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        public String charName;
        private String _Label;
        public String Label
        {
            get { return _Label; }
            set
            {
                if (value != _Label)
                {
                    _Label = value;
                    OnPropertyChanged("Label");
                }
            }
        }

        public ArmoryPreloaderData()
        {
            Label = "Loading Armory Character";
        }

        public void LoadArmoryData(String region, String server, String name)
        {
            SimC simc = null;
            Label = "Acquiring SimC Executable";
            ThreadPool.QueueUserWorkItem((_) =>
            {
                try
                {
                    simc = SimCManager.AcquireSimC();
                    if(simc == null)
                    {
                        MessageBox.Show("Could not acquire SimC. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            PreloadingFailed(this, new EventArgs());
                        });
                        return;
                    }
                }
                catch(Exception e)
                {
                    MessageBox.Show("Could not acquire SimC because of an Exception: " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        PreloadingFailed(this, new EventArgs());
                    });
                    return;
                }
                Label = "Generating Armory Profile";
                var text = File.ReadAllText("templates/armory.simc").Replace("%region%", region).Replace("%realm%", server.Replace(" ", "-")).Replace("%name%", name);
                if (!Directory.Exists("characters"))
                {
                    Directory.CreateDirectory("characters");
                }
                if(File.Exists("characters/" + name + ".simc"))
                {
                    File.Delete("characters/" + name + ".simc");
                }
                File.WriteAllText("characters/" + name + ".simc", text);
                if (simc.RunSim("characters/" + name + ".simc"))
                {
                    Label = "Armory Profile Generated";
                    charName = name;
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        PreloadingComplete(this, new EventArgs());
                    });
                }
                else
                {
                    Label = "Failed to Generate Profile";
                    MessageBox.Show("Failed to generate profile. Please check your input and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    App.Current.Dispatcher.Invoke(() =>
                    {
                        PreloadingFailed(this, new EventArgs());
                    });
                }
            });
        }
    }
}
