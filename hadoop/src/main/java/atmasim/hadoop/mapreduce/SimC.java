package atmasim.hadoop.mapreduce;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;

import java.lang.Process;
import java.lang.Runtime;

// EMR Bootstrap download/build simc + put it into path/link in bin. No need to acquire runtime or singleton access management. 
public class SimC {
    public static void ExecuteSim(String profilePath) {
        try {
            Process p = Runtime.getRuntime().exec(new String[]{"simc", profilePath});
            BufferedReader reader = new BufferedReader(new InputStreamReader(p.getInputStream()));
            String line = null;
            while ((line = reader.readLine()) != null) {
                System.out.println("O: " + line);
            }
            reader = new BufferedReader(new InputStreamReader(p.getErrorStream()));
            while ((line = reader.readLine()) != null) {
            System.out.println("E: " + line);
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}