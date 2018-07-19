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

        private Dictionary<String, String> FlaskNameMapping = new Dictionary<string, string>
        {
            {"whispered_pact", "Flask of the Whispered Pact"},
            {"seventh_demon", "Flask of the Seventh Demon"},
            {"ten_thousand_scars", "Flask of Ten Thousand Scars"},
            {"countless_armies", "Flask of the Countless Armies"},
            {"flask_of_the_whispered_pact", "Flask of the Whispered Pact"},
            {"flask_of_the_seventh_demon", "Flask of the Seventh Demon"},
            {"flask_of_ten_thousand_scars", "Flask of Ten Thousand Scars"},
            {"flask_of_the_countless_armies", "Flask of the Countless Armies"}
        };

        private Dictionary<String, String> PotionNameMapping = new Dictionary<string, string>
        {
            {"prolonged_power", "Potion of Prolonged Power"},
            {"unbending", "Unbending Potion"},
            {"deadly_grace", "Potion of Deadly Grace"},
            {"old_war", "Potion of the Old War"}
        };

        private Dictionary<String, String> FoodNameMapping = new Dictionary<string, string>
        {
            {"the_hungry_magister", "The Hungry Magister"},
            {"hungry_magister", "The Hungry Magister"},
            {"seed-battered_fish_plate", "Seed-Battered Fish Plate"},
            {"nightborne_delicacy_platter", "Nightborne Delicacy Platter"},
            {"azshari_salad", "Azshari Salad"},
            {"lemon_herb_filet", "Lemon Herb Filet" },
            {"lavish_suramar_feast", "Lavish Suramar Feast" }
        };

        private Dictionary<String, String> AugmentNameMapping = new Dictionary<string, string>
        {
            {"defiled", "Defiled Augment Rune"}
        };

        public CustomizationData()
        {
            ModifyIlvl = false;
            ModifyTier = false;
            Class = "Class";
            Spec = "Spec";
            Name = "Name";
            Potions = new ObservableCollection<string>(new List<string> { "Unbending Potion", "Potion of Prolonged Power", "Potion of Deadly Grace", "Potion of the Old War" });
            Runes = new ObservableCollection<string>(new List<string> { "Defiled Augment Rune" });
            Flasks = new ObservableCollection<string>(new List<string> { "Flask of the Whispered Pact", "Flask of the Seventh Demon", "Flask of Ten Thousand Scars", "Flask of the Countless Armies" });
            Foods = new ObservableCollection<string>(new List<string> { "Azshari Salad", "Nightborne Delicacy Platter", "Seed-Battered Fish Plate", "The Hungry Magister", "Lemon Herb Filet", "Lavish Suramar Feast" });
            Potion = Potions[0];
            Rune = Runes[0];
            Flask = Flasks[0];
            Food = Foods[0];
        }

        private string _class;
        private string _spec;
        private string _potion;
        private string _flask;
        private string _food;
        private string _rune;
        private string _name;

        private bool _t212pc;
        private bool _t214pc;
        private bool _t202pc;
        private bool _t204pc;
        private bool _t192pc;
        private bool _t194pc;

        private bool _modifyIlvl;
        private bool _modifyTier;

        private float _ilvl;

        public ObservableCollection<String> Potions { get; set; }
        public ObservableCollection<String> Flasks { get; set; }
        public ObservableCollection<String> Runes { get; set; }
        public ObservableCollection<String> Foods { get; set; }
       
        
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

        public bool T212pc
        {
            get { return _t212pc; }
            set
            {
                if (value != _t212pc)
                {
                    _t212pc = value;
                    OnPropertyChanged("T212pc");
                }
            }
        }
        public bool T214pc
        {
            get { return _t214pc; }
            set
            {
                if (value != _t214pc)
                {
                    _t214pc = value;
                    OnPropertyChanged("T214pc");
                }
            }
        }
        public bool T202pc
        {
            get { return _t202pc; }
            set
            {
                if (value != _t202pc)
                {
                    _t202pc = value;
                    OnPropertyChanged("T202pc");
                }
            }
        }
        public bool T204pc
        {
            get { return _t204pc; }
            set
            {
                if (value != _t204pc)
                {
                    _t204pc = value;
                    OnPropertyChanged("T204pc");
                }
            }
        }
        public bool T192pc
        {
            get { return _t192pc; }
            set
            {
                if (value != _t192pc)
                {
                    _t192pc = value;
                    OnPropertyChanged("T192pc");
                }
            }
        }
        public bool T194pc
        {
            get { return _t194pc; }
            set
            {
                if (value != _t194pc)
                {
                    _t194pc = value;
                    OnPropertyChanged("T194pc");
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
        
        public string GetKeyForValue(string value, Dictionary<string,string> mapping)
        {
            foreach(var key in mapping.Keys)
            {
                if(mapping[key].Equals(value))
                {
                    return key;
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
            _profileText = _profileText.Replace(potionRegex.Match(_profileText).Groups[0].Value, "potion=" + GetKeyForValue(Potion, PotionNameMapping));
            _profileText = _profileText.Replace(flaskRegex.Match(_profileText).Groups[0].Value, "flask=" + GetKeyForValue(Flask, FlaskNameMapping));
            _profileText = _profileText.Replace(foodRegex.Match(_profileText).Groups[0].Value, "food=" + GetKeyForValue(Food, FoodNameMapping));
            _profileText = _profileText.Replace(augmentRegex.Match(_profileText).Groups[0].Value, "augmentation=" + GetKeyForValue(Rune, AugmentNameMapping));
            if(ModifyTier)
            {
                if(T212pc)
                {
                    _profileText += "\r\nset_bonus=tier21_2pc=1";
                } else
                {
                    _profileText += "\r\nset_bonus=tier21_2pc=0";
                }
                if (T214pc)
                {
                    _profileText += "\r\nset_bonus=tier21_4pc=1";
                } else
                {
                    _profileText += "\r\nset_bonus=tier21_4pc=0";
                }
                if (T202pc)
                {
                    _profileText += "\r\nset_bonus=tier20_2pc=1";
                } else
                {
                    _profileText += "\r\nset_bonus=tier20_2pc=0";
                }
                if (T204pc)
                {
                    _profileText += "\r\nset_bonus=tier20_4pc=1";
                } else
                {
                    _profileText += "\r\nset_bonus=tier20_4pc=0";
                }
                if (T192pc)
                {
                    _profileText += "\r\nset_bonus=tier19_2pc=1";
                } else
                {
                    _profileText += "\r\nset_bonus=tier19_2pc=0";
                }
                if (T194pc)
                {
                    _profileText += "\r\nset_bonus=tier19_4pc=1";
                } else
                {
                    _profileText += "\r\nset_bonus=tier19_4pc=0";
                }
            }
            if(ModifyIlvl)
            {
                _profileText += "\r\nscale_to_itemlevel=" + ILvl;
            }
            return _profileText;
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

                var potionName = potionRegex.Match(profileText).Groups[1].Value;
                var flaskName = flaskRegex.Match(profileText).Groups[1].Value;
                var foodName = foodRegex.Match(profileText).Groups[1].Value;
                var runeName = augmentRegex.Match(profileText).Groups[1].Value;
                if (PotionNameMapping.ContainsKey(potionName))
                {
                    Potion = PotionNameMapping[potionName];
                }
                if (FlaskNameMapping.ContainsKey(flaskName))
                {
                    Flask = FlaskNameMapping[flaskName];
                }
                if (FoodNameMapping.ContainsKey(foodName))
                {
                    Food = FoodNameMapping[foodName];
                }
                if (AugmentNameMapping.ContainsKey(runeName))
                {
                    Rune = AugmentNameMapping[runeName];
                }

                var tierMatches = tierRegex.Matches(profileText);
                for (int i = 0; i < tierMatches.Count; i++)
                {
                    var match = tierMatches[i];
                    if (match.Groups[1].Value.Equals("21"))
                    {
                        if (match.Groups[2].Value.Equals("2"))
                        {
                            T212pc = true;
                        }
                        else if (match.Groups[2].Value.Equals("4"))
                        {
                            T214pc = true;
                        }
                    }
                    if (match.Groups[1].Value.Equals("20"))
                    {
                        if (match.Groups[2].Value.Equals("2"))
                        {
                            T202pc = true;
                        }
                        else if (match.Groups[2].Value.Equals("4"))
                        {
                            T204pc = true;
                        }
                    }
                    if (match.Groups[1].Equals("19"))
                    {
                        if (match.Groups[2].Value.Equals("2"))
                        {
                            T192pc = true;
                        }
                        else if (match.Groups[2].Value.Equals("4"))
                        {
                            T194pc = true;
                        }
                    }
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("Ran into an issue loading your profile: " + e.Message, "Error", MessageBoxButton.OK);
            }
        }
    }
}
