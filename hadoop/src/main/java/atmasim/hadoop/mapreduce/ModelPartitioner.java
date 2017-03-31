package atmasim.hadoop.mapreduce;

import org.apache.hadoop.io.Text;
import org.apache.hadoop.mapreduce.Partitioner;

public class ModelPartitioner extends Partitioner<DPSKey, DPSValue> {

    @Override
    public int getPartition(DPSKey key, DPSValue value, int numReduceTasks) {
        try {
            String modelName = Text.decode(key.modelString.getBytes());
            int reducerPartition = Models.getPartitionForModel(Models.getModelByName(modelName));
            
            if(numReduceTasks == 0)
                return 0;

            return reducerPartition % numReduceTasks;
        }
        catch(Exception e) {
            return 0;
        }
    }
}