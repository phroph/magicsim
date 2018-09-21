using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace magicsim
{
    public class QuickSimCustomizationData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        
        SimData BackingData;
        public ObservableCollection<Sim> Sims
        {
            get
            {
                return BackingData.Sims;
            }
        }

        public ObservableCollection<string> Models
        {
            get
            {
                return BackingData.Models;
            }
        }

        public String SelectedModel
        {
            get { return BackingData._SelectedModel; }
            set
            {
                if (value != BackingData._SelectedModel)
                {
                    BackingData._SelectedModel = value;
                    OnPropertyChanged("SelectedModel");
                }
            }
        }

        public bool DisableStatWeights
        {
            get { return BackingData._DisableStatWeights; }
            set
            {
                if (value != BackingData._DisableStatWeights)
                {
                    BackingData._DisableStatWeights = value;
                    OnPropertyChanged("DisableStatWeights");
                }
            }
        }
        
        public bool DisableBuffs
        {
            get { return BackingData._DisableBuffs; }
            set
            {
                if (value != BackingData._DisableBuffs)
                {
                    BackingData._DisableBuffs = value;
                    OnPropertyChanged("DisableBuffs");
                }
            }
        }

        public void PopulateServers(List<string> servers)
        {
            this.Servers.Clear();
            servers.ForEach((server) =>
            {
                this.Servers.Add(server);
            });
            SelectedServer = Properties.Settings.Default.realmName;
        }

        public ObservableCollection<CustomizationData> CustomizationDataList { get; set; }


        public ObservableCollection<String> Servers { get; set; }
        public ObservableCollection<String> Regions { get; set; }
        private String _Name;
        public String Name
        {
            get { return _Name; }
            set
            {
                if (value != _Name)
                {
                    _Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public String _SelectedServer;
        public String _SelectedRegion;

        public String SelectedServer
        {
            get { return _SelectedServer; }
            set
            {
                if (value != _SelectedServer)
                {
                    _SelectedServer = value;
                    OnPropertyChanged("SelectedServer");
                }
            }
        }
        public String SelectedRegion
        {
            get { return _SelectedRegion; }
            set
            {
                if (value != _SelectedRegion)
                {
                    _SelectedRegion = value;
                    OnPropertyChanged("SelectedRegion");
                }
            }
        }

        public QuickSimCustomizationData()
        {
            BackingData = new SimData();
            CustomizationDataList = new ObservableCollection<CustomizationData>();
            Regions = new ObservableCollection<string>(new List<string> { "US", "EU", "KR", "TW" });
            Servers = new ObservableCollection<string>();
            Name = Properties.Settings.Default.characterName;
            SelectedRegion = Properties.Settings.Default.regionName;
        }


        public void LoadProfilePaths(List<String> paths)
        {
            paths.ForEach(path =>
            {
                var customizationData = new CustomizationData();
                customizationData.LoadProfilePath(path);
                CustomizationDataList.Add(customizationData);
            });
        }
    }
}
