package atmasim.hadoop.mapreduce;

import org.apache.hadoop.conf.Configuration;
import org.apache.hadoop.fs.Path;
import org.apache.hadoop.io.FloatWritable;
import org.apache.hadoop.io.Text;
import org.apache.hadoop.mapreduce.Job;
import org.apache.hadoop.mapreduce.lib.output.FileOutputFormat;
import org.apache.hadoop.mapreduce.lib.output.LazyOutputFormat;
import org.apache.hadoop.mapreduce.lib.output.TextOutputFormat;
import org.apache.log4j.Logger;


public class AtmaSim {

  // The flow is such
  // Get a list of sims, run them all and convert them into dps datapoints
  // organize them by model. And for each model:
  // Generate a list of dps data points for each talent.
  // AKA this is partitioned by model and grouped by (talent x partition). The data points are reforge (cmh), dps, and simulation
  private static Logger logger = Logger.getLogger(AtmaSim.class);
  public static int NUMBER_OF_RECORDS = 0;
  public static void main(String[] args) throws Exception {
    Configuration conf = new Configuration();
    Job job = Job.getInstance(conf, "atmasim");
    
    NUMBER_OF_RECORDS = Integer.parseInt(args[2]);
    job.setJarByClass(AtmaSim.class);
    logger.info("Starting AtmaSim driver.");
    // IN: simkey:text -map-> dpskey:dpsvalue -reduce-> compositedpskey:float
    //     (simstring,talentstring):reforgestring -map-> (talentstring,modelstring):(sim,reforge,dps) 
    //     -partition(by key)-> -reduce-> (modelString,talentString,reforgeString):mergedDps
    
    job.setInputFormatClass(SimInputFormat.class); 
    job.setMapperClass(SimMapper.class);
    job.setMapOutputKeyClass(DPSKey.class);
    job.setMapOutputValueClass(DPSValue.class);
    job.setPartitionerClass(ModelPartitioner.class);
    job.setReducerClass(DPSReducer.class);
    job.setOutputKeyClass(FloatWritable.class);
    job.setOutputValueClass(ReducedDPSValue.class);
    job.setOutputFormatClass(FileOutputFormat.class);
    LazyOutputFormat.setOutputFormatClass(job, TextOutputFormat.class);
    SimInputFormat.addInputPath(job, new Path(args[0]));
    FileOutputFormat.setOutputPath(job, new Path(args[1]));

    System.exit(job.waitForCompletion(true) ? 0 : 1);
  }
}