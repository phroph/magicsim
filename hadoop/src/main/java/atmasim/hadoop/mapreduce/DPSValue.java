package atmasim.hadoop.mapreduce;

import java.io.IOException;
import java.io.DataOutput;
import java.io.DataInput;

import org.apache.hadoop.io.FloatWritable;
import org.apache.hadoop.io.Text;
import org.apache.hadoop.io.WritableComparable;

public class DPSValue implements WritableComparable<DPSValue> {

    public Text simString;
    public Text reforgeString;
    public FloatWritable dps;

    public DPSValue() {
        simString = new Text();
        reforgeString = new Text();
        dps = new FloatWritable();
    }   
     
    public DPSValue(String sim, String reforge, float dps) {
        this.simString = new Text(sim);
        this.reforgeString = new Text(reforge);
        this.dps = new FloatWritable(dps);
    }

    @Override
    public void write(DataOutput out) throws IOException {
        this.simString.write(out);
        this.reforgeString.write(out);
        this.dps.write(out);
    }

    @Override
    public void readFields(DataInput in) throws IOException {
        this.simString.readFields(in);
        this.reforgeString.readFields(in);
        this.dps.readFields(in);
    }

    @Override
    public int compareTo(DPSValue o) {
        int c1 = this.simString.compareTo(o.simString);
        int c2 = this.reforgeString.compareTo(o.reforgeString);
        int c3 = this.dps.compareTo(o.dps);
        return c1 == 0 ? c2 == 0 ? c3 == 0 ? 0 : c3 : c2 : c1;
    }

    @Override
    public String toString() {
        return this.simString.toString() + ";" + this.reforgeString.toString() + ";" + this.dps.toString();
    }
}