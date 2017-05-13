package atmasim.hadoop.mapreduce;

import java.io.IOException;
import java.io.DataOutput;
import java.io.DataInput;

import org.apache.hadoop.io.WritableComparable;
import org.apache.hadoop.io.Text;

public class SimKey implements WritableComparable<SimKey> {

    public Text simString;
    public Text talentString;

    public SimKey(String sims, String talents) {
        this.simString = new Text(sims);
        this.talentString = new Text(talents);
    }
    
    public void set(byte[] sim, byte[] talent) {
        simString.set(sim);
        talentString.set(talent);
    }

    public void write(DataOutput out) throws IOException {
        this.simString.write(out);
        this.talentString.write(out);
    }

    public void readFields(DataInput in) throws IOException {
        this.simString.readFields(in);
        this.talentString.readFields(in);
    }

    public int compareTo(SimKey o) {
        int c1 = this.simString.compareTo(o.simString);
        int c2 = this.talentString.compareTo(o.talentString);
        return c1 == 0 ? c2 == 0 ? 0 : c2 : c1;
    }
}