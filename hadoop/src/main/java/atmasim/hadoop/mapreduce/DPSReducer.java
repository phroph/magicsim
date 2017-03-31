package atmasim.hadoop.mapreduce;

import java.io.IOException;

import org.apache.hadoop.io.FloatWritable;
import org.apache.hadoop.io.Text;
import org.apache.hadoop.mapreduce.Reducer;


 public class DPSReducer extends Reducer<DPSKey,DPSValue,Text,FloatWritable> {
    private FloatWritable result = new FloatWritable();
 
    // We have to make sure that keys aren't just grouped on Key, since we want specifically the key subset:
    // (simname)_(talentstring)_(reforgestring)_(model) where we group on primary key model, where the secondary key is talent string.
    // This ensures we get all the values where model and talent match. Then we can take the list of all reforge sims / sim combinations for given model and talent
    // There we iterate through, reduce like reforge / sim on the model (to get composite reforge and write the list of composite reforge points where final key is: model_reforgestring -> composite dps)
    public void reduce(DPSKey key, Iterable<DPSValue> values, Context context) throws IOException, InterruptedException {
        String modelString = Text.decode(key.modelString.getBytes());

        // TODO: bucket them by reforge
        float weightedSum = 0;
        for (DPSValue val : values) {
            String simString = Text.decode(val.simString.getBytes());
            float dps = val.dps.get();
            weightedSum += dps * getWeightForSim(simString, Models.getModelByName(modelString));
        }
        result.set(weightedSum);
        context.write(new Text(key.toString()), new FloatWritable(weightedSum));
    }

    private float getWeightForSim(String sim, Models model) {
        return 0.0f;
    }
}