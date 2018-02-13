using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
            {"countless_armies", "Flask of the Countless Armies"}
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
            {"seed-battered_fish_plate", "Seed-Battered Fish PLate"},
            {"nightborne_delicacy_platter", "Nightborne Delicacy Platter"},
            {"azshari_salad", "Azshari Salad"},
            {"lemon_herb_filet", "Lemon Herb Filet" }
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
            Foods = new ObservableCollection<string>(new List<string> { "Azshari Salad", "Nightborne Delicacy Platter", "Seed-Battered Fish Plate", "The Hungry Magister", "Lemon Herb Filet" });
            Tier1Crucible = new ObservableCollection<int>(new List<int> { 0, 1, 2, 3 });
            Tier2Crucible = new ObservableCollection<string>(Tier2CrucibleMapping.Values.ToList().Union(new List<string>{ "None" }));
            Tier3Crucible = new ObservableCollection<string>(new List<string> { "None" });
            Tier1 = 0;
            Potion = Potions[0];
            Rune = Runes[0];
            Flask = Flasks[0];
            Food = Foods[0];
            Tier21 = "None";
            Tier22 = "None";
            Tier23 = "None";
            Tier31 = "None";
            Tier32 = "None";
            Tier33 = "None";
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

        public ObservableCollection<int> Tier1Crucible { get; set; }
        public ObservableCollection<String> Tier2Crucible { get; set; }
        public ObservableCollection<String> Tier3Crucible { get; set; }

        public Dictionary<String, String> Tier2CrucibleMapping = new Dictionary<string, string>
        {
            { "1771","Master of Shadows"},
            { "1774","Murderous Intent"},
            { "1778","Shadowbind"},
            { "1779","Chaotic Darkness"},
            { "1780","Torment the Weak"},
            { "1781","Dark Sorrows"},
            { "1770","Light Speed"},
            { "1775","Refractive Shell"},
            { "1777","Shocklight"},
            { "1782","Secure in the Light"},
            { "1783","Infusion of Light"},
            { "1784","Light's Embrace"}
        };
        public Dictionary<String, String> ShadowPriestCrucibleMapping = new Dictionary<string, string>
        {
            { "1573","Fiending Dark"},
            { "776","Creeping Shadows"},
            { "777","Touch of Darkness"},
            { "773","To the Pain"},
            { "767","Unleash the Shadows"},
            { "772","Mind Shattering"},
            { "774","Death's Embrace"},
            { "778","Void Corruption"},
            { "771","From the Shadows"},
            { "775","Thoughts of Insanity"},
        };
        public Dictionary<String, String> DisciplinePriestCrucibleMapping = new Dictionary<string, string>
        {
            { "1565","Lenience's Reward"},
            { "888","Confession"},
            { "894","Shield of Faith"},
            { "891","The Edge of Dark and Light"},
            { "895","Borrowed Time"},
            { "893","Doomsayer"},
            { "892","Burst of Light"},
            { "896","Darkest Shadows"},
            { "890","Pain is in Your Mind"},
            { "889","Vestments of Discipline"}
        };
        public Dictionary<String, String> HolyPriestCrucibleMapping = new Dictionary<string, string>
        {
            { "1569","Caress of the Naaru"},
            { "842","Say Your Prayers"},
            { "849","Holy Guidance"},
            { "847","Hallowed Ground"},
            { "838","Power of the Naaru"},
            { "848","Words of Healing"},
            { "846","Holy Hands"},
            { "844","Reverence"},
            { "843","Serenity Now"},
            { "841","Trust in the Light"}
        };
        public Dictionary<String, String> ArcaneMageCrucibleMapping = new Dictionary<string, string>
        {
            { "1529","Aegwynn's Intensity"},
            { "81","Ethereal Sensitivity"},
            { "75","Aegwynn's Wrath"},
            { "79","Aegwynn's Fury"},
            { "74","Blasting Rod"},
            { "77","Arcane Purification"},
            { "72","Torrential Barrage"},
            { "82","Aegwynn's Imperative"},
            { "84","Force Barrier"},
            { "73","Everywhere At Once"}
        };
        public Dictionary<String, String> FrostMageCrucibleMapping = new Dictionary<string, string>
        {
            { "1537","Obsidian Lance"},
            { "788","Frozen Veins"},
            { "786","Let It Go"},
            { "785","Ice Age"},
            { "789","Clarity of Thought"},
            { "784","Icy Caress"},
            { "790","The Storm Rages"},
            { "787","Orbital Strike"},
            { "792","Jouster"},
            { "791","Shield of Alodi"}
        };
        public Dictionary<String, String> FireMageCrucibleMapping = new Dictionary<string, string>
        {
            { "749","Fire at Will"},
            { "1533","Pre-Ignited"},
            { "751","Pyroclasmic Paranoia"},
            { "752","Burning Gaze"},
            { "755","Everburning Consumption"},
            { "750","Reignition Overdrive"},
            { "754","Great Balls of Fire"},
            { "753","Blue Flame Special"},
            { "756","Cauterizing Blink"},
            { "757","Molten Skin"}
        };
        public Dictionary<String, String> MarksmanHunterCrucibleMapping = new Dictionary<string, string>
        {
            { "1521","Unerring Arrows"},
            { "314","Quick Shot"},
            { "313","Windrunner's Guidance"},
            { "318","Precision"},
            { "319","Marked for Death"},
            { "312","Deadly Aim"},
            { "315","Called Shot"},
            { "317","Survival of the Fittest"},
            { "316","Healing Shell"},
            { "320","Gust of Wind"}
        };
        public Dictionary<String, String> BeastMasteryHunterCrucibleMapping = new Dictionary<string, string>
        {
            { "1517","Slithering Serpents"},
            { "872","Focus of the Titans"},
            { "874","Mimiron's Shell"},
            { "875","Jaws of Thunder"},
            { "869","Furious Swipes"},
            { "1095","Unleash the Beast"},
            { "870","Pack Leader"},
            { "868","Wilderness Expert"},
            { "873","Spitting Cobras"},
            { "871","Natural Reflexes"}
        };
        public Dictionary<String, String> SurvivalHunterCrucibleMapping = new Dictionary<string, string>
        {
            { "1525","Jaws of the Mongoose"},
            { "1076","Hellcarver"},
            { "1073","Raptor's Cry"},
            { "1075","Explosive Force"},
            { "1072","Lacerating Talons"},
            { "1074","Fluffy, Go"},
            { "1077","Bird of Prey"},
            { "1070","Sharpened Fang"},
            { "1071","My Beloved Monster"},
            { "1078","Hunter's Bounty"}
        };
        public Dictionary<String, String> FuryWarriorCrucibleMapping = new Dictionary<string, string>
        {
            { "995","Unrivaled Strength"},
            { "1617","Pulse of Battle"},
            { "996","Raging Berserker"},
            { "990","Wrath and Fury"},
            { "991","Unstoppable"},
            { "988","Deathdealer"},
            { "989","Wild Slashes"},
            { "992","Uncontrolled Rage"},
            { "994","Bloodcraze"},
            { "993","Battle Scars"}
        };
        public Dictionary<String, String> ArmsWarriorCrucibleMapping = new Dictionary<string, string>
        {
            { "1613","Storm of Swords"},
            { "1150","Exploit the Weakness"},
            { "1149","Precise Strikes"},
            { "1146","Many Will Fall"},
            { "1145","Crushing Blows"},
            { "1143","Unending Rage"},
            { "1147","Deathblow"},
            { "1144","One Against Many"},
            { "1148","Tactical Advance"},
            { "1151","Touch of Zakajz"}
        };
        public Dictionary<String, String> ProtectionWarriorCrucibleMapping = new Dictionary<string, string>
        {
            { "1621","Bastion of the Aspects"},
            { "95","Vrykul Shield Training"},
            { "100","Dragon Skin"},
            { "105","Toughness"},
            { "96","Rage of the Fallen"},
            { "106","Thunder Crash"},
            { "102","Shatter the Bones"},
            { "101","Intolerance"},
            { "98","Will to Survive"},
            { "99","Leaping Giants"}
        };
        public Dictionary<String, String> FeralDruidCrucibleMapping = new Dictionary<string, string>
        {
            { "1166","Tear the Flesh"},
            { "1161","Razor Fangs"},
            { "1164","Ashamane's Energy"},
            { "1167","Feral Instinct"},
            { "1162","Feral Power"},
            { "1505","Thrashing Claws"},
            { "1163","Powerful Bite"},
            { "1168","Sharpened Claws"},
            { "1165","Attuned to Nature"},
            { "1160","Honed Instincts"}
        };
        public Dictionary<String, String> BalanceDruidCrucibleMapping = new Dictionary<string, string>
        {
            { "1501","Light of the Evening Star"},
            { "1041","Sunfire Burns"},
            { "1037","Twilight Glow"},
            { "1039","Scythe of the Stars"},
            { "1038","Solar Stabbing"},
            { "1040","Falling Star"},
            { "1281","Dark Side of the Moon"},
            { "1042","Empowerment"},
            { "1035","Touch of the Moon"},
            { "1034","Bladed Feathers"}
        };
        public Dictionary<String, String> RestorationDruidCrucibleMapping = new Dictionary<string, string>
        {
            { "137","Persistence"},
            { "1513","Eternal Restoration"},
            { "140","Grovewalker"},
            { "130","Knowledge of the Ancients"},
            { "132","Essence of Nordrassil"},
            { "133","Infusion of Nature"},
            { "135","Natural Mending"},
            { "131","Seeds of the World Tree"},
            { "136","Blessing of the World Tree"},
            { "134","Armor of the Ancients"}
        };
        public Dictionary<String, String> GuardianDruidCrucibleMapping = new Dictionary<string, string>
        {
            { "1509","Scintillating Moonlight"},
            { "952","Jagged Claws"},
            { "956","Vicious Bites"},
            { "949","Ursoc's Endurance"},
            { "950","Wildflesh"},
            { "953","Bestial Fortitude"},
            { "951","Perpetual Spring"},
            { "948","Reinforced Fur"},
            { "955","Mauler"},
            { "954","Sharpened Instincts"}
        };
        public Dictionary<String, String> SubtletyRogueCrucibleMapping = new Dictionary<string, string>
        {
            { "1585","Weak Point"},
            { "854","Gutripper"},
            { "858","Energetic Stabbing"},
            { "857","Soul Shadows"},
            { "855","Precision Strike"},
            { "853","Demon's Kiss"},
            { "852","The Quiet Knife"},
            { "856","Fortune's Bite"},
            { "859","Catlike Reflexes"},
            { "860","Ghost Armor"}
        };
        public Dictionary<String, String> OutlawRogueCrucibleMapping = new Dictionary<string, string>
        {
            { "1581","Sabermetrics"},
            { "1061","Fate's Thirst"},
            { "1064","Fatebringer"},
            { "1065","Fortune Strikes"},
            { "1062","Fortune's Strike"},
            { "1060","Blade Dancer"},
            { "1063","Gunslinger"},
            { "1059","Black Powder"},
            { "1067","Fortune's Boon"},
            { "1066","Ghostly Shell"}
        };
        public Dictionary<String, String> AssassinationRogueCrucibleMapping = new Dictionary<string, string>
        {
            { "1577","Strangler"},
            { "330","Master Assassin"},
            { "328","Gushing Wound"},
            { "323","Toxic Blades"},
            { "327","Balanced Blades"},
            { "324","Serrated Edge"},
            { "331","Poison Knives"},
            { "325","Master Alchemist"},
            { "329","Shadow Walker"},
            { "326","Fade into Shadows"}
        };
        public Dictionary<String, String> HavocDemonHunterCrucibleMapping = new Dictionary<string, string>
        {
            { "1493","Wide Eyes"},
            { "1001","Critical Chaos"},
            { "1004","Demon Rage"},
            { "1002","Chaos Vision"},
            { "1003","Sharpened Glaives"},
            { "1000","Contained Fury"},
            { "1006","Unleashed Demons"},
            { "1005","Illidari Knowledge"},
            { "1008","Overwhelming Power"},
            { "1007","Deceiver's Fury"}
        };
        public Dictionary<String, String> VengeanceDemonHunterCrucibleMapping = new Dictionary<string, string>
        {
            { "1497","Lingering Ordeal"},
            { "1231","Fiery Demise"},
            { "1100","Aura of Pain"},
            { "1230","Embrace the Pain"},
            { "1101","Honed Warblades"},
            { "1099","Aldrachi Design"},
            { "1229","Inferal Force"},
            { "1233","Devour Souls"},
            { "1232","Will of the Illidari"},
            { "1234","Shatter the Souls"}
        };
        public Dictionary<String, String> BloodDeathKnightCrucibleMapping = new Dictionary<string, string>
        {
            { "1481","Carrion Feast"},
            { "277","Vampiric Fangs"},
            { "275","Veinrender"},
            { "279","Dance of Darkness"},
            { "280","Bonebreaker"},
            { "278","All-Consuming Rot"},
            { "273","Meat Shield"},
            { "272","Iron Heart"},
            { "274","Grim Perserverance"},
            { "276","Coagulopathy"}
        };
        public Dictionary<String, String> UnholyDeathKnightCrucibleMapping = new Dictionary<string, string>
        {
            { "158","Plaguebearer"},
            { "1489","Lash of Shadows"},
            { "266","The Darkest Crusade"},
            { "156","Deadliest Coil"},
            { "1119","Eternal Agony"},
            { "157","Rotten Touch"},
            { "265","Scourge the Unbeliever"},
            { "267","Unholy Endurance"},
            { "262","Deadly Durability"},
            { "264","Runic Tattoos"}
        };
        public Dictionary<String, String> FrostDeathKnightCrucibleMapping = new Dictionary<string, string>
        {
            { "1485","Runefrost"},
            { "109","Blast Radius"},
            { "110","Ambidexterity"},
            { "108","Cold as Ice"},
            { "113","Nothing but the Boots"},
            { "111","Over-Powered"},
            { "114","Bad to the Bone"},
            { "117","Dead of Winter"},
            { "115","Ice in Your Veins"},
            { "1090","Frozen Skin"}
        };
        public Dictionary<String, String> EnhancementShamanCrucibleMapping = new Dictionary<string, string>
        {
            { "1593","Crashing Hammer"},
            { "910","Wind Strikes"},
            { "906","Forged in Lava"},
            { "905","Weapons of the Elements"},
            { "908","Wind Surge"},
            { "913","Gathering of the Maelstrom"},
            { "912","Gathering Storms"},
            { "911","Spiritual Healing"},
            { "909","Elemental Healing"},
            { "907","Spirit of the Maelstrom"}
        };
        public Dictionary<String, String> ElementalShamanCrucibleMapping = new Dictionary<string, string>
        {
            { "1589","Elemental Destabilization"},
            { "300","Lava Imbued"},
            { "301","Firestorm"},
            { "303","Molten Blast"},
            { "298","Call the Thunder"},
            { "306","Earthen Attunement"},
            { "304","Electric Discharge"},
            { "299","The Ground Trembles"},
            { "302","Protection of the Elements"},
            { "305","Shamanistic Healing"}
        };
        public Dictionary<String, String> RestorationShamanCrucibleMapping = new Dictionary<string, string>
        {
            { "1597","Tidewalker"},
            { "1108","Queen Ascendant"},
            { "1107","Empowered Droplets"},
            { "1109","Floodwaters"},
            { "1104","Buffeting Waves"},
            { "1106","Wavecrash"},
            { "1105","Pull of the Sea"},
            { "1103","Tidal Chains"},
            { "1111","Caress of the Tidemother"},
            { "1110","Ghost in the Mist"}
        };
        public Dictionary<String, String> HolyPaladinCrucibleMapping = new Dictionary<string, string>
        {
            { "972","Shock Treatment"},
            { "977","Deliver the Light"},
            { "1186","Second Sunrise"},
            { "976","Expel the Darkness"},
            { "1553","Tyr's Munificence"},
            { "971","Justice Through Sacrifice"},
            { "970","Focused Healing"},
            { "975","Blessings of the Silver Hnad"},
            { "969","Share the Burden"},
            { "974","Knight of the Silver Hand"}
        };
        public Dictionary<String, String> ProtectionPaladinCrucibleMapping = new Dictionary<string, string>
        {
            { "1557","Holy Aegis"},
            { "1126","Consecration in Flame"},
            { "1121","Hammer Time"},
            { "1122","Stern Judgment"},
            { "1124","Righteous Crusade"},
            { "1123","Unflinching Defense"},
            { "1125","Scatter the Shadows"},
            { "1129","Faith's Armor"},
            { "1128","Resolve of Truth"},
            { "1127","Bastion of Truth"}
        };
        public Dictionary<String, String> RetributionPaladinCrucibleMapping = new Dictionary<string, string>
        {
            { "1561","Righteous Verdict"},
            { "53","Wrath of the Ashbringer"},
            { "50","Deliver the Justice"},
            { "51","Might of the Templar"},
            { "43","Righteous Blade"},
            { "44","Embrace the Light"},
            { "41","Highlord's Justice"},
            { "52","Protector of the Ashen Blade"},
            { "42","Sharpened Edge"},
            { "47","Deflection"}
        };
        public Dictionary<String, String> AfflictionWarlockCrucibleMapping = new Dictionary<string, string>
        {
            { "1601","Winnowing"},
            { "920","Perdition"},
            { "921","Shadowy Incantations"},
            { "918","Inherently Unstable"},
            { "915","Inimitable Agony"},
            { "916","Hideous Corruption"},
            { "917","Drained to a Husk"},
            { "919","Seeds of Doom"},
            { "923","Long Dark Night of the Soul"},
            { "922","Shadows of the Flesh"}
        };
        public Dictionary<String, String> DemonologyWarlockCrucibleMapping = new Dictionary<string, string>
        {
            { "1605","Left Hand of Darkness"},
            { "1176","Infernal Furnace"},
            { "1173","The Doom of Azeroth"},
            { "1171","Summoner's Prowess"},
            { "1174","Sharpened Dreadfangs"},
            { "1177","Maw of Shadows"},
            { "1175","Dirty Hands"},
            { "1172","Legionwrath"},
            { "1179","Firm Resolve"},
            { "1178","Open Link"}
        };
        public Dictionary<String, String> DestructionWarlockCrucibleMapping = new Dictionary<string, string>
        {
            { "1609","Flames of Sargeras"},
            { "809","Burning Hunger"},
            { "807","Residual Flames"},
            { "805","Chaotic Instability"},
            { "806","Fire and the Flames"},
            { "804","Master of Disaster"},
            { "808","Soulsnatcher"},
            { "810","Fire From the Sky"},
            { "812","Eternal Struggle"},
            { "811","Devourer of Life"}
        };
        public Dictionary<String, String> BrewmasterMonkCrucibleMapping = new Dictionary<string, string>
        {
            { "1541","Draught of Darkness"},
            { "1286","Face Palm"},
            { "1279","Hot Blooded"},
            { "1285","Potent Kick"},
            { "1280","Overflow"},
            { "1282","Staggering Around"},
            { "1278","Obsidian Fists"},
            { "1284","Gifted Student"},
            { "1283","Healthy Appetite"},
            { "1036","Dark Side of the Moon"}
        };
        public Dictionary<String, String> MistweaverMonkCrucibleMapping = new Dictionary<string, string>
        {
            { "1545","Tendrils of Revival"},
            { "944","Infusion of Life"},
            { "942","Extended Healing"},
            { "946","Essence of the Mists"},
            { "940","Way of the Mistweaver"},
            { "938","Coalescing Mists"},
            { "941","Protection of Shaohao"},
            { "943","Soothing Remedies"},
            { "939","Shroud of Mist"},
            { "945","Spirit Tether"}
        };
        public Dictionary<String, String> WindwalkerMonkCrucibleMapping = new Dictionary<string, string>
        {
            { "1549","Split Personality"},
            { "825","Fists of the Wind"},
            { "820","Rising Winds"},
            { "800","Inner Peace"},
            { "821","Tiger Claws"},
            { "1094","Strength of Xuen"},
            { "824","Power of a Thousand Cranes"},
            { "822","Death Art"},
            { "801","Light on Your Feet"},
            { "829","Healing Winds"}
        };


        public int _tier1;

        public String _tier21;
        public String _tier22;
        public String _tier23;

        public String _tier31;
        public String _tier32;
        public String _tier33;

        public int Tier1
        {
            get { return _tier1; }
            set
            {
                if (value != _tier1)
                {
                    _tier1 = value;
                    OnPropertyChanged("Tier1");
                }
            }
        }

        public String Tier21
        {
            get { return _tier21; }
            set
            {
                if (value != _tier21)
                {
                    _tier21 = value;
                    OnPropertyChanged("Tier21");
                }
            }
        }
        public String Tier22
        {
            get { return _tier22; }
            set
            {
                if (value != _tier22)
                {
                    _tier22 = value;
                    OnPropertyChanged("Tier22");
                }
            }
        }
        public String Tier23
        {
            get { return _tier23; }
            set
            {
                if (value != _tier23)
                {
                    _tier23 = value;
                    OnPropertyChanged("Tier23");
                }
            }
        }

        public String Tier31
        {
            get { return _tier31; }
            set
            {
                if (value != _tier31)
                {
                    _tier31 = value;
                    OnPropertyChanged("Tier31");
                }
            }
        }
        public String Tier32
        {
            get { return _tier32; }
            set
            {
                if (value != _tier32)
                {
                    _tier32 = value;
                    OnPropertyChanged("Tier32");
                }
            }
        }
        public String Tier33
        {
            get { return _tier33; }
            set
            {
                if (value != _tier33)
                {
                    _tier33 = value;
                    OnPropertyChanged("Tier33");
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

        public void CrucibleLoader(Dictionary<String,String> classSpecDictionary, String crucible)
        {
            Tier3Crucible.Clear();
            Tier3Crucible.Add("None");
            classSpecDictionary.Values.ToList().ForEach((trait) =>
            {
                Tier3Crucible.Add(trait);
            });
            if(crucible == null || crucible.Length == 0)
            {
                Tier1 = 0;
                Tier21 = "None";
                Tier22 = "None";
                Tier23 = "None";
                Tier31 = "None";
                Tier32 = "None";
                Tier33 = "None";
            }
            if (crucible.Contains("/"))
            {
                var relics = crucible.Split('/');
                var slot = 1;
                foreach (var relic in relics)
                {
                    if (slot == 1)
                    {
                        foreach (var trait in relic.Split(':'))
                        {
                            if (trait.Equals("1739"))
                            {
                                Tier1++;
                            }
                            else if (Tier2CrucibleMapping.Keys.Contains(trait))
                            {
                                Tier21 = Tier2CrucibleMapping[trait];
                            }
                            else if (classSpecDictionary.Keys.Contains(trait))
                            {
                                Tier31 = classSpecDictionary[trait];
                            }
                        }
                    }
                    else if (slot == 2)
                    {
                        foreach (var trait in relic.Split(':'))
                        {
                            if (trait.Equals("1739"))
                            {
                                Tier1++;
                            }
                            else if (Tier2CrucibleMapping.Keys.Contains(trait))
                            {
                                Tier22 = Tier2CrucibleMapping[trait];
                            }
                            else if (classSpecDictionary.Keys.Contains(trait))
                            {
                                Tier32 = classSpecDictionary[trait];
                            }
                        }
                    }
                    else if (slot == 3)
                    {
                        foreach (var trait in relic.Split(':'))
                        {
                            if (trait.Equals("1739"))
                            {
                                Tier1++;
                            }
                            else if (Tier2CrucibleMapping.Keys.Contains(trait))
                            {
                                Tier23 = Tier2CrucibleMapping[trait];
                            }
                            else if (classSpecDictionary.Keys.Contains(trait))
                            {
                                Tier33 = classSpecDictionary[trait];
                            }
                        }
                    }
                    slot++;
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


        public string CrucibleUnloader(Dictionary<string,string> classSpecMapping)
        {
            var relic1 = "";
            var relic2 = "";
            var relic3 = "";
            if(Tier1 >= 1)
            {
                relic1 = "1739";
            }
            if(Tier1 >= 2)
            {
                relic2 = "1739";
            }
            if(Tier1 >= 3)
            {
                relic3 = "1739";
            }
            var key = GetKeyForValue(Tier21, Tier2CrucibleMapping);
            if(!key.Equals(""))
            {
                relic1 += ":" + key;
            }
            key = GetKeyForValue(Tier22, Tier2CrucibleMapping);
            if (!key.Equals(""))
            {
                relic2 += ":" + key;
            }
            key = GetKeyForValue(Tier23, Tier2CrucibleMapping);
            if (!key.Equals(""))
            {
                relic3 += ":" + key;
            }
            key = GetKeyForValue(Tier31, classSpecMapping);
            if (!key.Equals(""))
            {
                relic1 += ":" + key;
            }
            key = GetKeyForValue(Tier32, classSpecMapping);
            if (!key.Equals(""))
            {
                relic2 += ":" + key;
            }
            key = GetKeyForValue(Tier33, classSpecMapping);
            if (!key.Equals(""))
            {
                relic3 += ":" + key;
            }
            var retVal = "";
            if(relic1 != null && relic1.Length>0)
            {
                retVal = relic1;
            }
            if (relic2 != null && relic2.Length > 0)
            {
                if (retVal != "")
                {
                    retVal += "/" + relic2;
                }
                else
                {
                    retVal = relic2;
                }
            }
            if (relic3 != null && relic3.Length > 0)
            {
                if (retVal != "")
                {
                    retVal += "/" + relic3;
                }
                else
                {
                    retVal = relic3;
                }
            }
            return retVal;
        }

        private string _profileText;

        public string ConstructFinalProfile()
        {
            var nameClassRegex = new Regex("(\\w+)=\\\"(\\w+)\\\"");
            var specRegex = new Regex("spec=(\\w+)");
            var crucibleRegex = new Regex("crucible=([^\r\n]+)");
            var artifactRegex = new Regex("artifact=([^\r\n]+)");
            var ilvlRegex = new Regex("gear_ilvl=(\\d+.?\\d*)");
            var tierRegex = new Regex("set_bonus=tier(\\d+)_(\\d)pc=1");
            var potionRegex = new Regex("potion=(\\w+)");
            var flaskRegex = new Regex("flask=(\\w+)");
            var foodRegex = new Regex("food=(\\w+)");
            var augmentRegex = new Regex("augmentation=(\\w+)");

            // Replace Crucible,Ilvl if flagged, tier if flagged, potion,flask,food,augment
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
            if(!_profileText.Contains("crucible="))
            {
                var match = artifactRegex.Match(_profileText).Groups[0].Value;
                _profileText = _profileText.Replace(match, match + "\r\ncrucible=0\r\n");
            }
            if (Class.Equals("Priest"))
            {
                if (Spec.Equals("Shadow"))
                {
                    _profileText = crucibleRegex.Replace(_profileText, "crucible=" + CrucibleUnloader(ShadowPriestCrucibleMapping));
                }
                else if (Spec.Equals("Discipline"))
                {
                    _profileText = crucibleRegex.Replace(_profileText, "crucible=" + CrucibleUnloader(DisciplinePriestCrucibleMapping));
                }
                else if (Spec.Equals("Holy"))
                {
                    _profileText = crucibleRegex.Replace(_profileText, "crucible=" + CrucibleUnloader(HolyPriestCrucibleMapping));
                }
            }
            else if (Class.Equals("Mage"))
            {
                if (Spec.Equals("Arcane"))
                {
                    _profileText = crucibleRegex.Replace(_profileText, "crucible=" + CrucibleUnloader(ArcaneMageCrucibleMapping));
                }
                else if (Spec.Equals("Frost"))
                {
                    _profileText = crucibleRegex.Replace(_profileText, "crucible=" + CrucibleUnloader(FrostMageCrucibleMapping));
                }
                else if (Spec.Equals("Fire"))
                {
                    _profileText = crucibleRegex.Replace(_profileText, "crucible=" + CrucibleUnloader(FireMageCrucibleMapping));
                }
            }
            else if (Class.Equals("Warrior"))
            {
                if (Spec.Equals("Protection"))
                {
                    _profileText = crucibleRegex.Replace(_profileText, "crucible=" + CrucibleUnloader(ProtectionWarriorCrucibleMapping));
                }
                else if (Spec.Equals("Arms"))
                {
                    _profileText = crucibleRegex.Replace(_profileText, "crucible=" + CrucibleUnloader(ArmsWarriorCrucibleMapping));
                }
                else if (Spec.Equals("Fury"))
                {
                    _profileText = crucibleRegex.Replace(_profileText, "crucible=" + CrucibleUnloader(FuryWarriorCrucibleMapping));
                }
            }
            else if (Class.Equals("Hunter"))
            {
                if (Spec.Equals("Beast Mastery"))
                {
                    _profileText = crucibleRegex.Replace(_profileText, "crucible=" + CrucibleUnloader(BeastMasteryHunterCrucibleMapping));
                }
                else if (Spec.Equals("Survival"))
                {
                    _profileText = crucibleRegex.Replace(_profileText, "crucible=" + CrucibleUnloader(SurvivalHunterCrucibleMapping));
                }
                else if (Spec.Equals("Marksman"))
                {
                    _profileText = crucibleRegex.Replace(_profileText, "crucible=" + CrucibleUnloader(MarksmanHunterCrucibleMapping));
                }
            }
            else if (Class.Equals("Demon Hunter"))
            {
                if (Spec.Equals("Havoc"))
                {
                    _profileText = crucibleRegex.Replace(_profileText, "crucible=" + CrucibleUnloader(HavocDemonHunterCrucibleMapping));
                }
                else if (Spec.Equals("Vengeance"))
                {
                    _profileText = crucibleRegex.Replace(_profileText, "crucible=" + CrucibleUnloader(VengeanceDemonHunterCrucibleMapping));
                }
            }
            else if (Class.Equals("Monk"))
            {
                if (Spec.Equals("Windwalker"))
                {
                    _profileText = crucibleRegex.Replace(_profileText, "crucible=" + CrucibleUnloader(WindwalkerMonkCrucibleMapping));
                }
                else if (Spec.Equals("Mistweaver"))
                {
                    _profileText = crucibleRegex.Replace(_profileText, "crucible=" + CrucibleUnloader(MistweaverMonkCrucibleMapping));
                }
                else if (Spec.Equals("Brewmaster"))
                {
                    _profileText = crucibleRegex.Replace(_profileText, "crucible=" + CrucibleUnloader(BrewmasterMonkCrucibleMapping));
                }
            }
            else if (Class.Equals("Warlock"))
            {
                if (Spec.Equals("Affliction"))
                {
                    _profileText = crucibleRegex.Replace(_profileText, "crucible=" + CrucibleUnloader(AfflictionWarlockCrucibleMapping));
                }
                else if (Spec.Equals("Demonology"))
                {
                    _profileText = crucibleRegex.Replace(_profileText, "crucible=" + CrucibleUnloader(DemonologyWarlockCrucibleMapping));
                }
                else if (Spec.Equals("Destruction"))
                {
                    _profileText = crucibleRegex.Replace(_profileText, "crucible=" + CrucibleUnloader(DestructionWarlockCrucibleMapping));
                }
            }
            else if (Class.Equals("Paladin"))
            {
                if (Spec.Equals("Holy"))
                {
                    _profileText = crucibleRegex.Replace(_profileText, "crucible=" + CrucibleUnloader(HolyPaladinCrucibleMapping));
                }
                else if (Spec.Equals("Retribution"))
                {
                    _profileText = crucibleRegex.Replace(_profileText, "crucible=" + CrucibleUnloader(RetributionPaladinCrucibleMapping));
                }
                else if (Spec.Equals("Protection"))
                {
                    _profileText = crucibleRegex.Replace(_profileText, "crucible=" + CrucibleUnloader(ProtectionPaladinCrucibleMapping));
                }
            }
            else if (Class.Equals("Death Knight"))
            {
                if (Spec.Equals("Unholy"))
                {
                    _profileText = crucibleRegex.Replace(_profileText, "crucible=" + CrucibleUnloader(UnholyDeathKnightCrucibleMapping));
                }
                else if (Spec.Equals("Frost"))
                {
                    _profileText = crucibleRegex.Replace(_profileText, "crucible=" + CrucibleUnloader(FrostDeathKnightCrucibleMapping));
                }
                else if (Spec.Equals("Blood"))
                {
                    _profileText = crucibleRegex.Replace(_profileText, "crucible=" + CrucibleUnloader(BloodDeathKnightCrucibleMapping));
                }
            }
            else if (Class.Equals("Shaman"))
            {
                if (Spec.Equals("Enhancement"))
                {
                    _profileText = crucibleRegex.Replace(_profileText, "crucible=" + CrucibleUnloader(EnhancementShamanCrucibleMapping));
                }
                else if (Spec.Equals("Elemental"))
                {
                    _profileText = crucibleRegex.Replace(_profileText, "crucible=" + CrucibleUnloader(ElementalShamanCrucibleMapping));
                }
                else if (Spec.Equals("Restoration"))
                {
                    _profileText = crucibleRegex.Replace(_profileText, "crucible=" + CrucibleUnloader(RestorationShamanCrucibleMapping));
                }
            }
            else if (Class.Equals("Druid"))
            {
                if (Spec.Equals("Restoration"))
                {
                    _profileText = crucibleRegex.Replace(_profileText, "crucible=" + CrucibleUnloader(RestorationDruidCrucibleMapping));
                }
                else if (Spec.Equals("Feral"))
                {
                    _profileText = crucibleRegex.Replace(_profileText, "crucible=" + CrucibleUnloader(FeralDruidCrucibleMapping));
                }
                else if (Spec.Equals("Balance"))
                {
                    _profileText = crucibleRegex.Replace(_profileText, "crucible=" + CrucibleUnloader(BalanceDruidCrucibleMapping));
                }
                else if (Spec.Equals("Guardian"))
                {
                    _profileText = crucibleRegex.Replace(_profileText, "crucible=" + CrucibleUnloader(GuardianDruidCrucibleMapping));
                }
            }
            else if (Class.Equals("Rogue"))
            {
                if (Spec.Equals("Subtlety"))
                {
                    _profileText = crucibleRegex.Replace(_profileText, "crucible=" + CrucibleUnloader(SubtletyRogueCrucibleMapping));
                }
                else if (Spec.Equals("Assassination"))
                {
                    _profileText = crucibleRegex.Replace(_profileText, "crucible=" + CrucibleUnloader(AssassinationRogueCrucibleMapping));
                }
                else if (Spec.Equals("Outlaw"))
                {
                    _profileText = crucibleRegex.Replace(_profileText, "crucible=" + CrucibleUnloader(OutlawRogueCrucibleMapping));
                }
            }
            if(_profileText.Contains("crucible=0\r\n") || _profileText.Contains("crucible=\r\n")) {
                _profileText = _profileText.Replace("crucible=0", "").Replace("crucible=", "");
            }
            return _profileText;
        }

        public void LoadProfilePath(String path)
        {
            _profileText = File.ReadAllText(path);
            var profileText = _profileText;
            //Parse and update values.
            var nameClassRegex = new Regex("(\\w+)=\\\"(\\w+)\\\"");
            var specRegex = new Regex("spec=(\\w+)");
            var crucibleRegex = new Regex("crucible=([^\r\n]+)");
            var ilvlRegex = new Regex("gear_ilvl=(\\d+.?\\d*)");
            var tierRegex = new Regex("set_bonus=tier(\\d+)_(\\d)pc=1");
            var potionRegex = new Regex("potion=(\\w+)");
            var flaskRegex = new Regex("flask=(\\w+)");
            var foodRegex = new Regex("food=(\\w+)");
            var augmentRegex = new Regex("augmentation=(\\w+)");

            var nameClassMatch = nameClassRegex.Match(profileText);
            Name = nameClassMatch.Groups[2].Value;
            Class = nameClassMatch.Groups[1].Value.UppercaseWords().Replace("Deathk","Death K").Replace("Demonh","Demon H").UppercaseWords();
            Spec = specRegex.Match(profileText).Groups[1].Value.UppercaseWords().Replace("Beastm", "Beast M");
            var crucible = crucibleRegex.Match(profileText).Groups[1].Value;
            ILvl = float.Parse(ilvlRegex.Match(profileText).Groups[1].Value);

            Potion = PotionNameMapping[potionRegex.Match(profileText).Groups[1].Value];
            Flask = FlaskNameMapping[flaskRegex.Match(profileText).Groups[1].Value];
            Food = FoodNameMapping[foodRegex.Match(profileText).Groups[1].Value];
            Rune = AugmentNameMapping[augmentRegex.Match(profileText).Groups[1].Value];

            var tierMatches = tierRegex.Matches(profileText);
            for(int i = 0; i < tierMatches.Count; i++)
            {
                var match = tierMatches[i];
                if(match.Groups[1].Value.Equals("21"))
                {
                    if(match.Groups[2].Value.Equals("2"))
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

            Tier1 = 0;
            if (Class.Equals("Priest"))
            {
                if (Spec.Equals("Shadow"))
                {
                    CrucibleLoader(ShadowPriestCrucibleMapping, crucible);
                }
                else if (Spec.Equals("Discipline"))
                {
                    CrucibleLoader(DisciplinePriestCrucibleMapping, crucible);
                }
                else if (Spec.Equals("Holy"))
                {
                    CrucibleLoader(HolyPriestCrucibleMapping, crucible);
                }
            }
            else if (Class.Equals("Mage"))
            {
                if (Spec.Equals("Arcane"))
                {
                    CrucibleLoader(ArcaneMageCrucibleMapping, crucible);
                }
                else if (Spec.Equals("Frost"))
                {
                    CrucibleLoader(FrostMageCrucibleMapping, crucible);
                }
                else if (Spec.Equals("Fire"))
                {
                    CrucibleLoader(FireMageCrucibleMapping, crucible);
                }
            }
            else if (Class.Equals("Warrior"))
            {
                if (Spec.Equals("Protection"))
                {
                    CrucibleLoader(ProtectionWarriorCrucibleMapping, crucible);
                }
                else if (Spec.Equals("Arms"))
                {
                    CrucibleLoader(ArmsWarriorCrucibleMapping, crucible);
                }
                else if (Spec.Equals("Fury"))
                {
                    CrucibleLoader(FuryWarriorCrucibleMapping, crucible);
                }
            }
            else if (Class.Equals("Hunter"))
            {
                if (Spec.Equals("Beast Mastery"))
                {
                    CrucibleLoader(BeastMasteryHunterCrucibleMapping, crucible);
                }
                else if (Spec.Equals("Survival"))
                {
                    CrucibleLoader(SurvivalHunterCrucibleMapping, crucible);
                }
                else if (Spec.Equals("Marksman"))
                {
                    CrucibleLoader(MarksmanHunterCrucibleMapping, crucible);
                }
            }
            else if (Class.Equals("Demon Hunter"))
            {
                if (Spec.Equals("Havoc"))
                {
                    CrucibleLoader(HavocDemonHunterCrucibleMapping, crucible);
                }
                else if (Spec.Equals("Vengeance"))
                {
                    CrucibleLoader(VengeanceDemonHunterCrucibleMapping, crucible);
                }
            }
            else if (Class.Equals("Monk"))
            {
                if (Spec.Equals("Windwalker"))
                {
                    CrucibleLoader(WindwalkerMonkCrucibleMapping, crucible);
                }
                else if (Spec.Equals("Mistweaver"))
                {
                    CrucibleLoader(MistweaverMonkCrucibleMapping, crucible);
                }
                else if (Spec.Equals("Brewmaster"))
                {
                    CrucibleLoader(BrewmasterMonkCrucibleMapping, crucible);
                }
            }
            else if (Class.Equals("Warlock"))
            {
                if (Spec.Equals("Affliction"))
                {
                    CrucibleLoader(AfflictionWarlockCrucibleMapping, crucible);
                }
                else if (Spec.Equals("Demonology"))
                {
                    CrucibleLoader(DemonologyWarlockCrucibleMapping, crucible);
                }
                else if (Spec.Equals("Destruction"))
                {
                    CrucibleLoader(DestructionWarlockCrucibleMapping, crucible);
                }
            }
            else if (Class.Equals("Paladin"))
            {
                if (Spec.Equals("Holy"))
                {
                    CrucibleLoader(HolyPaladinCrucibleMapping, crucible);
                }
                else if (Spec.Equals("Retribution"))
                {
                    CrucibleLoader(RetributionPaladinCrucibleMapping, crucible);
                }
                else if (Spec.Equals("Protection"))
                {
                    CrucibleLoader(ProtectionPaladinCrucibleMapping, crucible);
                }
            }
            else if (Class.Equals("Death Knight"))
            {
                if (Spec.Equals("Unholy"))
                {
                    CrucibleLoader(UnholyDeathKnightCrucibleMapping, crucible);
                }
                else if (Spec.Equals("Frost"))
                {
                    CrucibleLoader(FrostDeathKnightCrucibleMapping, crucible);
                }
                else if (Spec.Equals("Blood"))
                {
                    CrucibleLoader(BloodDeathKnightCrucibleMapping, crucible);
                }
            }
            else if (Class.Equals("Shaman"))
            {
                if (Spec.Equals("Enhancement"))
                {
                    CrucibleLoader(EnhancementShamanCrucibleMapping, crucible);
                }
                else if (Spec.Equals("Elemental"))
                {
                    CrucibleLoader(ElementalShamanCrucibleMapping, crucible);
                }
                else if (Spec.Equals("Restoration"))
                {
                    CrucibleLoader(RestorationShamanCrucibleMapping, crucible);
                }
            }
            else if (Class.Equals("Druid"))
            {
                if (Spec.Equals("Restoration"))
                {
                    CrucibleLoader(RestorationDruidCrucibleMapping, crucible);
                }
                else if (Spec.Equals("Feral"))
                {
                    CrucibleLoader(FeralDruidCrucibleMapping, crucible);
                }
                else if (Spec.Equals("Balance"))
                {
                    CrucibleLoader(BalanceDruidCrucibleMapping, crucible);
                }
                else if (Spec.Equals("Guardian"))
                {
                    CrucibleLoader(GuardianDruidCrucibleMapping, crucible);
                }
            }
            else if (Class.Equals("Rogue"))
            {
                if (Spec.Equals("Subtlety"))
                {
                    CrucibleLoader(SubtletyRogueCrucibleMapping, crucible);
                }
                else if (Spec.Equals("Assassination"))
                {
                    CrucibleLoader(AssassinationRogueCrucibleMapping, crucible);
                }
                else if (Spec.Equals("Outlaw"))
                {
                    CrucibleLoader(OutlawRogueCrucibleMapping, crucible);
                }
            }
        }
    }
}
