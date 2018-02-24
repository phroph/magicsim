using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magicsim
{
    public class SimNode
    {
        public List<Player> players;
    }

    public class Player
    {
        public string name;
        public string specialization;
        public ScaleFactors scale_factors;
        public CollectedData collected_data;
        public Gear gear;

        private void AddGearToResults(GearResults results, GearItem item, bool strength, bool intellect, bool agility)
        {
            if (item != null)
            {
                if (strength)
                {
                    results.strength += item.strength;
                    results.strength += item.stragiint;
                }
                else if (intellect)
                {
                    results.intellect += item.intellect;
                    results.intellect += item.stragiint;
                }
                else if (agility)
                {
                    results.agility += item.agility;
                    results.agility += item.stragiint;
                }
                results.crit += item.crit_rating;
                results.haste += item.haste_rating;
                results.mastery += item.mastery_rating;
                results.vers += item.versatility_rating;
            }
        }
        public GearResults GetStats()
        {
            bool strength = false, intellect = false, agility = false;
            var results = new GearResults();

            if (collected_data.buffed_stats.attribute.strength > collected_data.buffed_stats.attribute.intellect && collected_data.buffed_stats.attribute.strength > collected_data.buffed_stats.attribute.agility)
            {
                strength = true;
            }
            if (collected_data.buffed_stats.attribute.agility > collected_data.buffed_stats.attribute.intellect && collected_data.buffed_stats.attribute.agility > collected_data.buffed_stats.attribute.strength)
            {
                agility = true;
            }
            if (collected_data.buffed_stats.attribute.intellect > collected_data.buffed_stats.attribute.strength && collected_data.buffed_stats.attribute.intellect > collected_data.buffed_stats.attribute.agility)
            {
                intellect = true;
            }

            if (gear != null)
            {
                AddGearToResults(results, gear.head, strength, intellect, agility);
                AddGearToResults(results, gear.neck, strength, intellect, agility);
                AddGearToResults(results, gear.shoulders, strength, intellect, agility);
                AddGearToResults(results, gear.chest, strength, intellect, agility);
                AddGearToResults(results, gear.waist, strength, intellect, agility);
                AddGearToResults(results, gear.legs, strength, intellect, agility);
                AddGearToResults(results, gear.feet, strength, intellect, agility);
                AddGearToResults(results, gear.wrists, strength, intellect, agility);
                AddGearToResults(results, gear.hands, strength, intellect, agility);
                AddGearToResults(results, gear.finger1, strength, intellect, agility);
                AddGearToResults(results, gear.finger2, strength, intellect, agility);
                AddGearToResults(results, gear.trinket1, strength, intellect, agility);
                AddGearToResults(results, gear.trinket2, strength, intellect, agility);
                AddGearToResults(results, gear.back, strength, intellect, agility);
                AddGearToResults(results, gear.main_hand, strength, intellect, agility);
                AddGearToResults(results, gear.off_hand, strength, intellect, agility);
            }
            return results;
        }
    }

    public class ScaleFactors
    {
        public double Int;
        public double Agi;
        public double Str;
        public double Crit;
        public double Haste;
        public double Mastery;
        public double Vers;
    }

    public class CollectedData
    {
        public Dps dps;
        public Damage dmg;
        public BuffedStats buffed_stats;
    }

    public class BuffedStats
    {
        public Attribute attribute;
    }

    public class Attribute
    {
        public double strength;
        public double agility;
        public double intellect;
    }

    public class Dps
    {
        public double mean;
    }

    public class Damage
    {
        public double mean;
    }

    public class GearResults
    {
        public int strength, intellect, agility, crit, vers, haste, mastery;
    }

    public class Gear
    {
        public GearItem head;
        public GearItem neck;
        public GearItem shoulders;
        public GearItem chest;
        public GearItem waist;
        public GearItem legs;
        public GearItem feet;
        public GearItem wrists;
        public GearItem hands;
        public GearItem finger1;
        public GearItem finger2;
        public GearItem trinket1;
        public GearItem trinket2;
        public GearItem back;
        public GearItem main_hand;
        public GearItem off_hand;
    }

    public class GearItem
    {
        public int crit_rating;
        public int mastery_rating;
        public int haste_rating;
        public int versatility_rating;
        public int intellect;
        public int agility;
        public int strength;
        public int stragiint;
    }
}
