package atmasim.hadoop.mapreduce;

import java.io.IOException;
import java.io.DataOutput;
import java.io.DataInput;

import org.apache.hadoop.io.Text;
import org.apache.hadoop.io.WritableComparable;

public class DPSKey implements WritableComparable<DPSKey> {

    public Text talentString;
    public Text modelString;
    public Text legendaries;
    public Text relics;
    public Text crucible;

    public DPSKey() {
        modelString = new Text();
        talentString = new Text();
        legendaries = new Text();
        relics = new Text();
        crucible = new Text();
    }

    public DPSKey(String talents, String model, String legendaries, String relics, String crucible) {
        this.talentString = new Text(talents);
        this.modelString = new Text(model);
        this.legendaries = new Text(legendaries);
        this.relics = new Text(relics);
        this.crucible = new Text(crucible);
    }

    @Override
    public void write(DataOutput out) throws IOException {
        this.talentString.write(out);
        this.modelString.write(out);
        this.legendaries.write(out);
        this.relics.write(out);
        this.crucible.write(out);
    }

    @Override
    public void readFields(DataInput in) throws IOException {
        this.talentString.readFields(in);
        this.modelString.readFields(in);
        this.legendaries.readFields(in);
        this.relics.readFields(in);
        this.crucible.readFields(in);
    }

    @Override
    public int compareTo(DPSKey o) {
        int c1 = this.talentString.compareTo(o.talentString);
        int c2 = this.modelString.compareTo(o.modelString);
        int c3 = this.legendaries.compareTo(o.legendaries);
        int c4 = this.relics.compareTo(o.relics);
        int c5 = this.crucible.compareTo(o.crucible);
        return c1 == 0 ? c2 == 0 ? c3 == 0 ?  c4 == 0 ? c5 == 0 ? 0 : c5 : c4 : c3 : c2 : c1;
    }

    @Override
    public String toString() {
        return this.talentString.toString() + ";" + this.modelString.toString() + this.legendaries.toString() + ";" + this.relics.toString() + ";" + this.crucible.toString();
    }
}