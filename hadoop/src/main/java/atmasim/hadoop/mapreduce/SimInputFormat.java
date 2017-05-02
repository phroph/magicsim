package atmasim.hadoop.mapreduce;

import org.apache.hadoop.mapreduce.lib.input.FileInputFormat;
import org.apache.hadoop.mapreduce.InputSplit;
import org.apache.hadoop.mapreduce.TaskAttemptContext;
import org.apache.hadoop.mapreduce.RecordReader;

import org.apache.hadoop.io.Text;

public class SimInputFormat extends FileInputFormat<SimKey, Text> {
    @Override
    public RecordReader<SimKey, Text> createRecordReader(InputSplit split, TaskAttemptContext context) {
        return new SimRecordReader();
    }
}