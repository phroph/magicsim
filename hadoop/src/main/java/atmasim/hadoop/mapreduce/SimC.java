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
            System.out.println("Executed sim ("+profilePath+") with status code: " + new ProcessBuilder("simc", profilePath).start().waitFor());
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}