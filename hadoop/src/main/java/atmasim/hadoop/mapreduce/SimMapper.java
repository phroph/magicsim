package atmasim.hadoop.mapreduce;

import java.io.IOException;

import org.apache.hadoop.io.Text;
import org.apache.hadoop.mapreduce.Mapper;
import org.apache.hadoop.conf.Configuration;
import org.apache.hadoop.fs.FileSystem;

 public class SimMapper  extends Mapper<SimKey, Text, DPSKey, DPSValue>{

    @Override 
    public void map(SimKey key, Text value, Context context) throws IOException, InterruptedException {
        float dps = 0;
        String simString = Text.decode(key.simString.getBytes());
        String talentString = Text.decode(key.talentString.getBytes());
        String reforgeString = Text.decode(value.getBytes());
        
        String returnDataPath = "";
        String profileData = "";
        String profilePath = "";
        // Create profile with data output pointer at returnDataPath. Write to path. Execute synchronously.
        SimC.ExecuteSim(profilePath);
        // Collect result from returnDataPath and stuff dps into our variable.
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