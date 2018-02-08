using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magicsim
{
    public class Sim : INotifyPropertyChanged
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

        private String _Profile;
        public String Profile
        {
            get { return _Profile; }
            set
            {
                if (value != _Profile)
                {
                    _Profile = value;
                    OnPropertyChanged("Profile");
                }
            }
        }

        private bool _Modifiable;
        public bool IsModifiable
        {
            get { return _Modifiable; }
            set
            {
                if (value != _Modifiable)
                {
                    _Modifiable = value;
                    OnPropertyChanged("IsModifiable");
                }
            }
        }

        public Sim()
        {
            IsModifiable = false;

        }
    }
}
