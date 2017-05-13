package atmasim.hadoop.mapreduce;

import java.io.IOException;
import java.io.DataOutput;
import java.io.DataInput;

import org.apache.hadoop.io.FloatWritable;
import org.apache.hadoop.io.Text;
import org.apache.hadoop.io.WritableComparable;

public class ReducedDPSValue implements WritableComparable<DPSValue> {

    public Text reforgeString;
    public FloatWritable dps;

    public ReducedDPSValue(String reforge, float dps) {
        this.reforgeString = new Text(reforge);
        this.dps = new FloatWritable(dps);
    }

    public void write(DataOutput out) throws IOException {
        this.reforgeString.write(out);
        this.dps.write(out);
    }

    public void readFields(DataInput in) throws IOException {
        this.reforgeString.readFields(in);
        this.dps.readFields(in);
    }

    public int compareTo(DPSValue o) {
        int c1 = this.reforgeString.compareTo(o.reforgeString);
        int c2 = this.dps.compareTo(o.dps);
        return c1 == 0 ? c2 == 0 ? 0 : c2 : c1;
    }
}