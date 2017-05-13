package atmasim.hadoop.mapreduce;

import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;

import com.google.gson.JsonArray;
import com.google.gson.JsonObject;

import com.google.gson.JsonParser;

public class ModelProvider {
    private static ModelProvider modelProvider;

    public JsonArray models;

    public ModelProvider() {
        JsonParser parser = new JsonParser();
        try {     
            models =  parser.parse(new FileReader("c:\\file.json")).getAsJsonArray();
        } catch (FileNotFoundException e) {
            e.printStackTrace();
        }
    }

    public JsonObject getModelByName(String name) {
        for(int i=0; i< models.size(); i++) {
            if(models.get(i).getAsJsonObject().get("name").equals(name)) {
                return models.get(i).getAsJsonObject();
            }
        }
        return null;
    }

    public int getModelIndexByName(String name) {        
        for(int i=0; i< models.size(); i++) {
            if(models.get(i).getAsJsonObject().get("name").equals(name)) {
                return i;
            }
        }
        return 0;
    }

    public static ModelProvider getProvider() {
        if(modelProvider == null) {
            modelProvider = new ModelProvider();
        }
        return modelProvider;
    }
}