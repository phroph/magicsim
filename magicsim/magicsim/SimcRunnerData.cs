using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using static magicsim.SimQueue;

namespace magicsim
{
    public class SimcRunnerData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler RunningComplete;
        public event EventHandler RunningFailed;

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
            if (0 == Total)
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    Label = "All Sims Completed";
                    RunningComplete(this, new EventArgs());
                });
                return;
            }
            simc = SimCManager.AcquireSimC();
            for (int i = 0; i < processCount; i++)
            {
                Label = "Running Simc Profiles";
                ThreadPool.QueueUserWorkItem((_) =>
                {
                    var filenameRegex = new Regex(String.Format("json2=results\\{0}([^\\{0}]+\\{0}[^\\{0}]+).json", Path.DirectorySeparatorChar));
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
                        guid = fileName.Split(Path.DirectorySeparatorChar)[0];
                        if(!Directory.Exists("sims" + Path.DirectorySeparatorChar + guid))
                        {
                            Directory.CreateDirectory("sims" + Path.DirectorySeparatorChar + guid);
                        }
                        if (!Directory.Exists("results" + Path.DirectorySeparatorChar))
                        {
                            Directory.CreateDirectory("results" + Path.DirectorySeparatorChar);
                        }
                        if (!Directory.Exists("results" + Path.DirectorySeparatorChar + guid))
                        {
                            Directory.CreateDirectory("results" + Path.DirectorySeparatorChar + guid);
                        }

                        File.WriteAllText("sims" + Path.DirectorySeparatorChar + fileName + ".simc", profile);
                        if (simc.RunSim("sims" + Path.DirectorySeparatorChar + fileName + ".simc"))
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
                            MessageBox.Show("Sims failed to be ran. Try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            App.Current.Dispatcher.Invoke(() =>
                            {
                                RunningFailed(this, new EventArgs());
                            });
                        }
                    }
                });
            }
        }
    }
}
