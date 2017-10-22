package atmasim.hadoop.wow;

public class GearSlot {
    GearSlots slot;
    String id;
    String bonus_id;
    String gems;
    String enchant;
    String relic_id;

    public GearSlot(GearSlots slot, String id) {
        this.slot = slot;
        this.id = "id=" + id;
    }
    public GearSlot(GearSlots slot, String id, String bonus_or_gem, boolean hasGem) {
        this.slot = slot;
        this.id = "id=" + id;
        if(!hasGem) {
            this.bonus_id = "bonus_id=" + bonus_or_gem;
        } else {
            this.gems = "gems=" + bonus_or_gem;
        }
    }
    public GearSlot(GearSlots slot, String id, String bonus_id, String gems) {
        this.slot = slot;
        this.id = "id=" + id;
        this.bonus_id = "bonus_id=" + bonus_id;
        this.gems = "gems=" + gems;
    }
    public GearSlot(GearSlots slot, String id, String bonus_or_gem, String enchant, boolean hasGem) {
        this.slot = slot;
        this.id = "id=" + id;
        if(!hasGem) {
            this.bonus_id = "bonus_id=" + bonus_or_gem;
        } else {
            this.gems = "gems=" + bonus_or_gem;
        }
        this.enchant = "enchant=" + enchant;
    }
    public GearSlot(GearSlots slot, String id, String bonus_id, String gems, String enchant_or_relic, boolean hasRelic) {
        this.slot = slot;
        this.id = "id=" + id;
        this.bonus_id = "bonus_id=" + bonus_id;
        this.gems = "gems=" + gems;
        if(hasRelic) {
            this.relic_id = "relic_id=" + enchant_or_relic;
        }
        else {
            this.enchant = "enchant=" + enchant_or_relic;
        }
    }

    @Override
    public String toString() {
        String fString = "%s=,%s";
        int optCount = 2;
        if(bonus_id!=null && !bonus_id.isEmpty()) {
            fString += ",%s";
            optCount++;
        }
        if(gems!=null && !gems.isEmpty()) {
            fString += ",%s";
            optCount++;
        }
        if(enchant!=null && !enchant.isEmpty()) {
            fString += ",%s";
            optCount++;
        }
        if(relic_id!=null && !relic_id.isEmpty()) {
            fString += ",%s";
            optCount++;
        }
        Object[] options = new String[optCount];
        int it = 0;
        options[it++] = slot.toString().toLowerCase();
        options[it++] = id;
        if(bonus_id!=null && !bonus_id.isEmpty()) {
            options[it] = bonus_id;
            it++;
        }
        if(gems!=null && !gems.isEmpty()) {
            options[it] = gems;
            it++;
        }
        if(enchant!=null && !enchant.isEmpty()) {
            options[it] = enchant;
            it++;
        }
        if(relic_id!=null && !relic_id.isEmpty()) {
            options[it] = relic_id;
            it++;
        }
        return String.format(fString, options);
    }
}