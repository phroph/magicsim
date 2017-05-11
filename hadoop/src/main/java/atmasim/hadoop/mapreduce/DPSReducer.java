package atmasim.hadoop.mapreduce;

import java.io.IOException;
import java.util.Dictionary;
import java.util.Hashtable;
import java.util.Collections;

import org.apache.hadoop.io.FloatWritable;
import org.apache.hadoop.io.Text;
import org.apache.hadoop.mapreduce.Reducer;


 public class DPSReducer extends Reducer<DPSKey,DPSValue,CompositeDPSKey,FloatWritable> {
 
    // We have to make sure that keys aren't just grouped on Key, since we want specifically the key subset:
    // (simname)_(talentstring)_(reforgestring)_(model) where we group on primary key model, where the secondary key is talent string.
    // This ensures we get all the values where model and talent match. Then we can take the list of all reforge sims / sim combinations for given model and talent
    // There we iterate through, reduce like reforge / sim on the model (to get composite reforge and write the list of composite reforge points where final key is: model_reforgestring -> composite dps)
    public void reduce(DPSKey key, Iterable<DPSValue> values, Context context) throws IOException, InterruptedException {
        String modelString = Text.decode(key.modelString.getBytes());
        String talentString = Text.decode(key.talentString.getBytes());
        Models model = Models.getModelByName(modelString);
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
            context.write(new CompositeDPSKey(modelString, reforgeString, talentString), new FloatWritable(reforgeDpsMapping.get(reforgeString)));
        }
    }

    private float getWeightForSim(String sim, Models model) {
        return 0.0f;
    }
}