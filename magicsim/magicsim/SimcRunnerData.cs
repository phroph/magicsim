using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static magicsim.SimQueue;

namespace magicsim
{
    public class SimcRunnerData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler RunningComplete;

        public string guid;
        public Model model;

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
        private int _Completed;
        public int Completed
        {
            get { return _Completed; }
            set
            {
                if (value != _Completed)
                {
                    _Completed = value;
                    OnPropertyChanged("Completed");
                }
            }
        }

        private int _Total;
        public int Total
        {
            get { return _Total; }
            set
            {
                if (value != _Total)
                {
                    _Total = value;
                    OnPropertyChanged("Total");
                }
            }
        }


        public SimcRunnerData()
        {
            Label = "Running Sims";
        }

        public Object simLock = new Object();

        public void LoadSimLoadout(List<string> profileset, int processCount, Model model)
        {
            SimC simc;
            Total = profileset.Count();
            Completed = 0;
            this.model = model;
            Label = "Acquiring SimC Executable";
            var topHandle = 0;
            simc = SimCManager.AcquireSimC();
            for (int i = 0; i < processCount; i++)
            {
                Label = "Running Simc Profiles";
                ThreadPool.QueueUserWorkItem((_) =>
                {
                    var filenameRegex = new Regex("json2=results/([^/]+/[^/]+).json");
                    while (true)
                    {
                        var profile = "";
                        lock (simLock)
                        {
                            if(topHandle == profileset.Count)
                            {
                                return;
                            }
                            profile = profileset[topHandle];
                            topHandle++;
                        }
                        var fileName = filenameRegex.Match(profile).Groups[1].Value;
                        guid = fileName.Split('/')[0];
                        if(!Directory.Exists("sims/" + guid))
                        {
                            Directory.CreateDirectory("sims/" + guid);
                        }
                        if (!Directory.Exists("results/"))
                        {
                            Directory.CreateDirectory("results/");
                        }
                        if (!Directory.Exists("results/" + guid))
                        {
                            Directory.CreateDirectory("results/" + guid);
                        }

                        File.WriteAllText("sims/" + fileName + ".simc", profile);
                        if (simc.RunSim("sims/" + fileName + ".simc"))
                        {
                            Completed++;
                            App.Current.Dispatcher.Invoke(() =>
                            {
                                if (Completed == Total)
                                {
                                    Label = "All Sims Completed";
                                    RunningComplete(this, new EventArgs());
                                }
                            });
                        }
                        else
                        {
                            Label = "Failed to Run a Sim";
                        }
                    }
                });

            }
        }
    }
}
