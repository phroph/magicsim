package atmasim.hadoop.wow;

public class SimCharacter {
    String set;
    String level = "110";
    String race = "Troll";
    String role = "spell";
    String character = "priest=\"Atmasim\"";
    String spec = "shadow";
    String ptr;
    String artifacts;
    String talents;
    String crucible;

    String fString = "ptr=%s\noutput=nul\nstrict_work_queue=1\nreport_details=0\n"+
        "%s\nlevel=%s\nrace=%s\nrole=%s\n"+
        "priest_ignore_healing=1\nposition=back\ntalents=%s"+
        "\nartifact=%s\ncrucible=%s\nspec=%s\ndefault_actions=1\n%s\n";

    public SimCharacter(String setString, String artifactString, String crucible, String talents, String ptr) {
        this.set = setString;
        this.crucible = crucible;
        this.artifacts = artifactString;
        this.talents = talents;
        this.ptr = ptr;
    }

    @Override
    public String toString() {
        return String.format(fString, ptr, character, level, race, role, talents, artifacts, crucible, spec, set);
    }
}