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

        // profileset, resultname regex, process count, failure callback, success callback

        public void ExecuteSimRun(List<string> profiles, Regex filenameRegex, int processCount, EventHandler failureHandler, EventHandler successHandler, String writeDir, String resultDir, Boolean hasGuid)
        {
            try
            {
                RunningComplete += successHandler;
                RunningFailed += failureHandler;
                SimC simc;
                Total = profiles.Count();
                Completed = 0;
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
                        while (true)
                        {
                            var profile = "";
                            lock (simLock)
                            {
                                if (topHandle == profiles.Count)
                                {
                                    return;
                                }
                                profile = profiles[topHandle];
                                topHandle++;
                            }
                            var fileName = filenameRegex.Match(profile).Groups[1].Value;
                            if (!Directory.Exists(resultDir + Path.DirectorySeparatorChar))
                            {
                                Directory.CreateDirectory(resultDir + Path.DirectorySeparatorChar);
                            }


                            if (hasGuid)
                            {
                                guid = fileName.Split(Path.DirectorySeparatorChar)[0];
                                if (!Directory.Exists(writeDir + Path.DirectorySeparatorChar + guid))
                                {
                                    Directory.CreateDirectory(writeDir + Path.DirectorySeparatorChar + guid);
                                }
                                if (!Directory.Exists(resultDir + Path.DirectorySeparatorChar + guid))
                                {
                                    Directory.CreateDirectory(resultDir + Path.DirectorySeparatorChar + guid);
                                }
                            }

                            File.WriteAllText(writeDir + Path.DirectorySeparatorChar + fileName + ".simc", profile);
                            if (simc.RunSim(writeDir + Path.DirectorySeparatorChar + fileName + ".simc"))
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
                                return;
                            }
                        }
                    });
                }
            }
            finally
            {

                RunningComplete -= successHandler;
                RunningFailed -= failureHandler;
            }
        }
    }
}
