using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static magicsim.SimQueue;

namespace magicsim
{
    public class ResultData : INotifyPropertyChanged
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

        public ObservableCollection<SimResult> Results { get; set; }
        public ObservableCollection<PlayerResult> MergedResults { get; set; }
        private Dictionary<string, double> playerDpsValues;
        private Dictionary<string, double> playerMainStatValues;
        private Dictionary<string, string> playerMainStatTypes;
        private Dictionary<string, double> playerHasteValues;
        private Dictionary<string, double> playerCritValues;
        private Dictionary<string, double> playerMasteryValues;
        private Dictionary<string, double> playerVersValues;

        public ResultData()
        {
            Results = new ObservableCollection<SimResult>();
            MergedResults = new ObservableCollection<PlayerResult>();
            playerCritValues = new Dictionary<string, double>();
            playerDpsValues = new Dictionary<string, double>();
            playerHasteValues = new Dictionary<string, double>();
            playerMainStatValues = new Dictionary<string, double>();
            playerMainStatTypes = new Dictionary<string, string>();
            playerMasteryValues = new Dictionary<string, double>();
            playerVersValues = new Dictionary<string, double>();
            MergedResults.Add(new PlayerResult() { Dps = 2400000.23, Name = "Damocleysius", Spec = "Demon Hunter" });
            MergedResults.Add(new PlayerResult() { Dps = 2200000.23, Name = "Valaraukador", Spec = "Mage" });
            MergedResults.Add(new PlayerResult() { Dps = 2300000.23, Name = "Anduwrynn", Spec = "Warlock" });
            MergedResults.Add(new PlayerResult() { Dps = 2800000.23, Name = "Semidash", Spec = "Priest" });
            MergedResults.Add(new PlayerResult() { Dps = 2400000.23, Name = "Cyrukh", Spec = "Death Knight" });
            MergedResults.Add(new PlayerResult() { Dps = 2400000.23, Name = "Cyrukh", Spec = "Death Knight" });
        }

        public void LoadResultPath(String path)
        {
            var results = Directory.EnumerateFiles(path).Where((file) =>
            {
                return file.Contains(".json");
            });
            Results.Clear();
            results.ToList().ForEach((result) =>
            {
                using (StreamReader r = new StreamReader(path + "/" + result))
                {
                    string json = r.ReadToEnd();

                    SimResult res = JsonConvert.DeserializeObject<SimResult>(json);
                    res.filename = result;
                    Results.Add(res);
                }
            });
        }

        public void MergeResults(Model model)
        {
            playerCritValues = new Dictionary<string, double>();
            playerDpsValues = new Dictionary<string, double>();
            playerHasteValues = new Dictionary<string, double>();
            playerMainStatValues = new Dictionary<string, double>();
            playerMainStatTypes = new Dictionary<string, string>();
            playerMasteryValues = new Dictionary<string, double>();
            playerVersValues = new Dictionary<string, double>();
            Results.ToList().ForEach((result) =>
            {
                var splitIndex = result.filename.IndexOf('_');
                var time = result.filename.Substring(0, splitIndex);
                var fight = result.filename.Substring(splitIndex).Split('.')[0];
                if(model.model.ContainsKey(fight) && model.timeModel.ContainsKey(time))
                {
                    double modelWeight = model.model[fight];
                    double timeWeight = model.timeModel[time];
                    result.sim.players.ForEach((player) =>
                    {
                        double dps = player.collected_data.timeline_dmg.dps.mean;
                        string mainstat = "";
                        double mainstatValue = 0.0;
                        if(player.scale_factors.Int > 0)
                        {
                            mainstat = "Intellect";
                            mainstatValue = player.scale_factors.Int;
                        } else if(player.scale_factors.Agi > 0)
                        {
                            mainstat = "Agility";
                            mainstatValue = player.scale_factors.Agi;
                        } else if(player.scale_factors.Str > 0)
                        {
                            mainstat = "Strength";
                            mainstatValue = player.scale_factors.Str;
                        }
                        if (!playerCritValues.ContainsKey(player.name))
                        {
                            playerCritValues[player.name] = 0.0;
                        }
                        if (!playerDpsValues.ContainsKey(player.name))
                        {
                            playerDpsValues[player.name] = 0.0;
                        }
                        if (!playerHasteValues.ContainsKey(player.name))
                        {
                            playerHasteValues[player.name] = 0.0;
                        }
                        if (!playerMainStatValues.ContainsKey(player.name))
                        {
                            playerMainStatValues[player.name] = 0.0;
                        }
                        if (!playerMasteryValues.ContainsKey(player.name))
                        {
                            playerMasteryValues[player.name] = 0.0;
                        }
                        if (!playerVersValues.ContainsKey(player.name))
                        {
                            playerVersValues[player.name] = 0.0;
                        }
                        if (!playerMainStatTypes.ContainsKey(player.name))
                        {
                            playerMainStatTypes[player.name] = mainstat;
                        }
                        playerCritValues[player.name] += modelWeight * timeWeight * player.scale_factors.Crit / mainstatValue;
                        playerDpsValues[player.name] += modelWeight * timeWeight * dps;
                        playerHasteValues[player.name] += modelWeight * timeWeight * player.scale_factors.Haste / mainstatValue;
                        playerMainStatValues[player.name] += modelWeight * timeWeight;
                        playerMasteryValues[player.name] += modelWeight * timeWeight * player.scale_factors.Mastery / mainstatValue;
                        playerVersValues[player.name] += modelWeight * timeWeight * player.scale_factors.Vers / mainstatValue;
                    });
                }
            });

            List<PlayerResult> sublist = new List<PlayerResult>();
            foreach(string key in playerMainStatTypes.Keys)
            {
                var playerRes = new PlayerResult();
                playerRes.Crit = playerCritValues[key];
                playerRes.Dps = playerDpsValues[key];
                playerRes.Haste = playerHasteValues[key];
                playerRes.MainstatValue = playerMainStatValues[key];
                playerRes.MainstatType = playerMainStatTypes[key];
                playerRes.Mastery = playerMasteryValues[key];
                playerRes.Vers = playerVersValues[key];
                sublist.Add(playerRes);
            }
            MergedResults.Clear();
            MergedResults.Concat(sublist.OrderByDescending(player => player.Dps));
        }
    }
}
