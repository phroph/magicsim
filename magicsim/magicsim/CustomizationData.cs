using magicsim.objectModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace magicsim
{
    public class CustomizationData : INotifyPropertyChanged
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

        public Dictionary<string, Dictionary<string, List<Azerite>>> AzeriteMapping;
        public List<Consumable> FlaskNameMapping;
        public List<Consumable> PotionNameMapping;
        public List<Consumable> FoodNameMapping;
        public List<Consumable> AugmentNameMapping;

        public CustomizationData()
        {
            ModifyIlvl = false;
            ModifyTier = false;
            Class = "Class";
            Spec = "Spec";
            Name = "Name";
            Potions = new ObservableCollection<string>();
            Runes = new ObservableCollection<string>();
            Flasks = new ObservableCollection<string>();
            Foods = new ObservableCollection<string>();
            Ring1 = new ObservableCollection<string>();
            Ring2 = new ObservableCollection<string>();
            Ring4 = new ObservableCollection<string>();
            Ring1Mapping = new List<Azerite>();
            Ring2Mapping = new List<Azerite>();
            Ring4Mapping = new List<Azerite>();
        }

        private string _class;
        private string _spec;
        private string _potion;
        private string _flask;
        private string _food;
        private string _rune;
        private string _name;

        private string _headRing1;
        private string _headRing2;
        private string _headRing4;
        private string _chestRing1;
        private string _chestRing2;
        private string _chestRing4;
        private string _shoulderRing1;
        private string _shoulderRing2;
        private string _shoulderRing4;

        private bool _modifyIlvl;
        private bool _modifyTier;

        private float _ilvl;

        public ObservableCollection<String> Potions { get; set; }
        public ObservableCollection<String> Flasks { get; set; }
        public ObservableCollection<String> Runes { get; set; }
        public ObservableCollection<String> Foods { get; set; }
        public ObservableCollection<String> Ring1 { get; set; }
        public ObservableCollection<String> Ring2 { get; set; }
        public ObservableCollection<String> Ring4 { get; set; }
        public List<Azerite> Ring1Mapping { get; set; }
        public List<Azerite> Ring2Mapping { get; set; }
        public List<Azerite> Ring4Mapping { get; set; }

        public String HeadRing1
        {
            get { return _headRing1; }
            set
            {
                if (value != _headRing1)
                {
                    _headRing1 = value;
                    OnPropertyChanged("HeadRing1");
                }
            }
        }
        public String HeadRing2
        {
            get { return _headRing2; }
            set
            {
                if (value != _headRing2)
                {
                    _headRing2 = value;
                    OnPropertyChanged("HeadRing2");
                }
            }
        }
        public String HeadRing4
        {
            get { return _headRing4; }
            set
            {
                if (value != _headRing4)
                {
                    _headRing4 = value;
                    OnPropertyChanged("HeadRing4");
                }
            }
        }
        public String ChestRing1
        {
            get { return _chestRing1; }
            set
            {
                if (value != _chestRing1)
                {
                    _chestRing1 = value;
                    OnPropertyChanged("ChestRing1");
                }
            }
        }
        public String ChestRing2
        {
            get { return _chestRing2; }
            set
            {
                if (value != _chestRing2)
                {
                    _chestRing2 = value;
                    OnPropertyChanged("ChestRing2");
                }
            }
        }
        public String ChestRing4
        {
            get { return _chestRing4; }
            set
            {
                if (value != _chestRing4)
                {
                    _chestRing4 = value;
                    OnPropertyChanged("ChestRing4");
                }
            }
        }
        public String ShouldersRing1
        {
            get { return _shoulderRing1; }
            set
            {
                if (value != _shoulderRing1)
                {
                    _shoulderRing1 = value;
                    OnPropertyChanged("ShouldersRing1");
                }
            }
        }
        public String ShouldersRing2
        {
            get { return _shoulderRing2; }
            set
            {
                if (value != _shoulderRing2)
                {
                    _shoulderRing2 = value;
                    OnPropertyChanged("ShouldersRing2");
                }
            }
        }
        public String ShouldersRing4
        {
            get { return _shoulderRing4; }
            set
            {
                if (value != _shoulderRing4)
                {
                    _shoulderRing4 = value;
                    OnPropertyChanged("ShouldersRing4");
                }
            }
        }


        public String Class
        {
            get { return _class; }
            set
            {
                if (value != _class)
                {
                    _class = value;
                    OnPropertyChanged("Class");
                    OnPropertyChanged("SpecClass");
                }
            }
        }
        public String Name
        {
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        public String Spec
        {
            get { return _spec; }
            set
            {
                if (value != _spec)
                {
                    _spec = value;
                    OnPropertyChanged("Spec");
                    OnPropertyChanged("SpecClass");
                }
            }
        }
        public String SpecClass
        {
            get { return _spec + " " + _class; }
        }
        public String Potion
        {
            get { return _potion; }
            set
            {
                if (value != _potion)
                {
                    _potion = value;
                    OnPropertyChanged("Potion");
                }
            }
        }
        public String Flask
        {
            get { return _flask; }
            set
            {
                if (value != _flask)
                {
                    _flask = value;
                    OnPropertyChanged("Flask");
                }
            }
        }
        public String Food
        {
            get { return _food; }
            set
            {
                if (value != _food)
                {
                    _food = value;
                    OnPropertyChanged("Food");
                }
            }
        }
        public String Rune
        {
            get { return _rune; }
            set
            {
                if (value != _rune)
                {
                    _rune = value;
                    OnPropertyChanged("Rune");
                }
            }
        }
        
        public float ILvl
        {
            get { return _ilvl; }
            set
            {
                if (value != _ilvl)
                {
                    _ilvl = value;
                    OnPropertyChanged("ILvl");
                }
            }
        }
        public bool ModifyIlvl
        {
            get { return _modifyIlvl; }
            set
            {
                if (value != _modifyIlvl)
                {
                    _modifyIlvl = value;
                    OnPropertyChanged("ModifyIlvl");
                }
            }
        }
        public bool ModifyTier
        {
            get { return _modifyTier; }
            set
            {
                if (value != _modifyTier)
                {
                    _modifyTier = value;
                    OnPropertyChanged("ModifyTier");
                }
            }
        }
        
        public string GetSimcName(string value, List<Consumable> mapping)
        {
            foreach(var consumable in mapping)
            {
                if(consumable.name.Equals(value))
                {
                    return consumable.simc_names[0];
                }
            }
            return "";
        }
       

        private string _profileText;

        public string ConstructFinalProfile()
        {
            var nameClassRegex = new Regex("([^=]+)=\"?([^\r\n\"]+)\"?");
            var specRegex = new Regex("spec=(\\w+)");
            var artifactRegex = new Regex("artifact=([^\r\n]+)");
            var ilvlRegex = new Regex("gear_ilvl=(\\d+.?\\d*)");
            var tierRegex = new Regex("set_bonus=tier(\\d+)_(\\d)pc=1");
            var potionRegex = new Regex("potion=(\\w+)");
            var flaskRegex = new Regex("flask=(\\w+)");
            var foodRegex = new Regex("food=(\\w+)");
            var augmentRegex = new Regex("augmentation=(\\w+)");

            // Replace Ilvl if flagged, tier if flagged, potion,flask,food,augment
            _profileText = _profileText.Replace(potionRegex.Match(_profileText).Groups[0].Value, "potion=" + GetSimcName(Potion, PotionNameMapping));
            _profileText = _profileText.Replace(flaskRegex.Match(_profileText).Groups[0].Value, "flask=" + GetSimcName(Flask, FlaskNameMapping));
            _profileText = _profileText.Replace(foodRegex.Match(_profileText).Groups[0].Value, "food=" + GetSimcName(Food, FoodNameMapping));
            _profileText = _profileText.Replace(augmentRegex.Match(_profileText).Groups[0].Value, "augmentation=" + GetSimcName(Rune, AugmentNameMapping));
            
            _profileText = UnloadAzerite(_profileText);

            if(ModifyTier)
            {
                
            }
            if(ModifyIlvl)
            {
                _profileText += "\r\nscale_to_itemlevel=" + ILvl;
            }
            return _profileText;
        }

        private String UnloadAzerite(String profileText)
        {
            var _profileText = profileText;

            string headPowers = "";
            string shoulderPowers = "";
            string chestPowers = "";
            var azeriteHeadRegex = new Regex("head=(.*)azerite_powers=[^\r\n,]+(.*)");
            var azeriteShoulderRegex = new Regex("shoulders?=(.*)azerite_powers=[^\r\n,]+(.*)");
            var azeriteChestRegex = new Regex("chest=(.*)azerite_powers=[^\r\n,]+(.*)");

            if (Ring1Mapping.Exists(x => x.Name.Equals(HeadRing1)))
            {
                headPowers += Ring1Mapping.Find(x => x.Name.Equals(HeadRing1)).Id + "/";
            }
            if (Ring2Mapping.Exists(x => x.Name.Equals(HeadRing2)))
            {
                headPowers += Ring2Mapping.Find(x => x.Name.Equals(HeadRing2)).Id + "/";
            }
            if (Ring4Mapping.Exists(x => x.Name.Equals(HeadRing4)))
            {
                headPowers += Ring4Mapping.Find(x => x.Name.Equals(HeadRing4)).Id + "/";
            }

            if (Ring1Mapping.Exists(x => x.Name.Equals(ChestRing1)))
            {
                chestPowers += Ring1Mapping.Find(x => x.Name.Equals(ChestRing1)).Id + "/";
            }
            if (Ring2Mapping.Exists(x => x.Name.Equals(ChestRing2)))
            {
                chestPowers += Ring2Mapping.Find(x => x.Name.Equals(ChestRing2)).Id + "/";
            }
            if (Ring4Mapping.Exists(x => x.Name.Equals(ChestRing4)))
            {
                chestPowers += Ring4Mapping.Find(x => x.Name.Equals(ChestRing4)).Id + "/";
            }

            if (Ring1Mapping.Exists(x => x.Name.Equals(ShouldersRing1)))
            {
                shoulderPowers += Ring1Mapping.Find(x => x.Name.Equals(ShouldersRing1)).Id + "/";
            }
            if (Ring2Mapping.Exists(x => x.Name.Equals(ShouldersRing2)))
            {
                shoulderPowers += Ring2Mapping.Find(x => x.Name.Equals(ShouldersRing2)).Id + "/";
            }
            if (Ring4Mapping.Exists(x => x.Name.Equals(ShouldersRing4)))
            {
                shoulderPowers += Ring4Mapping.Find(x => x.Name.Equals(ShouldersRing4)).Id + "/";
            }
            shoulderPowers = shoulderPowers.ToString();

            if (headPowers.Length > 0)
            {
                if (azeriteHeadRegex.Match(profileText).Success)
                {
                    _profileText = _profileText.Replace(azeriteHeadRegex.Match(_profileText).Groups[0].Value, "head=" + azeriteHeadRegex.Match(_profileText).Groups[1] + "azerite_powers=" + headPowers.Substring(0, headPowers.Count() - 1) + azeriteHeadRegex.Match(_profileText).Groups[2]);
                }
                else
                {
                    _profileText = _profileText.Replace(new Regex("head=([^\r\n]*)").Match(_profileText).Groups[0].Value, new Regex("head=([^\r\n]*)").Match(_profileText).Groups[0].Value + ",azerite_powers=" + headPowers.Substring(0, headPowers.Count() - 1));
                }
            }


            if (shoulderPowers.Length > 0)
            {
                if (azeriteShoulderRegex.Match(profileText).Success)
                {
                    _profileText = _profileText.Replace(azeriteShoulderRegex.Match(_profileText).Groups[0].Value, "shoulders=" + azeriteShoulderRegex.Match(_profileText).Groups[1] + "azerite_powers=" + shoulderPowers.Substring(0, shoulderPowers.Count() - 1) + azeriteShoulderRegex.Match(_profileText).Groups[2]);
                }
                else
                {
                    _profileText = _profileText.Replace(new Regex("shoulders=([^\r\n]*)").Match(_profileText).Groups[0].Value, new Regex("shoulders=([^\r\n]*)").Match(_profileText).Groups[0].Value + ",azerite_powers=" + shoulderPowers.Substring(0, shoulderPowers.Count() - 1));
                }
            }

            if (chestPowers.Length > 0)
            {
                if (azeriteChestRegex.Match(profileText).Success)
                {
                    _profileText = _profileText.Replace(azeriteChestRegex.Match(_profileText).Groups[0].Value, "chest=" + azeriteChestRegex.Match(_profileText).Groups[1] + "azerite_powers=" + chestPowers.Substring(0, chestPowers.Count() - 1) + azeriteChestRegex.Match(_profileText).Groups[2]);
                }
                else
                {
                    _profileText = _profileText.Replace(new Regex("chest=([^\r\n]*)").Match(_profileText).Groups[0].Value, new Regex("chest=([^\r\n]*)").Match(_profileText).Groups[0].Value + ",azerite_powers=" + chestPowers.Substring(0, chestPowers.Count() - 1));
                }
            }

            return _profileText;
        }

        private void LoadAzerite(String profileText)
        {
            var azeriteHeadRegex = new Regex("head=.*azerite_powers=([^\r\n,]+).*");
            var azeriteShoulderRegex = new Regex("shoulders?=.*azerite_powers=([^\r\n,]+).*");
            var azeriteChestRegex = new Regex("chest=.*azerite_powers=([^\r\n,]+).*");
            var azeriteHelm = azeriteHeadRegex.Match(profileText);
            if (azeriteHelm.Success)
            {
                var value = azeriteHelm.Groups[1].Value;
                if (!azeriteHelm.Groups[1].Value.Contains("/"))
                {
                    if (Ring1Mapping.Exists(x => x.Id.Equals(value)))
                    {
                        HeadRing1 = Ring1Mapping.Find(x => x.Id.Equals(value)).Name;
                    }
                    else if (Ring2Mapping.Exists(x => x.Id.Equals(value)))
                    {
                        HeadRing2 = Ring2Mapping.Find(x => x.Id.Equals(value)).Name;
                    }
                    else if (Ring4Mapping.Exists(x => x.Id.Equals(value)))
                    {
                        HeadRing4 = Ring4Mapping.Find(x => x.Id.Equals(value)).Name;
                    }
                    // Ring3 gets skipped
                }
                else
                {
                    var traits = azeriteHelm.Groups[1].Value.Split('/');
                    foreach (var trait in traits)
                    {
                        if (Ring1Mapping.Exists(x => x.Id.Equals(trait)))
                        {
                            HeadRing1 = Ring1Mapping.Find(x => x.Id.Equals(trait)).Name;
                        }
                        else if (Ring2Mapping.Exists(x => x.Id.Equals(trait)))
                        {
                            HeadRing2 = Ring2Mapping.Find(x => x.Id.Equals(trait)).Name;
                        }
                        else if (Ring4Mapping.Exists(x => x.Id.Equals(trait)))
                        {
                            HeadRing4 = Ring4Mapping.Find(x => x.Id.Equals(trait)).Name;
                        }
                        // Ring3 gets skipped
                    }
                }
            }
            var azeriteShoulder = azeriteShoulderRegex.Match(profileText);
            if (azeriteShoulder.Success)
            {
                var value = azeriteShoulder.Groups[1].Value;
                if (!azeriteShoulder.Groups[1].Value.Contains("/"))
                {
                    if (Ring1Mapping.Exists(x => x.Id.Equals(value)))
                    {
                        ShouldersRing1 = Ring1Mapping.Find(x => x.Id.Equals(value)).Name;
                    }
                    else if (Ring2Mapping.Exists(x => x.Id.Equals(value)))
                    {
                        ShouldersRing2 = Ring2Mapping.Find(x => x.Id.Equals(value)).Name;
                    }
                    else if (Ring4Mapping.Exists(x => x.Id.Equals(value)))
                    {
                        ShouldersRing4 = Ring4Mapping.Find(x => x.Id.Equals(value)).Name;
                    }
                    // Ring3 gets skipped
                }
                else
                {
                    var traits = azeriteShoulder.Groups[1].Value.Split('/');
                    foreach (var trait in traits)
                    {
                        if (Ring1Mapping.Exists(x => x.Id.Equals(trait)))
                        {
                            ShouldersRing1 = Ring1Mapping.Find(x => x.Id.Equals(trait)).Name;
                        }
                        else if (Ring2Mapping.Exists(x => x.Id.Equals(trait)))
                        {
                            ShouldersRing2 = Ring2Mapping.Find(x => x.Id.Equals(trait)).Name;
                        }
                        else if (Ring4Mapping.Exists(x => x.Id.Equals(trait)))
                        {
                            ShouldersRing4 = Ring4Mapping.Find(x => x.Id.Equals(trait)).Name;
                        }
                        // Ring3 gets skipped
                    }
                }
            }
            var azeriteChest = azeriteChestRegex.Match(profileText);
            if (azeriteChest.Success)
            {
                var value = azeriteChest.Groups[1].Value;
                if (!azeriteChest.Groups[1].Value.Contains("/"))
                {
                    if (Ring1Mapping.Exists(x => x.Id.Equals(value)))
                    {
                        ChestRing1 = Ring1Mapping.Find(x => x.Id.Equals(value)).Name;
                    }
                    else if (Ring2Mapping.Exists(x => x.Id.Equals(value)))
                    {
                        ChestRing2 = Ring2Mapping.Find(x => x.Id.Equals(value)).Name;
                    }
                    else if (Ring4Mapping.Exists(x => x.Id.Equals(value)))
                    {
                        ChestRing4 = Ring4Mapping.Find(x => x.Id.Equals(value)).Name;
                    }
                    // Ring3 gets skipped
                }
                else
                {
                    var traits = azeriteChest.Groups[1].Value.Split('/');
                    foreach (var trait in traits)
                    {
                        if (Ring1Mapping.Exists(x => x.Id.Equals(trait)))
                        {
                            ChestRing1 = Ring1Mapping.Find(x => x.Id.Equals(trait)).Name;
                        }
                        else if (Ring2Mapping.Exists(x => x.Id.Equals(trait)))
                        {
                            ChestRing2 = Ring2Mapping.Find(x => x.Id.Equals(trait)).Name;
                        }
                        else if (Ring4Mapping.Exists(x => x.Id.Equals(trait)))
                        {
                            ChestRing4 = Ring4Mapping.Find(x => x.Id.Equals(trait)).Name;
                        }
                        // Ring3 gets skipped
                    }
                }
            }
        }

        public void LoadProfilePath(String path)
        {
            try
            {
                _profileText = File.ReadAllText(path);
                var profileText = _profileText;
                //Parse and update values.
                var nameClassRegex = new Regex("([^=]+)=\"?([^\r\n\"]+)\"?");
                var specRegex = new Regex("spec=(\\w+)");
                var ilvlRegex = new Regex("gear_ilvl=(\\d+.?\\d*)");
                var tierRegex = new Regex("set_bonus=tier(\\d+)_(\\d)pc=1");
                var potionRegex = new Regex("potion=(\\w+)");
                var flaskRegex = new Regex("flask=(\\w+)");
                var foodRegex = new Regex("food=(\\w+)");
                var augmentRegex = new Regex("augmentation=(\\w+)");
                
                var nameClassMatch = nameClassRegex.Match(profileText);
                Name = nameClassMatch.Groups[2].Value;
                Class = nameClassMatch.Groups[1].Value.UppercaseWords().Replace("Deathk", "Death K").Replace("Demonh", "Demon H").UppercaseWords();
                Spec = specRegex.Match(profileText).Groups[1].Value.UppercaseWords().Replace("Beastm", "Beast M");
                var fixedCulture = CultureInfo.CreateSpecificCulture("en-US");
                ILvl = float.Parse(ilvlRegex.Match(profileText).Groups[1].Value, fixedCulture); // Easy fix to convert EU to US style decimal notation.
                // Ring 1
                Ring1.Add("None");
                Ring1.Add("");
                Ring1.Add("- " + Class + " -");
                Ring1.Add("    - " + Spec + " -");
                AzeriteMapping[Class.ToLower().Replace(" ", "_")][Spec.ToLower().Replace(" ", "_")].ForEach(x => { Ring1.Add(x.Name); Ring1Mapping.Add(x); });
                Ring1.Add("");
                Ring1.Add("- PVE -");
                Ring1.Add("    - Magni -");
                AzeriteMapping["pve"]["magni"].ForEach(x => { Ring1.Add(x.Name); Ring1Mapping.Add(x); });
                Ring1.Add("");
                Ring1.Add("    - Uldir -");
                AzeriteMapping["pve"]["uldir"].ForEach(x => { Ring1.Add(x.Name); Ring1Mapping.Add(x); });
                Ring1.Add("");
                Ring1.Add("    - World -");
                AzeriteMapping["pve"]["location"].ForEach(x => { Ring1.Add(x.Name); Ring1Mapping.Add(x); });
                Ring1.Add("");
                Ring1.Add("    - Dungeons -");
                AzeriteMapping["pve"]["dungeon"].ForEach(x => { Ring1.Add(x.Name); Ring1Mapping.Add(x); });
                Ring1.Add("");
                Ring1.Add("- PVP -");
                Ring1.Add("    - Horde -");
                AzeriteMapping["pvp"]["horde"].ForEach(x => { Ring1.Add(x.Name); Ring1Mapping.Add(x); });
                Ring1.Add("");
                Ring1.Add("    - Alliance -");
                AzeriteMapping["pvp"]["alliance"].ForEach(x => { Ring1.Add(x.Name); Ring1Mapping.Add(x); });
                Ring1.Add("");
                Ring1.Add("- Professions -");
                Ring1.Add("    - Engineering -");
                AzeriteMapping["profession"]["engineering"].ForEach(x => { Ring1.Add(x.Name); Ring1Mapping.Add(x); });
                // Ring 2
                Ring2.Add("None");
                Ring2.Add("");
                Ring2.Add("- Role -");
                if (new List<String> { "Demon Hunter", "Death Knight", "Monk", "Paladin", "Druid", "Warrior" }.Contains(Class))
                {
                    Ring2.Add("    - Tank -");
                    AzeriteMapping["role"]["tank"].ForEach(x => { Ring2.Add(x.Name); Ring2Mapping.Add(x); });
                    Ring2.Add("");
                }
                if (new List<String> { "Paladin", "Priest", "Shaman", "Monk", "Druid" }.Contains(Class))
                {
                    Ring2.Add("    - Healer -");
                    AzeriteMapping["role"]["healer"].ForEach(x => { Ring2.Add(x.Name); Ring2Mapping.Add(x); });
                    Ring2.Add("");
                }
                Ring2.Add("    - DPS -");
                AzeriteMapping["role"]["dps"].ForEach(x => { Ring2.Add(x.Name); Ring2Mapping.Add(x); });
                Ring2.Add("");
                Ring2.Add("    - Any -");
                AzeriteMapping["role"]["any"].ForEach(x => { Ring2.Add(x.Name); Ring2Mapping.Add(x); });
                // Ring 4
                Ring4.Add("None");
                AzeriteMapping["generic"]["center"].ForEach(x => { Ring4.Add(x.Name); Ring4Mapping.Add(x); });

                HeadRing1 = "None";
                HeadRing2 = "None";
                HeadRing4 = "None";
                ChestRing1 = "None";
                ChestRing2 = "None";
                ChestRing4 = "None";
                ShouldersRing1 = "None";
                ShouldersRing2 = "None";
                ShouldersRing4 = "None";

                var potionName = potionRegex.Match(profileText).Groups[1].Value;
                var flaskName = flaskRegex.Match(profileText).Groups[1].Value;
                var foodName = foodRegex.Match(profileText).Groups[1].Value;
                var runeName = augmentRegex.Match(profileText).Groups[1].Value;
                if (PotionNameMapping.Exists(x => x.simc_names.Contains(potionName)))
                {
                    Potion = PotionNameMapping.First(x => x.simc_names.Contains(potionName)).name;
                }
                if (FlaskNameMapping.Exists(x => x.simc_names.Contains(flaskName)))
                {
                    Flask = FlaskNameMapping.First(x => x.simc_names.Contains(flaskName)).name;
                }
                if (FoodNameMapping.Exists(x => x.simc_names.Contains(foodName)))
                {
                    Food = FoodNameMapping.First(x => x.simc_names.Contains(foodName)).name;
                }
                if (AugmentNameMapping.Exists(x => x.simc_names.Contains(runeName)))
                {
                    Rune = AugmentNameMapping.First(x => x.simc_names.Contains(runeName)).name;
                }
                if (PotionNameMapping.Exists(x => x.simc_names.Contains(potionName)))
                {
                    Potion = PotionNameMapping.First(x => x.simc_names.Contains(potionName)).name;
                }
                LoadAzerite(profileText);

               var tierMatches = tierRegex.Matches(profileText);
                for (int i = 0; i < tierMatches.Count; i++)
                {
                    var match = tierMatches[i];
                    
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("Ran into an issue loading your profile: " + e.Message, "Error", MessageBoxButton.OK);
            }
        }
    }
}
