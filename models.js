var totalScaleFactor = (1.5*7.91) + (7.97/1.5);
var addScaleFactor = (1.5)/(totalScaleFactor);
var bossScaleFactor = 1/(1.5*totalScaleFactor);

module.exports.models = [
    {"dispName": "Mythic+ Composite", "name": "mythicplus",
        "model": {
            '30_lowmovement_4_adds': 0.1*addScaleFactor,
            '30_lowmovement_5_adds': 0.06*addScaleFactor,
            '50_lowmovement_3_adds': 0.15*addScaleFactor,
            '50_lowmovement_4_adds': 0.22*addScaleFactor,
            '50_lowmovement_5_adds': 0.22*addScaleFactor,
            '55_lowmovement_3_adds': 0.15*addScaleFactor,
            '55_lowmovement_4_adds': 0.3*addScaleFactor,
            '55_lowmovement_5_adds': 0.29*addScaleFactor,
            '60_lowmovement_3_adds': 0.14*addScaleFactor,
            '60_lowmovement_4_adds': 0.23*addScaleFactor,
            '60_lowmovement_5_adds': 0.12*addScaleFactor,
            '30_patchwerk_4_adds': 0.3*addScaleFactor,
            '30_patchwerk_5_adds': 0.19*addScaleFactor,
            '50_patchwerk_3_adds': 0.45*addScaleFactor,
            '50_patchwerk_4_adds': 0.66*addScaleFactor,
            '50_patchwerk_5_adds': 0.66*addScaleFactor,
            '55_patchwerk_3_adds': 0.45*addScaleFactor,
            '55_patchwerk_4_adds': 0.9*addScaleFactor,
            '55_patchwerk_5_adds': 0.86*addScaleFactor,
            '60_patchwerk_3_adds': 0.41*addScaleFactor,
            '60_patchwerk_4_adds': 0.69*addScaleFactor,
            '60_patchwerk_5_adds': 0.36*addScaleFactor,
            patchwerk_ba_st: 0.33*bossScaleFactor,
            patchwerk_na_st: 1.43*bossScaleFactor,
            lowmovement_ba_st: 1.86*bossScaleFactor,
            lowmovement_na_2t: 0.58*bossScaleFactor,
            lowmovement_na_st: 1.2*bossScaleFactor,
            highmovement_ba_st: 0.2*bossScaleFactor,
            highmovement_na_2t: 0.2*bossScaleFactor,
            highmovement_na_st: 2.17*bossScaleFactor
        },
        "timeModel": {
            90: 1
        }
    },
    {   "dispName": "Skorpyron", "name": "skorpyron",
        "model": {
            patchwerk_sa_2t: 0.1,
            patchwerk_sa_st: 0.1,
            lowmovement_sa_st: 0.55,
            lowmovement_na_2t: 0.25
        },
        "timeModel": {
            250: .15,
            400: .85
        }
    },
    {"dispName": "Chronomatic Anomaly", "name": "anomaly",
        "model": {
            lowmovement_ba_st: 0.2,
            highmovement_ba_st: 0.8
        },
        "timeModel": {
            250: .15,
            400: .85
        }
    },
    {"dispName": "Trilliax", "name": "trilliax",
        "model": {
            patchwerk_ba_st: 0.3,
            lowmovement_ba_st: 0.7
        },
        "timeModel": {
            250: .15,
            400: .85
        }
    },
    {"dispName": "Spellblade Aluriel", "name": "aluriel",
        "model": {
            patchwerk_ba_2t: 0.1,
            patchwerk_ba_st: 0.25,
            lowmovement_ba_2t: 0.65
        },
        "timeModel": {
            250: .15,
            400: .85
        }
    },
    {"dispName": "Tichondrius", "name": "tichondrius",
        "model": {
            lowmovement_sa_st: 0.65,
            highmovement_sa_st: 0.35
        },
        "timeModel": {
            250: .15,
            400: .85
        }
    },
    {"dispName": "High Botanist Tel'arn", "name": "botanist",
        "model": {
            lowmovement_ba_2t: 0.6,
            lowmovement_sa_2t: 0.25,
            highmovement_sa_2t: 0.15
        },
        "timeModel": {
            250: .15,
            400: .85
        }
    },
    {"dispName": "Star Augur Etraeus", "name": "augur",
        "model": {
            lowmovement_ba_st: 0.35,
            lowmovement_na_st: 0.65
        },
        "timeModel": {
            250: .15,
            400: .85
        }
    },
    {"dispName": "Krosus", "name": "krosus",
        "model": {
            patchwerk_sa_st: 0.4,
            patchwerk_na_st: 0.6,
        },
        "timeModel": {
            250: .15,
            400: .85
        }
    },
    {"dispName": "Grand Magistrix Elisande", "name": "elisande",
        "model": {
            lowmovement_ba_2t: 0.3,
            lowmovement_ba_st: 0.6,
            lowmovement_na_2t: 0.1
        },
        "timeModel": {
            250: .15,
            400: .85
        }
    },
    {"dispName": "Gul'dan", "name": "guldan",
        "model": {
            patchwerk_ba_2t: 0.15,
            patchwerk_ba_st: 0.15,
            patchwerk_sa_2t: 0.1,
            patchwerk_sa_st: 0.2,
            patchwerk_na_st: 0.1,
            lowmovement_ba_st: 0.1,
            lowmovement_na_2t: 0.2
        },
        "timeModel": {
            250: .15,
            400: .85
        }
    },
    {"dispName": "Nighthold Composite", "name": "nighthold",
        "model": {
            patchwerk_ba_2t: 0.025,
            patchwerk_ba_st: 0.07,
            patchwerk_sa_2t: 0.02,
            patchwerk_sa_st: 0.07,
            patchwerk_na_st: 0.07,
            lowmovement_ba_2t: 0.155,
            lowmovement_ba_st: 0.195,
            lowmovement_sa_2t: 0.05,
            lowmovement_sa_st: 0.12,
            lowmovement_na_2t: 0.03,
            lowmovement_na_st: 0.065,
            highmovement_ba_st: 0.08,
            highmovement_sa_2t: 0.015,
            highmovement_sa_st: 0.035
        },
        "timeModel": {
            250: .15,
            400: .85
        }
    }
]

module.exports.version = 1.1;