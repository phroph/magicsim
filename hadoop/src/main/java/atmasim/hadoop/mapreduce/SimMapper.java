package atmasim.hadoop.mapreduce;

import java.io.File;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.nio.file.Paths;
import java.util.Iterator;
import java.util.UUID;
import java.util.Map.Entry;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import org.apache.hadoop.io.Text;
import org.apache.hadoop.mapreduce.Mapper;
import org.apache.log4j.Logger;

import com.google.gson.JsonArray;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;

import atmasim.hadoop.wow.*;

 public class SimMapper  extends Mapper<SimKey, Text, DPSKey, DPSValue>{
    private Logger logger = Logger.getLogger(SimMapper.class);

    GearSlot mangaza = new GearSlot(GearSlots.WAIST, "132864", "1811/3570", false);
    GearSlot sephuz = new GearSlot(GearSlots.FINGER1, "132452", "3529/3459/3570", "150haste", "200haste", false);
    GearSlot zenk;
    GearSlot shahraz;
    GearSlot anunds;
    GearSlot kjbw;
    GearSlot soul;
    GearSlot zeks;
    GearSlot hotv;

    private GearSlot GetLegendaryByName(String name) {
        if(name.equals("mangaza")) {
            return mangaza;
        } else if(name.equals("sephuz")) {
            return sephuz;
        } else if(name.equals("shahraz")) {
            return shahraz;
        } else if(name.equals("soul")) {
            return soul;
        } else if(name.equals("zeks")) {
            return zeks;
        }
        return null;
    }

    @Override 
    public void map(SimKey key, Text value, Context context) throws IOException, InterruptedException {
        String simString = key.simString.toString(); // 'time_style_adds_bosses'
        String talentString = key.talentString.toString();  //number string of talents
        String modifierString = value.toString(); //c:val,m:val,h:val
        String reforgeString = "";
        String gearString = "";
        String label = "";
        boolean isReforge = false;
        String[] gearData = modifierString.split(":::");
        String[] modifierData = modifierString.split(";");
        String[] reforgeData = modifierData[0].split(",");
        if(gearData.length == 1) {
            isReforge = true;
            reforgeString = modifierString;
        } else {
            gearString = gearData[1];
            label = gearData[0];
            gearString = gearString.replaceAll("%NL%", "\n");
        }

        logger.info("Found key: " + key.toString());
        logger.info("Found value: " + modifierString);

        JsonArray models = ModelProvider.getProvider().models;
        
        String ba = "raid_events+=/adds,count=1,first=30,cooldown=60,duration=20\n";
        String sa = "raid_events+=/adds,count=3,first=45,cooldown=45,duration=10,distance=5\n";
        String doubleTarget = "enemy=enemy2\nraid_events+=/move_enemy,name=enemy2,cooldown=2000,duration=1000,x=-27,y=-27\n";

        String talents = talentString.replace(",", "") + "\n";
        String artifact = "47:0:0:0:0:764:1:765:1:766:1:767:4:768:1:769:1:770:1:771:4:772:4:773:4:774:4:775:4:776:4:777:4:778:4:779:1:1347:1:1381:1:1573:4:1574:1:1575:1:1576:24:1650:1\n";

        String header = "ptr=1\noutput=nul\nstrict_work_queue=1\nreport_details=0\npriest=\"Atmasim\"\nlevel=110\nrace=troll\nrole=spell\npriest_ignore_healing=1\nposition=back\n" + talents + artifact + "spec=shadow\ndefault_actions=1\n";
        int threads = 8;
        String gear; 

        //Pyrdaz is just stats, worthless to sim
        //Norgannon's is worthless to sim
        //Twin's is worthless to sim

        //GearSlot head = new GearSlot(GearSlots.HEAD, "138313", "3516/1487/1813", false);
        GearSlot neck = new GearSlot(GearSlots.NECK, "141325", "3562/41/1502/3336", "mark_of_the_claw", false);
        //GearSlot shoulders = new GearSlot(GearSlots.SHOULDERS, "147168", "3562/1502/3336", false);
        //GearSlot back = new GearSlot(GearSlots.BACK, "146984", "3563/1512/3528", "200int", false);
        //GearSlot chest = new GearSlot(GearSlots.CHEST, "147167", "3562/1497/3528", false);
        //GearSlot wrists = new GearSlot(GearSlots.WRISTS, "147001", "3561/41/1502/3337", false);
        //GearSlot hands = new GearSlot(GearSlots.HANDS, "147164", "3562/1507/3336", false);
        //GearSlot legs = new GearSlot(GearSlots.LEGS, "146992", "3562/1808/1507/3336", "150haste");
        //GearSlot feet = new GearSlot(GearSlots.FEET, "134416", "3536/1582/3337", false);
        //GearSlot finger2 = new GearSlot(GearSlots.FINGER2, "147195", "3562/1512/3336", "200haste", false);
        //GearSlot trinket1 = new GearSlot(GearSlots.TRINKET1, "141482", "1472", false);
        //GearSlot trinket2 = new GearSlot(GearSlots.TRINKET2, "141482", "1472", false);
        //main_hand=,id=128862
        GearSlot main_hand = new GearSlot(GearSlots.MAIN_HAND, "128827");
        //GearSlot off_hand = new GearSlot(GearSlots.OFF_HAND, "133958");

        // Upgrade to parse from parameters
        GearSlot leg1 = null;
        GearSlot leg2 = null;
        String setString = "set_bonus=tier19_2pc=0\nset_bonus=tier19_4pc=0\nset_bonus=tier20_2pc=0\nset_bonus=tier20_4pc=0\nset_bonus=tier21_2pc=1\nset_bonus=tier21_4pc=1\n";
        //GearSet gearSet = new GearSet(head, neck, shoulders, back, chest, wrists, hands, waist, legs, feet, finger1, finger2, trinket1, trinket2, main_hand, off_hand, setString);
        SimCharacter character = null;// = new SimCharacter(leg1.toString() +"\n" + leg2.toString() + "\n" + setString, "artifact=47:0:0:0:0:764:1:765:1:766:1:767:4:768:1:769:1:770:1:771:4:772:4:773:4:774:4:775:4:776:4:777:4:778:4:779:1:1347:1:1381:1:1573:4:1574:1:1575:1:1576:15:1650:1\n", talentString.replace(",", ""), "0");
        
        String[] simData = simString.split("_");


        if(isReforge) {
            String crit = reforgeData[0];
            String mastery = reforgeData[1];
            String haste = reforgeData[2];
            String intellect = reforgeData[3];
            String[] traits = modifierData[1].split(":");
            String legendary1 = modifierData[2];
            String legendary2 = modifierData[3];
            String crucible = modifierData[4];
            for(String trait : traits) {
                Pattern regex = Pattern.compile(":"+trait+":(\\d+):");
                Matcher matcher = regex.matcher(artifact);
                if(matcher.find()) {
                    int traitValue = Integer.parseInt(matcher.group(1));
                    logger.info("Found trait " + trait + " with value " + traitValue);
                    artifact = artifact.replace(":"+trait+":"+traitValue+":", ":"+trait+":"+(++traitValue)+":");
                    logger.info("Updated string to " + artifact);
                }
            }
            leg1 = GetLegendaryByName(legendary1);
            leg2 = GetLegendaryByName(legendary2);
            
            gear = neck.toString() + "\n" + leg1.toString() + "\n" + leg2.toString() + "\n" + main_hand.toString() + "\n"
            + "gear_versatility_rating=0\ngear_intellect="+intellect.split(":")[1]
            + "\ngear_crit_rating="+ crit.split(":")[1] 
            + "\ngear_haste_rating="+ haste.split(":")[1] 
            + "\ngear_mastery_rating="+ mastery.split(":")[1] + "\n"
            + setString;

            character = new SimCharacter(gear, artifact, crucible, talents, "1");
        } else {
            gear = gearString;
            reforgeString = label;
        }
        //String[] simData = simString.split("_");
        String base = "iterations=2000\nthreads="+threads+"\nmax_time="+ simData[0] +"\noptimal_raid=1\nfight_style="+ simData[1] +"\nenemy=enemy1\n";
        //waist=mangazas_madness,id=132864,bonus_id=3459/3530
        //finger2=sephuzs_secret,id=132452,bonus_id=3529/3530/1811,gems=150haste,enchant=200haste

        String addString = "\n";
        if(simData[2].equals("ba")) { 
            addString = ba;
        } else if(simData[2].equals("sa")) {
            addString = sa;
        }

        String targetString = "\n";
        if(simData[3].equals("2t")) {
            targetString = doubleTarget;
        }       
         
        String uuid = UUID.randomUUID().toString();
        String runtimePath = new File(".").getCanonicalPath();
        String profileName = "atmasim-"+uuid;
        Simulation simulation = new Simulation(character, simData[0], simData[1], Integer.toString(threads), addString, targetString, profileName);


        String footer = "json2=" + profileName + ".json\n";
        String profileData = simulation.toString();
        //String profileData = header + gear + base + targetString + addString + footer;
        
        String profilePath = Paths.get(runtimePath, profileName + ".simc").toString();
        File profile = new File(profilePath);
        logger.info("Created profile from key/value data:");
        logger.info("*** START OF PROFILE DATA ***");
        logger.info(profileData);
        logger.info("*** END OF PROFILE DATA ***");
        FileWriter fWriter = new FileWriter(profile);
        fWriter.write(profileData);
        fWriter.close();
        logger.info("Executing profile saved at: " + profilePath);
        SimC.ExecuteSim(profilePath);
        logger.info("SimC finished executing. Collecting results.");
        JsonObject results = new JsonParser().parse(new FileReader(Paths.get(runtimePath, profileName + ".json").toFile())).getAsJsonObject();
        float dps = results.get("sim").getAsJsonObject()
                        .get("players").getAsJsonArray()
                        .get(0).getAsJsonObject()
                        .get("collected_data").getAsJsonObject()
                        .get("dps").getAsJsonObject()
                        .get("mean").getAsFloat();
        logger.info("Found DPS: " + dps);
        // Write out the simstring self-keyed, to prevent merging.
        context.write(new DPSKey(talentString, simString, modifierData[2]+"+"+modifierData[3],modifierData[1],modifierData[4]), new DPSValue(simString, reforgeString, dps));
        for(int i = 0; i < models.size(); i++) {
            JsonObject model = models.get(i).getAsJsonObject();
            String modelName = model.get("name").toString();
            if(doesModelContainSim(model, simString)) {
                logger.info("Created DPSKey/Value for model: " + modelName);
                context.write(new DPSKey(talentString, modelName, modifierData[2]+"+"+modifierData[3],modifierData[1],modifierData[4]), new DPSValue(simString, reforgeString, dps));
            }
        }
        logger.info("Finished mapping key: " + key.toString());
    }
    
    public boolean doesModelContainSim(JsonObject model, String simName) {
        JsonObject sims = model.get("model").getAsJsonObject();
        Iterator<Entry<String,JsonElement>> simKeys = sims.entrySet().iterator();
        logger.info("Checking if model ("+model.get("name")+") contains: " + simName);
        while(simKeys.hasNext()) {
            if (simName.contains(simKeys.next().getKey())) {
                logger.info("Successfully found sim in model.");
                return true;
            }
        }
        logger.info("Did not find sim in model.");
        return false;
    }
 }