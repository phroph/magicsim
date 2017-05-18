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
            Process simc = new ProcessBuilder("simc", profilePath).start();
            BufferedReader reader = new BufferedReader(new InputStreamReader(simc.getInputStream()));
            BufferedReader eReader = new BufferedReader(new InputStreamReader(simc.getInputStream()));
            StringBuilder builder = new StringBuilder();
            StringBuilder eBuilder = new StringBuilder();
            String line = null, eLine = null;
            while (((line = reader.readLine()) != null) || ((eLine = eReader.readLine()) != null))  {
                if (line == null) {
                    // We only get here if reader is finished. So we only read error if no remaining lines. (to prevent blocking)
                    eBuilder.append(eLine);
                    eBuilder.append(System.getProperty("line.separator"));
                } else {
                    builder.append(line);
                    builder.append(System.getProperty("line.separator"));
                }
            }
            logger.info("*** START SIMC LOGS ***");
            logger.info(builder.toString());
            logger.warn(eBuilder.toString());
            logger.info("*** END SIMC LOGS ***");
            logger.info("Executed sim ("+profilePath+") with status code: " + simc.waitFor());
        } catch (Exception e) {
            logger.error(e);
        }
    }
}