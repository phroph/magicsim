package atmasim.hadoop.mapreduce;

import java.io.IOException;
import org.apache.hadoop.mapreduce.lib.input.FileSplit;
import org.apache.hadoop.fs.Path;
import org.apache.hadoop.fs.FileSystem;
import org.apache.hadoop.fs.FSDataInputStream;
import org.apache.hadoop.conf.Configuration;
import org.apache.hadoop.mapreduce.InputSplit;
import org.apache.hadoop.mapreduce.TaskAttemptContext;
import org.apache.hadoop.mapreduce.RecordReader;

import org.apache.hadoop.io.Text;
public class SimRecordReader extends RecordReader<SimKey, Text> {
 
    private long start;
    private long pos;
    private long end;
    private FSDataInputStream in;

    private SimKey key;
    private Text value;
 
    @Override
    public void initialize(InputSplit genericSplit, TaskAttemptContext context) throws IOException {

        FileSplit split = (FileSplit) genericSplit;

        start = split.getStart();
        end = start + split.getLength();
 
        Configuration job = context.getConfiguration();
        final Path file = split.getPath();
        FileSystem fs = file.getFileSystem(job);
        in = fs.open(split.getPath());
        in.seek(start);
 
        // Position is the actual start
        this.pos = start;
    }
 
    @Override
    public boolean nextKeyValue() throws IOException {
 
        // Current offset is the key
 
        SimKey newKey = new SimKey("","");
        Text newValue = new Text();

        newKey.readFields(in);
        value.readFields(in);
        this.pos = in.getPos();
        if (pos >= end) {
            // We've reached end of Split
            key = null;
            value = null;
            return false;
        } else {
            key = newKey;
            value = newValue;
            return true;
        }
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
        if (start == end) {
            return 0.0f;
        } else {
            return Math.min(1.0f, (pos - start) / (float) (end - start));
        }
    }

    @Override
    public void close() throws IOException {
        in.close();
    }
}