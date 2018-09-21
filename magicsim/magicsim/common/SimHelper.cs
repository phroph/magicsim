using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace magicsim.common
{
    public static class SimHelper
    {
        public static String CreateSimForModel(string model, string time, bool ptr, bool fixedError, List<Sim> sims, bool statWeights, bool reforge, bool reforgeHaste, bool reforgeCrit, bool reforgeVers, bool reforgeMastery, int reforgeAmount, int reforgeStepSize, int threads, string guid, bool disableBuffs, bool editNames) { 
            try
            {
                var simProfile = "";
                if (ptr)
                {
                    simProfile += "ptr=1";
                }
                sims.ForEach((simChar) =>
                {
                    if (editNames)
                    {
                        var nameRegex = new Regex("[^=]+=\"?([^\r\n\"]+)\"?");
                        simChar.Profile = simChar.Profile.Replace(nameRegex.Match(simChar.Profile).Groups[1].Value, simChar.Name);
                    }
                    simProfile += simChar.Profile + "\r\n";
                    if (disableBuffs)
                    {
                        simProfile += "potion=disabled\r\nflask=disabled\r\nfood=disabled\r\noverride.bloodlust=0\r\n";
                    }
                });
                if (fixedError)
                {
                    simProfile += "target_error=0.1\r\n";
                }
                simProfile += "iterations=10000\r\nthreads=" + threads + "\r\n";
                if (disableBuffs)
                {
                    simProfile += "optimal_raid=0\r\n";
                }
                else
                {
                    simProfile += "optimal_raid=1\r\n";
                }
                if (time != null)
                {
                    simProfile += "max_time=" + time + "\r\n";
                }
                if (Directory.EnumerateFiles("adaptiveTemplates").Contains("adaptiveTemplates" + Path.DirectorySeparatorChar + model + ".simc"))
                {
                    simProfile += File.ReadAllText("adaptiveTemplates" + Path.DirectorySeparatorChar + model + ".simc") + "\r\n";
                }
                else
                {
                    MessageBox.Show(String.Format("Couldn't find template \"{0}\" requested. Sim results will be skewed.", model), "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return "";
                }

                var filePrefix = time != null ? "results" + Path.DirectorySeparatorChar + guid + Path.DirectorySeparatorChar + time + "_" + model + "."
                    : "results" + Path.DirectorySeparatorChar + guid + Path.DirectorySeparatorChar + model + ".";

                if (!statWeights)
                {
                    simProfile += "calculate_scale_factors=0\r\n";
                }
                else
                {
                    simProfile += "calculate_scale_factors=1\r\n";
                }
                if (reforge)
                {
                    var reforge_stat = "";
                    if (reforgeCrit)
                    {
                        reforge_stat += "crit";
                    }
                    if (reforgeHaste)
                    {
                        if (reforge_stat.Equals(""))
                        {
                            reforge_stat += "haste";
                        }
                        else
                        {
                            reforge_stat += ",haste";
                        }
                    }
                    if (reforgeMastery)
                    {
                        if (reforge_stat.Equals(""))
                        {
                            reforge_stat += "mastery";
                        }
                        else
                        {
                            reforge_stat += ",mastery";
                        }
                    }
                    if (reforgeVers)
                    {
                        if (reforge_stat.Equals(""))
                        {
                            reforge_stat += "versatility";
                        }
                        else
                        {
                            reforge_stat += ",versatility";
                        }
                    }
                    if (!reforge_stat.Contains(","))
                    {
                        MessageBox.Show(String.Format("At least 2 stats are required to reforge. Reforging will be disabled.", model), "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    else if (reforgeAmount == 0 || reforgeStepSize == 0)
                    {
                        MessageBox.Show("No reforging size supplied. Reforging will be disabled", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    else
                    {
                        simProfile += "reforge_plot_stat=" + reforge_stat + "\r\nreforge_plot_amount=" + reforgeAmount + "\r\nreforge_plot_step=" + reforgeStepSize + "\r\nreforge_plot_output_file=" + filePrefix + "csv\r\n";
                    }
                }

                simProfile += "json2=" + filePrefix + "json\r\nhtml=" + filePrefix + "html\r\n";
                return simProfile;
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to create your character model. You may have selected an invalid character name or advanced parameter.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return "";
            }
        }
    }
}
