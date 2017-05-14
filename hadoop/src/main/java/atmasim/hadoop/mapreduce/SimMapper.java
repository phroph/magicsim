package atmasim.hadoop.mapreduce;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.Iterator;
import java.util.UUID;
import java.util.Map.Entry;

import org.apache.hadoop.io.Text;
import org.apache.hadoop.mapreduce.Mapper;

import com.google.gson.JsonArray;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;

 public class SimMapper  extends Mapper<SimKey, Text, DPSKey, DPSValue>{

    @Override 
    public void map(SimKey key, Text value, Context context) throws IOException, InterruptedException {
        String simString = Text.decode(key.simString.getBytes()); // 'time_style_adds_bosses'
        String talentString = Text.decode(key.talentString.getBytes());  //number string of talents
        String reforgeString = Text.decode(value.getBytes()); //c:val,m:val,h:val

        JsonArray models = ModelProvider.getProvider().models;
        
        String ba = "raid_events+=/adds,count=1,first=30,cooldown=60,duration=20\n";
        String sa = "raid_events+=/adds,count=3,first=45,cooldown=45,duration=10,distance=5\n";
        String doubleTarget = "enemy=enemy2\nraid_events+=/move_enemy,name=enemy2,cooldown=2000,duration=1000,x=-27,y=-27\n";

        String talents = "talents=" + talentString + "\n";
        String artifact = "artifact=47:0:0:0:0:764:1:765:1:766:1:767:3:768:1:769:1:770:1:771:3:772:3:773:4:774:3:775:3:776:4:777:4:778:3:779:1:1347:1:1381:1:1573:4:1574:1:1650:1\n";

        String header = "priest=\"Atmasim\"\nlevel=110\nrace=troll\nrole=spell\npriest_ignore_healing=1\nposition=back\n" + talents + artifact + "spec=shadow\ndefault_actions=1\n";
        int threads = 16;

        // TODO Setup gear from reforge.
        String gear = "\n";
        String base = "iterations=10000\nthreads="+threads+"\ntarget_error=0.000\nmax_time="+ simString.split("_")[0] +"\noptimal_raid=1\nfight_style="+ simString.split("_")[1] +"\nenemy=enemy1\n";

        String addString = "\n";
        if(simString.split("_")[2].equals("ba")) { 
            addString = ba;
        } else if(simString.split("_")[2].equals("sa")) {
            addString = sa;
        }

        String targetString = "\n";
        if(simString.split("_")[3].equals("2t")) {
            targetString = doubleTarget;
        }

        String uuid = UUID.randomUUID().toString();
        String runtimePath = new File(".").getCanonicalPath();
        String profileName = "atmasim-"+uuid;
        String footer = "json=" + profileName + ".json\n";
        String profileData = header + gear + base + targetString + addString + footer;
        String profilePath = Paths.get(runtimePath, profileName + ".simc").toString();
        File profile = new File(profilePath);
        FileWriter fWriter = new FileWriter(profile);
        fWriter.write(profileData);
        fWriter.close();
        SimC.ExecuteSim(profilePath);

        JsonElement results = new JsonParser().parse(new FileReader(Paths.get(runtimePath, profileName + ".json").toFile()));
        float dps = 0;
        
        for(int i = 0; i <= models.size(); i++) {
            JsonObject model = models.get(i).getAsJsonObject();
            String modelName = model.get("name").toString();
            if(doesModelContainSim(model, simString)) {
                context.write(new DPSKey(talentString, modelName), new DPSValue(simString, reforgeString, dps));
            }
        }
    }
    
    public boolean doesModelContainSim(JsonObject model, String simName) {
        JsonObject sims = model.get("model").getAsJsonObject();
        Iterator<Entry<String,JsonElement>> simKeys = sims.entrySet().iterator();
        while(simKeys.hasNext()) {
            if (simName.contains(simKeys.next().getKey())) {
                return true;
            }
        }
        return false;
    }
 }