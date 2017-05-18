package atmasim.hadoop.mapreduce;

import java.io.IOException;
import java.util.Dictionary;
import java.util.Hashtable;
import java.util.Iterator;
import java.util.Map.Entry;
import java.util.Collections;

import org.apache.hadoop.io.FloatWritable;
import org.apache.hadoop.io.Text;
import org.apache.hadoop.mapreduce.Reducer;
import org.apache.hadoop.mapreduce.lib.output.MultipleOutputs;
import org.apache.log4j.Logger;

import com.google.gson.JsonElement;
import com.google.gson.JsonObject;


 public class DPSReducer extends Reducer<DPSKey,DPSValue,DPSKey,ReducedDPSValue> {
    private Logger logger = Logger.getLogger(DPSReducer.class);
    private MultipleOutputs<DPSKey,ReducedDPSValue> output;

    @Override
    public void setup(Context context) {
        output = new MultipleOutputs<>(context);
    }
 
    // We have to make sure that keys aren't just grouped on Key, since we want specifically the key subset:
    // (simname)_(talentstring)_(reforgestring)_(model) where we group on primary key model, where the secondary key is talent string.
    // This ensures we get all the values where model and talent match. Then we can take the list of all reforge sims / sim combinations for given model and talent
    // There we iterate through, reduce like reforge / sim on the model (to get composite reforge and write the list of composite reforge points where final key is: model_reforgestring -> composite dps)
    @Override
    public void reduce(DPSKey key, Iterable<DPSValue> values, Context context) throws IOException, InterruptedException {
        logger.info("Starting to reduce key: " + key.toString());
        String modelString = Text.decode(key.modelString.getBytes());
        JsonObject model = ModelProvider.getProvider().getModelByName(modelString.replace("\"", ""));
        Dictionary<String,Float> reforgeDpsMapping = new Hashtable<>();
        for (DPSValue val : values) {
            logger.info("Value for key: " + val.toString());
            String reforgeString = Text.decode(val.reforgeString.getBytes());
            String simString = Text.decode(val.simString.getBytes());
            if (reforgeDpsMapping.get(reforgeString) == null) {
                logger.info("Created mapping for reforge: " + reforgeString);
                reforgeDpsMapping.put(reforgeString, 0.0f);
            }
            reforgeDpsMapping.put(reforgeString, reforgeDpsMapping.get(reforgeString) + (val.dps.get() * getWeightForSim(simString, model)));
        }
        for (String reforgeString : Collections.list(reforgeDpsMapping.keys())) {
            // Shouldn't we be writing (modelString, talentString) -> (reforgeString, compositeDps) since we are reducing sims into across a model.
            // Which would imply a reduced DPSValue rather than an augmented DPSKey
            ReducedDPSValue val = new ReducedDPSValue(reforgeString, reforgeDpsMapping.get(reforgeString));
            logger.info("Writing out reduced key with value: " + val.toString());
            logger.info("Writing value to path (derived by key): " + generateFileName(key));
            output.write("", val, generateFileName(key));
            //context.write(key, val);
            // Then we can create CSVs based on shared key.
        }
    }

    private String generateFileName(DPSKey key) {
        return key.talentString + "/" + key.modelString;
    }

    private float getWeightForSim(String sim, JsonObject model) {
        if(model == null) {
            logger.info("Model not found.");
            return 0.0f;
        }
        logger.info("Looking at model: " + model.toString());
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