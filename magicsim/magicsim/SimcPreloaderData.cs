using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace magicsim
{
    public class SimcPreloaderData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler PreloadingComplete;

        public string charName;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        
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

        public SimcPreloaderData()
        {
            Label = "Loading /SimC Character";
        }

        public void LoadArmoryData(String simcString)
        {
            SimC simc;
            Label = "Acquiring SimC Executable";
            ThreadPool.QueueUserWorkItem((_) =>
            {
                simc = SimCManager.AcquireSimC();
                Label = "Generating SimC Profile";
                if (!Directory.Exists("characters"))
                {
                    Directory.CreateDirectory("characters");
                }
                Regex nameRegex = new Regex("[^=]+=\"([^\"]+)\"");
                String name = nameRegex.Match(simcString).Groups[1].Value;
                if(File.Exists("characters/" + name))
                {
                    File.Delete("characters/" + name);
                }
                String text = simcString + "\nsave=./characters/" + name + ".simc";
                File.WriteAllText("characters/" + name + ".simc", text);
                if (simc.RunSim("characters/" + name + ".simc"))
                {
                    Label = "SimC Profile Generated";
                    charName = name;
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        PreloadingComplete(this, new EventArgs());
                    });
                }
                else
                {
                    Label = "Failed to Generate Profile";
                }
            });
        }
    }
}
