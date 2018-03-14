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
    public class BulkCustomizationData : INotifyPropertyChanged
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

        public ObservableCollection<CustomizationData> CustomizationDataList { get; set; }

        public BulkCustomizationData()
        {
            CustomizationDataList = new ObservableCollection<CustomizationData>();
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
