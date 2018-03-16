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
                        if(!Directory.Exists("savedResults"))
                        {
                            MessageBox.Show("No saved runs to rename. Run a sim first.", "Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
                        }
                        if (value == "" || value == null)
                        {
                            MessageBox.Show("Invalid name. Try a different name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        if (_label != null)
                        {
                            if (Directory.EnumerateDirectories("savedResults").Where(x => x.Split(Path.DirectorySeparatorChar).Last().Equals(_label)).Count() > 0)
                            {
                                Directory.Move("savedResults" + Path.DirectorySeparatorChar + _label, "savedResults" + Path.DirectorySeparatorChar + value);
                            }
                            else
                            {
                                MessageBox.Show("Couldn't find the result to rename.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
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
