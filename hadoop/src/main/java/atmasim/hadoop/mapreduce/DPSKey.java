package atmasim.hadoop.mapreduce;

import java.io.IOException;
import java.io.DataOutput;
import java.io.DataInput;

import org.apache.hadoop.io.Text;
import org.apache.hadoop.io.WritableComparable;

public class DPSKey implements WritableComparable<DPSKey> {

    public Text talentString;
    public Text modelString;

    public DPSKey(String talents, String model) {
        this.talentString = new Text(talents);
        this.modelString = new Text(model);
    }

    public void write(DataOutput out) throws IOException {
        this.talentString.write(out);
        this.modelString.write(out);
    }

    public void readFields(DataInput in) throws IOException {
        this.talentString.readFields(in);
        this.modelString.readFields(in);
    }

    public int compareTo(DPSKey o) {
        int c1 = this.talentString.compareTo(o.talentString);
        int c2 = this.modelString.compareTo(o.modelString);
        return c1 == 0 ? c2 == 0 ? 0 : c2 : c1;
    }
}