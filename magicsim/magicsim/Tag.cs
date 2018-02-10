using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace magicsim
{
    public class Tag : INotifyPropertyChanged
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

        private string _label;
        public string Label
        {
            get { return _label; }
            set
            {
                if (_label != value)
                {
                    try
                    {
                        if (value == "" || value == null)
                        {
                            MessageBox.Show("Invalid name. Try a different name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        if (_label != null && !_label.Equals(""))
                        {
                            Directory.Move("savedResults" + Path.DirectorySeparatorChar + _label, "savedResults" + Path.DirectorySeparatorChar + value);
                        }
                        _label = value;
                        OnPropertyChanged("Label");
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Could not rename this saved run. Try a different name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
