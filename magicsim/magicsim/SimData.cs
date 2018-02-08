using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace magicsim
{
    public class SimData
    {
        public ObservableCollection<Sim> Sims { get; set; }
        public ObservableCollection<PantheonUser> PantheonUsers { get; set; }
        public ObservableCollection<string> PantheonTrinkets { get; set; }
        public ObservableCollection<string> Models { get; set; }

        internal float _PantheonHastePercent;
        internal string _SelectedPantheonTrinket;
        internal bool _PantheonTrinketsEnabled;
        internal bool _ReforgeEnabled;
        internal bool _DisableSephuz;
        internal bool _DisableStatWeights;
        internal bool _PTRMode;
        internal bool _ReforgeCrit;
        internal bool _ReforgeMastery;
        internal bool _ReforgeHaste;
        internal bool _ReforgeVers;
        internal int _Threads;
        internal int _Processes;
        internal int _ReforgeStepSize;
        internal int _ReforgeAmount;
        internal string _SelectedModel;

        

        public SimData()
        {
            Sims = new ObservableCollection<Sim>();
            PantheonUsers = new ObservableCollection<PantheonUser>();
            PantheonTrinkets = new ObservableCollection<string>();
            Models = new ObservableCollection<string>();
            _Processes = 1;
            _Threads = 1;
        }
    }
}