package atmasim.hadoop.mapreduce;

import org.apache.hadoop.conf.Configuration;
import org.apache.hadoop.fs.Path;
import org.apache.hadoop.io.IntWritable;
import org.apache.hadoop.io.Text;
import org.apache.hadoop.mapreduce.Job;
import org.apache.hadoop.mapreduce.lib.input.FileInputFormat;
import org.apache.hadoop.mapreduce.lib.output.FileOutputFormat;

public class AtmaSim {

  // The flow is such
  // Get a list of sims, run them all and convert them into dps datapoints
  // organize them by model. And for each model:
  // Generate a list of dps data points for each talent.
  // AKA this is partitioned by model and grouped by talent. The data points are reforge (cmh), dps, and simulation
  public static void main(String[] args) throws Exception {
    Configuration conf = new Configuration();
    Job job = Job.getInstance(conf, "atmasim");
    job.setJarByClass(AtmaSim.class);
    job.setMapOutputKeyClass(DPSKey.class);
    job.setMapOutputValueClass(DPSValue.class);
    // Partition to reducer by model
    job.setPartitionerClass(ModelPartitioner.class);
    // ((talent: sim reforge) -> (talent model: sim reforge dps)) for each model that sim belongs to
    job.setMapperClass(SimMapper.class);
    // ((talent model: sim reforge dps) -> (c h m dps)) output for each model x talent
    job.setReducerClass(DPSReducer.class);
    job.setOutputKeyClass(Text.class);
    job.setOutputValueClass(IntWritable.class);
    // TODO: Customize output format to break output by Model then by talent.
    FileInputFormat.addInputPath(job, new Path(args[0]));
    FileOutputFormat.setOutputPath(job, new Path(args[1]));
    System.exit(job.waitForCompletion(true) ? 0 : 1);
  }
}