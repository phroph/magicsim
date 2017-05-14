package atmasim.hadoop.mapreduce;

import java.io.IOException;
import java.util.Dictionary;
import java.util.Hashtable;
import java.util.Iterator;
import java.util.Map.Entry;
import java.util.Collections;

import org.apache.hadoop.io.Text;
import org.apache.hadoop.mapreduce.Reducer;

import com.google.gson.JsonElement;
import com.google.gson.JsonObject;


 public class DPSReducer extends Reducer<DPSKey,DPSValue,DPSKey,ReducedDPSValue> {
 
    // We have to make sure that keys aren't just grouped on Key, since we want specifically the key subset:
    // (simname)_(talentstring)_(reforgestring)_(model) where we group on primary key model, where the secondary key is talent string.
    // This ensures we get all the values where model and talent match. Then we can take the list of all reforge sims / sim combinations for given model and talent
    // There we iterate through, reduce like reforge / sim on the model (to get composite reforge and write the list of composite reforge points where final key is: model_reforgestring -> composite dps)
    public void reduce(DPSKey key, Iterable<DPSValue> values, Context context) throws IOException, InterruptedException {
        String modelString = Text.decode(key.modelString.getBytes());
        String talentString = Text.decode(key.talentString.getBytes());
        JsonObject model = ModelProvider.getProvider().getModelByName(modelString);
        Dictionary<String,Float> reforgeDpsMapping = new Hashtable<>();
        for (DPSValue val : values) {
            String reforgeString = Text.decode(val.reforgeString.getBytes());
            String simString = Text.decode(val.simString.getBytes());
            if (reforgeDpsMapping.get(reforgeString) == null) {
                reforgeDpsMapping.put(reforgeString, 0.0f);
            }
            reforgeDpsMapping.put(reforgeString, reforgeDpsMapping.get(reforgeString) + (val.dps.get() * getWeightForSim(simString, model)));
        }
        for (String reforgeString : Collections.list(reforgeDpsMapping.keys())) {
            // Shouldn't we be writing (modelString, talentString) -> (reforgeString, compositeDps) since we are reducing sims into across a model.
            // Which would imply a reduced DPSValue rather than an augmented DPSKey
            context.write(key, new ReducedDPSValue(reforgeString, reforgeDpsMapping.get(reforgeString)));
            // Then we can create CSVs based on shared key.
        }
    }

    private float getWeightForSim(String sim, JsonObject model) {
        JsonObject sims = model.get("model").getAsJsonObject();
        Iterator<Entry<String,JsonElement>> simKeys = sims.entrySet().iterator();
        while(simKeys.hasNext()) {
            Entry<String,JsonElement> key = simKeys.next();
            if (sim.contains(key.getKey())) {
                return key.getValue().getAsFloat();
            }
        }
        return 0.0f;
    }
}