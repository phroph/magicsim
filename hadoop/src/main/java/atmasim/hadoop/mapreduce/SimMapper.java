package atmasim.hadoop.mapreduce;

import java.io.IOException;
import java.util.Iterator;
import java.util.Map.Entry;

import org.apache.hadoop.io.Text;
import org.apache.hadoop.mapreduce.Mapper;

import com.google.gson.JsonArray;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;

 public class SimMapper  extends Mapper<SimKey, Text, DPSKey, DPSValue>{

    @Override 
    public void map(SimKey key, Text value, Context context) throws IOException, InterruptedException {
        float dps = 0;
        String simString = Text.decode(key.simString.getBytes());
        String talentString = Text.decode(key.talentString.getBytes());
        String reforgeString = Text.decode(value.getBytes());

        JsonArray models = ModelProvider.getProvider().models;
        
        String returnDataPath = "";
        String profileData = "";
        String profilePath = "";
        // Create profile with data output pointer at returnDataPath. Write to path. Execute synchronously.
        SimC.ExecuteSim(profilePath);
        // Collect result from returnDataPath and stuff dps into our variable.
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