using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace magicsim
{
    public class ResultLoaderData : INotifyPropertyChanged
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

        // In case I do some shenanigans in the future.
        private object _resultLock;
        public ObservableCollection<Tag> TagList { get; set; }

        private Tag _selectedTag;
        public Tag SelectedTag
        {
            get { return _selectedTag; }
            set
            {
                if(_selectedTag != value)
                {
                    _selectedTag = value;
                    OnPropertyChanged("SelectedTag");
                }
            }
        }

        public ResultLoaderData()
        {
            _resultLock = new object();
            TagList = new ObservableCollection<Tag>();
            Application.Current.Dispatcher.Invoke(() =>
            {
                LoadResults();
            });
        }

        public void LoadResults()
        {
            if(!Directory.Exists("savedResults"))
            {
                return;
            }
            var tags = Directory.EnumerateDirectories("savedResults").Select(x => x.Split(Path.DirectorySeparatorChar).Last()).ToList();
            lock (_resultLock)
            {
                TagList.Clear();
                tags.ForEach(x => TagList.Add(new Tag { Label = x }));
            }
        }
    }
}
