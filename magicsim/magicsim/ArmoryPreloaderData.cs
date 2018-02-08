using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace magicsim
{
    public class ArmoryPreloaderData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler PreloadingComplete;

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
            SimC simc;
            Label = "Acquiring SimC Executable";
            ThreadPool.QueueUserWorkItem((_) =>
            {
                simc = SimCManager.AcquireSimC();
                Label = "Generating Armory Profile";
                var text = File.ReadAllText("templates/armory.simc").Replace("%region%", region).Replace("%realm%", server).Replace("%name%", name);
                if (!Directory.Exists("characters"))
                {
                    Directory.CreateDirectory("characters");
                }
                if(File.Exists("characters/" + name))
                {
                    File.Delete("characters/" + name);
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
                }
            });
        }
    }
}
