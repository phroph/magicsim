package atmasim.hadoop.mapreduce;

import java.io.IOException;
import java.io.DataOutput;
import java.io.DataInput;

import org.apache.hadoop.io.Text;
import org.apache.hadoop.io.WritableComparable;

public class CompositeDPSKey implements WritableComparable<CompositeDPSKey> {

    public Text modelString;
    public Text reforgeString;
    public Text talentString;

    public CompositeDPSKey() {
        modelString = new Text();
        reforgeString = new Text();
        talentString = new Text();
    }
    
    public CompositeDPSKey(String model, String reforge, String talents) {
        this.modelString = new Text(model);
        this.reforgeString = new Text(reforge);
        this.talentString = new Text(talents);
    }

    public void write(DataOutput out) throws IOException {
        this.modelString.write(out);
        this.reforgeString.write(out);
        this.talentString.write(out);
    }

    public void readFields(DataInput in) throws IOException {
        this.modelString.readFields(in);
        this.reforgeString.readFields(in);
        this.talentString.readFields(in);
    }

    public int compareTo(CompositeDPSKey o) {
        int c1 = this.modelString.compareTo(o.modelString);
        int c2 = this.reforgeString.compareTo(o.reforgeString);
        int c3 = this.talentString.compareTo(o.talentString);
        return c1 == 0 ? c2 == 0 ? c3 == 0 ? 0 : c3 : c2 : c1;
    }
}