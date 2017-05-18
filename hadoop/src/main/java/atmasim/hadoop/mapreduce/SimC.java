package atmasim.hadoop.mapreduce;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.lang.ProcessBuilder.Redirect;

import org.apache.log4j.Logger;

// EMR Bootstrap download/build simc + put it into path/link in bin. No need to acquire runtime or singleton access management. 
public class SimC {
    private static Logger logger = Logger.getLogger(SimC.class);
    public static void ExecuteSim(String profilePath) {
        logger.info("Starting SimC process.");
        try {
            Process simc = new ProcessBuilder("simc", profilePath).redirectErrorStream(true).start();

            BufferedReader reader = new BufferedReader(new InputStreamReader(simc.getInputStream()));
            StringBuilder builder = new StringBuilder();
            String line = null;
            while (((line = reader.readLine()) != null))  {
                builder.append(line);
                builder.append(System.getProperty("line.separator"));
            }
            logger.info("*** START SIMC LOGS ***");
            logger.info(builder.toString());
            logger.info("*** END SIMC LOGS ***");
            logger.info("Executed sim ("+profilePath+") with status code: " + simc.waitFor());
        } catch (Exception e) {
            logger.error(e);
        }
    }
}