package atmasim.hadoop.wow;

public class GearSet {
    GearSlot head;
    GearSlot neck;
    GearSlot shoulders;
    GearSlot back;
    GearSlot chest;
    GearSlot wrists;
    GearSlot hands;
    GearSlot waist;
    GearSlot legs;
    GearSlot feet;
    GearSlot finger1;
    GearSlot finger2;
    GearSlot trinket1;
    GearSlot trinket2;
    GearSlot main_hand;
    GearSlot off_hand;
    String bonusString;

    // Override Properties
    boolean manualScaling;
    String ilvl;
    String versatility;
    String intellect;
    String critical_strike;
    String haste;
    String mastery;

    public GearSet(GearSlot head, GearSlot neck, GearSlot shoulders,
        GearSlot back, GearSlot chest, GearSlot wrists,
        GearSlot hands, GearSlot waist, GearSlot legs, 
        GearSlot feet, GearSlot finger1, GearSlot finger2, 
        GearSlot trinket1, GearSlot trinket2, GearSlot main_hand, 
        GearSlot off_hand, String bonusString) {
        this.head = head;
        this.neck = neck;
        this.shoulders = shoulders;
        this.back = back;
        this.chest = chest;
        this.wrists = wrists;
        this.hands = hands;
        this.waist = waist;
        this.legs = legs;
        this.feet = feet;
        this.finger1 = finger1;
        this.finger2 = finger2;
        this.trinket1 = trinket1;
        this.trinket2 = trinket2;
        this.main_hand = main_hand;
        this.off_hand = off_hand;
        this.bonusString = bonusString;
        manualScaling = false;
    }

    public void enforceGearProperties(String ilvl, String vers, String intellect,
        String crit, String haste, String mastery) {
        manualScaling = true;
        versatility = vers;
        this.intellect = intellect;
        critical_strike = crit;
        this.haste = haste;
        this.mastery = mastery;
    }

    @Override
    public String toString() {
        String fString = "%s\n%s\n%s\n%s\n%s\n%s\n%s\n%s\n%s\n%s\n%s\n%s\n%s\n%s\n%s\n%s\n%s\n";
        int numOpts = 17;
        if(manualScaling) {
            fString += "scale_to_itemlevel=%s\ngear_versatility=%s\n"+
                "gear_intellect=%s\ngear_crit_rating=%s\ngear_haste_rating=%s\n"+
                "gear_mastery_rating=%s\n";
            numOpts += 6;
        }
        Object[] options = new String[numOpts];
        int iterator = 0;
        options[iterator++] = head.toString();
        options[iterator++] = neck.toString();
        options[iterator++] = shoulders.toString();
        options[iterator++] = back.toString();
        options[iterator++] = chest.toString();
        options[iterator++] = wrists.toString();
        options[iterator++] = hands.toString();
        options[iterator++] = waist.toString();
        options[iterator++] = legs.toString();
        options[iterator++] = feet.toString();
        options[iterator++] = finger1.toString();
        options[iterator++] = finger2.toString();
        options[iterator++] = trinket1.toString();
        options[iterator++] = trinket2.toString();
        options[iterator++] = main_hand.toString();
        options[iterator++] = off_hand.toString();
        options[iterator++] = head.toString();
        if(manualScaling) {
            options[iterator++] = ilvl;
            options[iterator++] = versatility;
            options[iterator++] = intellect;
            options[iterator++] = critical_strike;
            options[iterator++] = haste;
            options[iterator++] = mastery;
        }
        return String.format(fString, options);
    }
}