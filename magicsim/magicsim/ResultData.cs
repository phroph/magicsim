using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using static magicsim.SimQueue;
using System.Globalization;
using System.Threading;

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
        private PlayerResult _selectedPlayer;
        public PlayerResult SelectedPlayer
        {
            get { return _selectedPlayer; }
            set
            {
                if (value != _selectedPlayer)
                {
                    _selectedPlayer = value;
                    GetPawnString(value);
                    GetSummaryString(value);
                    OnPropertyChanged("SelectedPlayer");
                }
            }
        }
        private string _pawnString;
        public string PawnString
        {
            get { return _pawnString; }
            set
            {
                if (value != _pawnString)
                {
                    _pawnString = value;
                    OnPropertyChanged("PawnString");
                }
            }
        }

        private string _summaryString;
        public string SummaryString
        {
            get { return _summaryString; }
            set
            {
                if (value != _summaryString)
                {
                    _summaryString = value;
                    OnPropertyChanged("SummaryString");
                }
            }
        }

        private Dictionary<string, double> playerDpsValues;
        private Dictionary<string, double> playerDamageValues;
        private Dictionary<string, double> playerMainStatValues;
        private Dictionary<string, string> playerMainStatTypes;
        private Dictionary<string, double> playerHasteValues;
        private Dictionary<string, double> playerCritValues;
        private Dictionary<string, double> playerMasteryValues;
        private Dictionary<string, double> playerVersValues;
        private Dictionary<string, string> playerSpecs;
        private Dictionary<string, string> playerClasses;

        public string _modelName;
        public string ModelName
        {
            get { return _modelName; }
            set
            {
                if (value != _modelName)
                {
                    _modelName = value;
                    OnPropertyChanged("ModelName");
                }
            }
        }

        public string _modelNameShort;
        public string ModelNameShort
        {
            get { return _modelNameShort; }
            set
            {
                if (value != _modelNameShort)
                {
                    _modelName = value;
                    OnPropertyChanged("ModelNameShort");
                }
            }
        }

        public string _tag;
        public string Tag
        {
            get { return _tag; }
            set
            {
                if (value != _tag)
                {
                    _tag = value;
                    OnPropertyChanged("Tag");
                }
            }
        }

        public ResultData()
        {
            Results = new ObservableCollection<SimResult>();
            MergedResults = new ObservableCollection<PlayerResult>();
            playerCritValues = new Dictionary<string, double>();
            playerDpsValues = new Dictionary<string, double>();
            playerDamageValues = new Dictionary<string, double>();
            playerHasteValues = new Dictionary<string, double>();
            playerMainStatValues = new Dictionary<string, double>();
            playerMainStatTypes = new Dictionary<string, string>();
            playerMasteryValues = new Dictionary<string, double>();
            playerVersValues = new Dictionary<string, double>();
            playerSpecs = new Dictionary<string, string>();
            playerClasses = new Dictionary<string, string>();
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

        public void MergeResults(Model model, string guid)
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            Tag = "";
            if (currentCulture.DateTimeFormat.ShortDatePattern.IndexOf("M") < currentCulture.DateTimeFormat.ShortDatePattern.IndexOf("d"))
            {
                Tag = DateTime.Today.Month.ToString().PadLeft(2, '0') + DateTime.Today.Day.ToString().PadLeft(2, '0') + DateTime.Today.Year.ToString();
            }
            else
            {
                Tag = DateTime.Today.Day.ToString().PadLeft(2, '0') + DateTime.Today.Month.ToString().PadLeft(2, '0') + DateTime.Today.Year.ToString();
            }
            Tag += "-" + guid;
            playerCritValues.Clear();
            playerDpsValues.Clear();
            playerHasteValues.Clear();
            playerMainStatValues.Clear();
            playerMainStatTypes.Clear();
            playerMasteryValues.Clear();
            playerVersValues.Clear();
            double minDps = double.MaxValue;
            ModelName = model.name;
            ModelNameShort = model.dispName;
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
                        double dps = player.collected_data.dps.mean;
                        double damage = player.collected_data.dmg.mean;
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
                        if (!playerDamageValues.ContainsKey(player.name))
                        {
                            playerDamageValues[player.name] = 0.0;
                        }
                        if (!playerHasteValues.ContainsKey(player.name))
                        {
                            playerHasteValues[player.name] = 0.0;
                        }
                        if (!playerMainStatValues.ContainsKey(player.name))
                        {
                            playerMainStatValues[player.name] = 0.0;
                        }
                        if (!playerMainStatTypes.ContainsKey(player.name) && mainstat.Length > 0)
                        {
                            playerMainStatTypes[player.name] = mainstat;
                        }
                        if (!playerMasteryValues.ContainsKey(player.name))
                        {
                            playerMasteryValues[player.name] = 0.0;
                        }
                        if (!playerVersValues.ContainsKey(player.name))
                        {
                            playerVersValues[player.name] = 0.0;
                        }
                        if (!playerClasses.ContainsKey(player.name))
                        {
                            var specClass = player.specialization.Replace("Death K", "Deathk").Replace("Demon H", "Demonh").Replace("Beast M", "Beastm").Split(' ');
                            playerClasses[player.name] = specClass[1];
                            playerSpecs[player.name] = specClass[0];
                        }
                        playerCritValues[player.name] += modelWeight * timeWeight * player.scale_factors.Crit;
                        playerDpsValues[player.name] += modelWeight * timeWeight * dps;
                        playerDamageValues[player.name] += modelWeight * timeWeight * damage;
                        playerHasteValues[player.name] += modelWeight * timeWeight * player.scale_factors.Haste;
                        playerMainStatValues[player.name] += modelWeight * timeWeight * mainstatValue;
                        playerMasteryValues[player.name] += modelWeight * timeWeight * player.scale_factors.Mastery;
                        playerVersValues[player.name] += modelWeight * timeWeight * player.scale_factors.Vers;
                    });
                }
            });
            List<PlayerResult> sublist = new List<PlayerResult>();
            foreach(string key in playerMainStatTypes.Keys)
            {
                var playerRes = new PlayerResult();
                playerRes.Dps = playerDpsValues[key];
                if (playerRes.Dps < minDps)
                {
                    minDps = playerRes.Dps;
                }
                playerRes.Damage = playerDamageValues[key];

                playerRes.Class = playerClasses[key];
                playerRes.ClassReadable = playerRes.Class.Replace("Deathk", "Death K").Replace("Demonh", "Demon H");
                playerRes.Spec = playerSpecs[key];
                playerRes.SpecReadble = playerRes.Spec.Replace("Beastm", "Beast M");

                if (playerMainStatTypes.ContainsKey(key))
                {
                    playerRes.MainstatType = playerMainStatTypes[key];
                    playerRes.MainstatValue = playerMainStatValues[key] / playerMainStatValues[key];
                    if (playerHasteValues.ContainsKey(key))
                    {
                        playerRes.Haste = playerHasteValues[key] / playerRes.MainstatValue;
                    }
                    if (playerCritValues.ContainsKey(key))
                    {
                        playerRes.Crit = playerCritValues[key] / playerRes.MainstatValue;
                    }
                    if (playerMasteryValues.ContainsKey(key))
                    {
                        playerRes.Mastery = playerMasteryValues[key] / playerRes.MainstatValue;
                    }
                    if (playerVersValues.ContainsKey(key))
                    {
                        playerRes.Vers = playerVersValues[key] / playerRes.MainstatValue;
                    }
                } else
                {
                    playerRes.MainstatType = "";
                }
                sublist.Add(playerRes);
            }

            sublist.ForEach((list) =>
            {
                if(list.Dps != minDps)
                {
                    list.DpsBoost = ((list.Dps / minDps) * 100.0).ToString("F2");
                }
            });
            MergedResults.Clear();
            MergedResults.Concat(sublist.OrderByDescending(player => player.Dps));
            SaveResults(guid);
        }

        public void SaveResults(string guid)
        {
            var resultJson = JsonConvert.SerializeObject(MergedResults.ToList());
            if(!Directory.Exists("result"))
            {
                Directory.CreateDirectory("result");
            }
            
            string dir = "result/" + Tag;
            int suffix = 0;
            string fixedDir = dir;
            if(Directory.Exists(dir))
            {
                fixedDir = dir + suffix;
            }
            while(Directory.Exists(fixedDir))
            {
                fixedDir = dir + ++suffix;
            }
            dir = fixedDir;
            Directory.CreateDirectory(dir);

            File.WriteAllText(dir + "/MergedResults.json", resultJson);
        }

        public string GetPawnString(PlayerResult player)
        {
            if(player.MainstatType == "")
            {
                return "No Pawn strings were generated in this run.";
            }
            string pawnString = "( Pawn: v1: \"" + player.Name + "_" + ModelNameShort + "_selfsim\": Class=" + player.Class + ", Spec=" + player.Spec + ", " + player.MainstatType + "=" + player.MainstatValue.ToString("F4");
            if(player.Haste > 0.0)
            {
                pawnString += ", HasteRating=" + player.Haste.ToString("F4");
            }
            if (player.Crit > 0.0)
            {
                pawnString += ", CritRating=" + player.Crit.ToString("F4");
            }
            if (player.Mastery > 0.0)
            {
                pawnString += ", MasteryRating=" + player.Mastery.ToString("F4");
            }
            if (player.Vers > 0.0)
            {
                pawnString += ", Versatility=" + player.Vers.ToString("F4");
            }
            pawnString += " )";
            return pawnString;
        }

        public string GetSummaryString(PlayerResult player)
        {
            return string.Format("{0}  -  {1} ( {2} Total )", player.Name, player.Dps, player.Damage);
        }

        public void LoadResults(string tag)
        {
            if (!Directory.Exists("result"))
            {
                return;
            }
            string dir = "result/" + tag;
            if (Directory.Exists(dir))
            {
                var resultJson = File.ReadAllText(dir + "/MergedResults.json");
                MergedResults.Clear();
                MergedResults.Concat(JsonConvert.DeserializeObject<List<PlayerResult>>(resultJson));
            }
        }
    }
}
