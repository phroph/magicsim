package atmasim.hadoop.mapreduce;
enum Models {
    UNIVERSAL(0), MYTHICPLUS(1), NIGHTHOLD(2), GULDAN(3), ELISANDE(4), KROSUS(5), TICHONDRIUS(6), TELARN(7), ETRAEUS(8), ALURIEL(9), TRILLIAX(10), ANOMALY(11), SKORPYRON(12);

    public static final int reducerCount = 13;
    private int partitionNumber;
    private Models(int partition) {
        this.partitionNumber = partition;
    }

    public static int getPartitionForModel(Models model) {
        return model.partitionNumber;
    }

    public static Models getModelByNumber(int number) {
        switch(number) {
            case 0:
                return UNIVERSAL;
            case 1:
                return MYTHICPLUS;
            case 2:
                return NIGHTHOLD;
            case 3:
                return GULDAN;
            case 4:
                return ELISANDE;
            case 5:
                return KROSUS;
            case 6:
                return TICHONDRIUS;
            case 7:
                return TELARN;
            case 8:
                return ETRAEUS;
            case 9:
                return ALURIEL;
            case 10:
                return TRILLIAX;
            case 11:
                return ANOMALY;
            case 12:
                return SKORPYRON;
            default:
                return UNIVERSAL;
        }
    }
    public static Models getModelByName(String name) {
        switch(name) {
            case "universal":
                return UNIVERSAL;
            case "mythicplus":
                return MYTHICPLUS;
            case "nighthold":
                return NIGHTHOLD;
            case "guldan":
                return GULDAN;
            case "elisande":
                return ELISANDE;
            case "krosus":
                return KROSUS;
            case "tichondrius":
                return TICHONDRIUS;
            case "telarn":
                return TELARN;
            case "etraeus":
                return ETRAEUS;
            case "aluriel":
                return ALURIEL;
            case "trilliax":
                return TRILLIAX;
            case "anomaly":
                return ANOMALY;
            case "skorpyron":
                return SKORPYRON;
            default:
                return UNIVERSAL;
        }
    }
}