var totalScaleFactor = (1.5*7.91) + (7.97/1.5);
var addScaleFactor = (1.5)/(totalScaleFactor);
var bossScaleFactor = 1/(1.5*totalScaleFactor);

module.exports.models = [
    {"dispName": "Tomb of Sargeras Composite", "name": "tos",
        "model": {
            "patchwerk_sa_st": 0.06875,
            "patchwerk_na_st": 0.125,
            "lightmovement_ba_st": 0.05,
            "lightmovement_sa_st": 0.1375,
            "lightmovement_sa_2t": 0.06875,
            "lightmovement_na_st": 0.3,
            "lightmovement_na_2t": 0.05,
            "heavymovement_na_st": 0.14375,
            "heavymovement_na_2t": 0.01875,
            "heavymovement_sa_st": 0.0375
        },
        "timeModel": {
            "250": 0.15,
            "400": 0.85
        }
    },
        {"dispName": "Nighthold Composite", "name": "nh",
        "model": {
            patchwerk_ba_2t: 0.025,
            patchwerk_ba_st: 0.07,
            patchwerk_sa_2t: 0.02,
            patchwerk_sa_st: 0.07,
            patchwerk_na_st: 0.07,
            lightmovement_ba_2t: 0.155,
            lightmovement_ba_st: 0.195,
            lightmovement_sa_2t: 0.05,
            lightmovement_sa_st: 0.12,
            lightmovement_na_2t: 0.03,
            lightmovement_na_st: 0.065,
            heavymovement_ba_st: 0.08,
            heavymovement_sa_2t: 0.015,
            heavymovement_sa_st: 0.035
        },
        "timeModel": {
            250: .15,
            400: .85
        }
    },
    {"dispName": "Mythic+ Composite", "name": "mplus",
        "model": {
            '30_lightmovement_4_adds': 0.1*addScaleFactor,
            '30_lightmovement_5_adds': 0.06*addScaleFactor,
            '50_lightmovement_3_adds': 0.15*addScaleFactor,
            '50_lightmovement_4_adds': 0.22*addScaleFactor,
            '50_lightmovement_5_adds': 0.22*addScaleFactor,
            '55_lightmovement_3_adds': 0.15*addScaleFactor,
            '55_lightmovement_4_adds': 0.3*addScaleFactor,
            '55_lightmovement_5_adds': 0.29*addScaleFactor,
            '60_lightmovement_3_adds': 0.14*addScaleFactor,
            '60_lightmovement_4_adds': 0.23*addScaleFactor,
            '60_lightmovement_5_adds': 0.12*addScaleFactor,
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
            lightmovement_ba_st: 1.86*bossScaleFactor,
            lightmovement_na_2t: 0.58*bossScaleFactor,
            lightmovement_na_st: 1.2*bossScaleFactor,
            heavymovement_ba_st: 0.2*bossScaleFactor,
            heavymovement_na_2t: 0.2*bossScaleFactor,
            heavymovement_na_st: 2.17*bossScaleFactor
        },
        "timeModel": {
            400: .85
        }
    },
    {"dispName": "Goroth", "name": "goroth",
        "model": {
            "patchwerk_na_st": 0.2,
            "lightmovement_na_st": 0.6,
            "heavymovement_na_st": 0.2
        },
        "timeModel": {
            "250": 0.15,
            "400": 0.85
        }
    },
    {"dispName": "Demonic Inquisition", "name": "inquisition",
        "model": {
            "lightmovement_sa_2t": 0.55,
            "lightmovement_na_2t": 0.35,
            "heavymovement_na_st": 0.15
        },
        "timeModel": {
            "250": 0.15,
            "400": 0.85
        }
    },
    {"dispName": "Harjatan", "name": "harjatan",
        "model": {
            "patchwerk_sa_st": 0.2,
            "patchwerk_na_st": 0.1,
            "lightmovement_ba_st": 0.2,
            "lightmovement_sa_st": 0.3,
            "heavymovement_sa_st": 0.2
        },
        "timeModel": {
            "250": 0.15,
            "400": 0.85
        }
    },
    {"dispName": "Mistress Sassz'ine", "name": "mistress",
        "model": {
            "patchwerk_sa_st": 0.15,
            "lightmovement_sa_st": 0.3,
            "lightmovement_na_st": 0.35,
            "heavymovement_na_st": 0.2
        },
        "timeModel": {
            "250": 0.15,
            "400": 0.85
        }
    },
    {"dispName": "Desolate Host", "name": "host",
        "model": {
            "patchwerk_sa_st": 0.2,
            "lightmovement_sa_st": 0.5,
            "lightmovement_na_st": 0.2,
            "heavymovement_sa_st": 0.1
        },
        "timeModel": {
            "250": 0.15,
            "400": 0.85
        }
    },
    {"dispName": "Maiden of Vigilance", "name": "maiden",
        "model": {
            "patchwerk_na_st": 0.5,
            "lightmovement_na_st": 0.3,
            "heavymovement_na_st": 0.2
        },
        "timeModel": {
            "250": 0.15,
            "400": 0.85
        }
    },
    {"dispName": "Sisters of the Moon", "name": "sisters",
        "model": {
            "patchwerk_na_st": 0.1,
            "lightmovement_ba_st": 0.2,
            "lightmovement_na_st": 0.45,
            "heavymovement_na_st": 0.25
        },
        "timeModel": {
            "250": 0.15,
            "400": 0.85
        }
    },
    {"dispName": "Fallen Avatar", "name": "avatar",
        "model": {
            "patchwerk_na_st": 0.1,
            "lightmovement_na_st": 0.5,
            "heavymovement_na_st": 0.2
        },
        "timeModel": {
            "250": 0.15,
            "400": 0.85
        }
    }
]