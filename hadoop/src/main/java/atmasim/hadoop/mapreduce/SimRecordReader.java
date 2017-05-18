package atmasim.hadoop.mapreduce;

import java.io.IOException;
import org.apache.hadoop.mapreduce.lib.input.LineRecordReader;
import org.apache.log4j.Logger;
import org.apache.hadoop.mapreduce.InputSplit;
import org.apache.hadoop.mapreduce.TaskAttemptContext;
import org.apache.hadoop.mapreduce.RecordReader;

import org.apache.hadoop.io.Text;
public class SimRecordReader extends RecordReader<SimKey, Text> {
    private static Logger logger = Logger.getLogger(SimRecordReader.class);
    private LineRecordReader reader;

    private SimKey key;
    private Text value;

    private byte separator;
    public SimRecordReader() {
        reader = new LineRecordReader();
        separator = (byte) ';';
    }

    @Override
    public void initialize(InputSplit genericSplit, TaskAttemptContext context) throws IOException {
        reader.initialize(genericSplit, context);
    }

    public static int findSeparator(byte[] utf, int start, int length, byte sep) {
        for (int i = start; i < (start + length); i++) {
            if (utf[i] == sep) {
                return i;
            }
        }
        return -1;
    } 
   public static void setKeyValue(SimKey key, Text value, byte[] line,int lineLen, int pos, int pos2) {
        if (pos == -1 || pos2 == -1) {
            value.set(line, 0, lineLen);
        } else {
            int simLen = pos;
            byte[] simBytes = new byte[simLen];
            System.arraycopy(line, 0, simBytes, 0, simLen);
            int talentLen = pos2 - simLen - 1;
            byte[] talentBytes = new byte[talentLen];
            System.arraycopy(line, pos + 1, talentBytes, 0, talentLen);
            int valLen = lineLen - talentLen - 1;
            byte[] valBytes = new byte[valLen];
            System.arraycopy(line, pos2 + 1, valBytes, 0, valLen);
            key.set(simBytes,talentBytes);
            value.set(valBytes);
            logger.info("Found key: " + key.toString());
            logger.info("Found value: " + value.toString());
        }
    }

 
    @Override
    public boolean nextKeyValue() throws IOException {
        byte[] line = null;
        int lineLen = -1;
        if (reader.nextKeyValue()) {
            Text innerValue = reader.getCurrentValue();
            line = innerValue.getBytes();
            lineLen = innerValue.getLength();
        } else {
            return false;
        }
        if (line == null)
            return false;
        if (key == null) {
            key = new SimKey("","");
        }
        if (value == null) {
            value = new Text();
        }
        String found = "";
        for(int i = 0; i < line.length; i++) {
            found += (char) line[i];
        }
        logger.info("Found line: " + found);
        int pos = findSeparator(line, 0, lineLen, this.separator);
        int pos2 = findSeparator(line, pos+1, lineLen-(pos+1), this.separator);
        logger.info("Found separator 1: " + pos);
        logger.info("Found separator 2: " + pos2);
        setKeyValue(key, value, line, lineLen, pos, pos2);
        return true;
    }
 
    @Override
    public SimKey getCurrentKey() throws IOException, InterruptedException {
        return key;
    }
 
    @Override
    public Text getCurrentValue() throws IOException, InterruptedException {
        return value;
    }
 
    @Override
    public float getProgress() throws IOException, InterruptedException {
        return reader.getProgress();
    }

    @Override
    public void close() throws IOException {
        reader.close();
    }
}