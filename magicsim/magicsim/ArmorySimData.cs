using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magicsim
{
    public class ArmorySimData : INotifyPropertyChanged
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

        public ObservableCollection<String> Servers { get; set; }
        public ObservableCollection<String> Regions { get; set; }
        private String _Name;
        public String Name
        {
            get { return _Name; }
            set
            {
                if(value != _Name)
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

        public ArmorySimData()
        {
            Regions = new ObservableCollection<string>(new List<string> { "US", "EU", "KR", "TW" });
            Servers = new ObservableCollection<string>();

            Name = Properties.Settings.Default.characterName;
            SelectedRegion = Properties.Settings.Default.regionName;
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
    }
}
