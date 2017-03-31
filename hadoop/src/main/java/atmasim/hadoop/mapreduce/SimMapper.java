package atmasim.hadoop.mapreduce;

import java.io.IOException;
import java.util.regex.Pattern;

import org.apache.hadoop.io.FloatWritable;
import org.apache.hadoop.io.Text;
import org.apache.hadoop.mapreduce.Mapper;

 public class SimMapper  extends Mapper<SimKey, Text, DPSKey, DPSValue>{
    public void map(SimKey key, Text value, Context context) throws IOException, InterruptedException {
        float dps = 0;
        String simString = Text.decode(key.simString.getBytes());
        String talentString = Text.decode(key.talentString.getBytes());
        String reforgeString = Text.decode(value.getBytes());
        // build the sim from these values.

        // run the sim through some mechanism and collect the result.
        for(int i = 0; i<=Models.reducerCount; i++) {
            String model = Models.getModelByNumber(i).toString().toLowerCase();
            if(doesModelContainSim(model, model)) {
                context.write(new DPSKey(talentString, model), new DPSValue(simString, reforgeString, dps));
            }
        }
    }
    
    public boolean doesModelContainSim(String modelName, String simName) {
        return false;
    }
 }