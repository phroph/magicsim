package atmasim.hadoop.mapreduce;

import org.apache.hadoop.mapreduce.lib.input.FileInputFormat;
import org.apache.hadoop.mapreduce.lib.input.FileSplit;
import org.apache.hadoop.mapreduce.InputSplit;
import org.apache.hadoop.mapreduce.JobContext;
import org.apache.hadoop.mapreduce.TaskAttemptContext;
import org.apache.hadoop.mapreduce.RecordReader;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.ArrayList;
import java.util.List;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.apache.hadoop.fs.FileStatus;
import org.apache.hadoop.fs.FileSystem;
import org.apache.hadoop.fs.Path;
import org.apache.hadoop.io.Text;

public class SimInputFormat extends FileInputFormat<SimKey, Text> {
    private Log log = LogFactory.getLog(SimInputFormat.class);
    @Override
    public RecordReader<SimKey, Text> createRecordReader(InputSplit split, TaskAttemptContext context) {
        context.setStatus(split.toString());
        return new SimRecordReader();
    }

    @Override
    public List<InputSplit> getSplits(JobContext job) throws IOException {
        List<InputSplit> splits = new ArrayList<InputSplit>();
        List<FileStatus> files = listStatus(job);
        for(FileStatus file : files) {
            int num_mappers = Integer.parseInt(job.getConfiguration().get("mapred.tasktracker.map.tasks.maximum"));
            Path path = file.getPath();
            FileSystem fs = path.getFileSystem(job.getConfiguration());
            long length = file.getLen();
            log.info("Input length: " + length);
            // Need to iterate over all slices of min(length,Int.MAX)
            if (length > Integer.MAX_VALUE) {
                log.warn("File too big to be supported right now.");
            }
            if ((length != 0) && isSplitable(job, path)) {
                log.info("Splitting into at most "+num_mappers+" parts"); 
                BufferedReader input = new BufferedReader(new InputStreamReader(fs.open(path)));
                String line = null;
                String content = "";
                while ((line=input.readLine()) != null){
                    content += line + System.getProperty("line.separator");
                }
                String[] records = content.split("\n");
                int numRecords = records.length;
                log.info("Found " + numRecords + " records for " + file.getPath());
                int numSplits = Math.min(num_mappers, numRecords);
                log.info("Creating " + numSplits + " splits.");
                int offset = 0;
                if(numSplits < num_mappers) {
                    for(String record : records) {
                        String[] hosts = fs.getFileBlockLocations(path, offset, record.length())[0].getHosts();
                        splits.add(new FileSplit(path, (long)offset, (long)record.length(), hosts));
                        log.info("Created split with offset " + offset + " and length " + record.length() + ".");
                        offset += record.length();
                    }
                } else {
                    int recordsPerSplit = numRecords / numSplits; // 17/16 2 per.
                    int processedRecords = 0;
                    int remainder = numRecords % numSplits; // Partial records IE if we have 17 records, we have remainder of 1. So only remainderth splits get recordsPerSplit. Everyone else gets -1.
                    for(int i = 0; i < num_mappers; i++) {
                        int recordsInSplit = recordsPerSplit;
                        if(splits.size() < remainder) {
                            recordsInSplit++;
                        }
                        int splitLength = 0;
                        for(int j = 0; j < recordsInSplit; j++) {
                            splitLength += records[processedRecords + j].length();
                        }
                        processedRecords += recordsInSplit;
                        String[] hosts = fs.getFileBlockLocations(path, offset, splitLength)[0].getHosts();
                        splits.add(new FileSplit(path, (long)offset, (long)splitLength, hosts));
                        log.info("Created split with offset " + offset + " and length " + splitLength + ".");
                        offset += splitLength;
                    }
                }
            } else if (length != 0) {
                log.info("Creating 1 split for unsplittable content.");
                splits.add(new FileSplit(path, 0, length, fs.getFileBlockLocations(path, 0, length)[0].getHosts()));
            } else { 
                //Create empty hosts array for zero length files
                log.info("Nothing to split.");
                splits.add(new FileSplit(path, 0, length, new String[0]));
            }
            job.getConfiguration().setLong(NUM_INPUT_FILES, files.size());
            log.debug("Total # of splits: " + splits.size());
        } 
        return splits;
    }
}