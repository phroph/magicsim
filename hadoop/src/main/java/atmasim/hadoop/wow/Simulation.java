package atmasim.hadoop.wow;

public class Simulation {
    SimCharacter character;
    String simTime;
    String simFight;
    String threads;
    String addString;
    String targetString;
    String profileName;
    //header + gear + base + targetString + addString + footer;
    String fString = "%s\n%s\n%s\n%s\n%s\n";
    String fBaseString = "iterations=2000\nthreads=%s\nmax_time=%s\noptimal_raid=1\nfight_style=%s\nenemy=enemy1\n";

    public Simulation(SimCharacter character, String time, String fight, String threads, String adds, String targets, String profile) {
        this.character = character;
        this.simTime = time;
        this.simFight = fight;
        this.threads = threads;
        this.addString = adds;
        this.targetString = targets;
        this.profileName = profile;
    }

    @Override
    public String toString() {
        return String.format(fString, character.toString(), String.format(fBaseString, threads, simTime, simFight), targetString, addString,  "json2=" + profileName + ".json\n");
    }
}