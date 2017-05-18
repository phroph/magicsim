package atmasim.hadoop.mapreduce;

import java.io.InputStreamReader;

import org.apache.log4j.Logger;

import com.google.gson.JsonArray;
import com.google.gson.JsonObject;

import com.google.gson.JsonParser;

public class ModelProvider {
    private static ModelProvider modelProvider;
    private Logger logger = Logger.getLogger(ModelProvider.class);

    public JsonArray models;

    public ModelProvider() {
        models =  new JsonParser().parse(new InputStreamReader(getClass().getResourceAsStream("/models.json"))).getAsJsonArray();
    }

    public JsonObject getModelByName(String name) {
        logger.info("Trying to find model: " + name);
        for(int i=0; i< models.size(); i++) {
            String modelName = models.get(i).getAsJsonObject().get("name").getAsString();
            logger.info("Trying: " + modelName);
            if(modelName.compareTo(name) == 0) {
                logger.info("Returning model object: " + models.get(i).toString());
                return models.get(i).getAsJsonObject();
            }
        }
        logger.info("Model not found.");
        return null;
    }

    public int getModelIndexByName(String name) { 
        logger.info("Trying to find model: " + name);       
        for(int i=0; i< models.size(); i++) {
            logger.info("Trying: " + models.get(i).getAsJsonObject().get("name"));
            if(models.get(i).getAsJsonObject().get("name").getAsString().equals(name)) {
                logger.info("Returning model index: " + i);
                return i;
            }
        }
        logger.info("Model not found.");
        return 0;
    }

    public static ModelProvider getProvider() {
        if(modelProvider == null) {
            modelProvider = new ModelProvider();
        }
        return modelProvider;
    }
}