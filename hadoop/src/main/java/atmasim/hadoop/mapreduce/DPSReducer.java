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


 public class DPSReducer extends Reducer<DPSKey,DPSValue,FloatWritable,ReducedDPSValue> {
    private Logger logger = Logger.getLogger(DPSReducer.class);
    private MultipleOutputs<FloatWritable,ReducedDPSValue> output;

    @Override
    public void setup(Context context) {
        logger.info("Setting up MultipleOutputs writer.");
        output = new MultipleOutputs<FloatWritable,ReducedDPSValue>(context);
    }
 
    // We have to make sure that keys aren't just grouped on Key, since we want specifically the key subset:
    // (simname)_(talentstring)_(reforgestring)_(model) where we group on primary key model, where the secondary key is talent string.
    // This ensures we get all the values where model and talent match. Then we can take the list of all reforge sims / sim combinations for given model and talent
    // There we iterate through, reduce like reforge / sim on the model (to get composite reforge and write the list of composite reforge points where final key is: model_reforgestring -> composite dps)
    @Override
    public void reduce(DPSKey key, Iterable<DPSValue> values, Context context) throws IOException, InterruptedException {
        logger.info("Starting to reduce key: " + key.toString());
        String modelString = key.modelString.toString().replace("\"", "");
        JsonObject model = ModelProvider.getProvider().getModelByName(modelString);
        Dictionary<String,Float> reforgeDpsMapping = new Hashtable<>();
        Dictionary<String,Float> reforgeCalibrationMapping = new Hashtable<>();
        for (DPSValue val : values) {
            logger.info("Value for key: " + val.toString());
            String reforgeString = val.reforgeString.toString();
            String simString = val.simString.toString();
            if (reforgeDpsMapping.get(reforgeString) == null) {
                logger.info("Created mapping for reforge: " + reforgeString);
                reforgeDpsMapping.put(reforgeString, 0.0f);
                reforgeCalibrationMapping.put(reforgeString, 0.0f);
            }
            float weight = modelString.contains("_") ? 1.0f : getWeightForSim(simString, model);
            reforgeDpsMapping.put(reforgeString, reforgeDpsMapping.get(reforgeString) + (val.dps.get() * weight));
            reforgeCalibrationMapping.put(reforgeString, reforgeCalibrationMapping.get(reforgeString) + (weight));
        }
        for (String reforgeString : Collections.list(reforgeDpsMapping.keys())) {
            // Shouldn't we be writing (modelString, talentString) -> (reforgeString, compositeDps) since we are reducing sims into across a model.
            // Which would imply a reduced DPSValue rather than an augmented DPSKey
            ReducedDPSValue val = new ReducedDPSValue(reforgeString, reforgeDpsMapping.get(reforgeString));
            logger.info("Writing out reduced key with value: " + val.toString());
            logger.info("Writing value to path (derived by key): " + generateFileName(key));
            output.write(new FloatWritable(reforgeCalibrationMapping.get(reforgeString)), val, generateFileName(key));
            //context.write(key, val);
            // Then we can create CSVs based on shared key.
        }
    }

    private String generateFileName(DPSKey key) {
        return key.talentString.toString().replaceAll(",", "") + "/" 
        + key.modelString.toString().replaceAll("\"", "") 
        + "/" + key.legendaries.toString().replaceAll("+", "and")
        + key.relics.toString() + "and" + key.crucible.toString().replaceAll(":", "-");
    }

    private float getWeightForSim(String sim, JsonObject model) {
        if(model == null) {
            return 0.0f;
        }
        float timeWeight = 0.0f;
        float simWeight = 0.0f;
        logger.info("Getting weight for: " + sim);
        JsonObject sims = model.get("model").getAsJsonObject();
        Iterator<Entry<String,JsonElement>> simKeys = sims.entrySet().iterator();
        while(simKeys.hasNext()) {
            Entry<String,JsonElement> key = simKeys.next();
            logger.info("Trying key: " + key.getKey());
            if (sim.contains(key.getKey())) {
                simWeight = key.getValue().getAsFloat();
                logger.info("Found sim weight: " + simWeight);
            }
        }
        JsonObject times = model.get("timeModel").getAsJsonObject();
        Iterator<Entry<String,JsonElement>> timeKeys = times.entrySet().iterator();
        while(timeKeys.hasNext()) {
            Entry<String,JsonElement> key = timeKeys.next();
            logger.info("Trying key: " + key.getKey());
            if (sim.contains(key.getKey())) {
                timeWeight = key.getValue().getAsFloat();
                logger.info("Found time weight: " + timeWeight);
            }
        }
        logger.info("Found final weight: " + timeWeight * simWeight);
        return timeWeight * simWeight;
    }
    
    @Override
    protected void cleanup(Context context) throws IOException, InterruptedException {
        if (output != null) {
            output.close();
        }
    }
}