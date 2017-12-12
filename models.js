// Adds 7.75875
// Boss 12
// Total 19.925
// Desired Add Contribution = 13.9475
// Desired Boss Contribution = 5.9775
var addsTotal = 7.75875;
var bossTotal = 12;
var percentageAdds = .7;
var total = addsTotal + bossTotal;
var addsDesired = total * percentageAdds;
var bossDesired = total * (1-percentageAdds);
var addScaleFactor = addsDesired/addsTotal/total;
var bossScaleFactor = bossDesired/bossTotal/total;
module.exports.models = [
    {"dispName": "Antorus, the Burning Throne Composite", "name": "ant",
        "model": {
            "heavymovement_ba_st": .2/11,
            "lightmovement_ba_st": .3/11,
            "patchwerk_ba_st": .45/11,
            "heavymovement_na_st": .25/11,
            "lightmovement_na_st": 1.95/11,
            "patchwerk_na_st": 1.9/11,
            "heavymovement_sa_st": 1.95/11,
            "lightmovement_sa_st": .9/11,
            "patchwerk_sa_st": 0.05,
            "heavymovement_ba_2t": 0.15/11,
            "lightmovement_ba_2t": 0.7/11,
            "patchwerk_ba_2t": 0.25/11,
            "lightmovement_na_2t": 0.65/11,
            "patchwerk_na_2t": 0.8/11
        },
        "timeModel": {
            "250": 0.15,
            "400": 0.85
        }
    },
    {"dispName": "Mythic+ Composite", "name": "mplus",
        "model": {
            '30_lightmovement_4_adds': 0.025*addScaleFactor,
            '30_lightmovement_5_adds': 0.1*addScaleFactor,
            '35_lightmovement_3_adds': 0.0625*addScaleFactor,
            '35_lightmovement_4_adds': 0.15*addScaleFactor,
            '35_lightmovement_5_adds': 0.21875*addScaleFactor,
            '40_lightmovement_3_adds': 0.21875*addScaleFactor,
            '40_lightmovement_4_adds': 0.15*addScaleFactor,
            '40_lightmovement_5_adds': 0.3*addScaleFactor,
            '45_lightmovement_3_adds': 0.2875*addScaleFactor,
            '45_lightmovement_4_adds': 0.1375*addScaleFactor,
            '45_lightmovement_5_adds': 0.23125*addScaleFactor,
            '50_lightmovement_3_adds': 0.11875*addScaleFactor,
            '30_patchwerk_4_adds': 0.3*addScaleFactor,
            '30_patchwerk_5_adds': 0.1875*addScaleFactor,
            '35_patchwerk_3_adds': 0.45*addScaleFactor,
            '35_patchwerk_4_adds': 0.65625*addScaleFactor,
            '35_patchwerk_5_adds': 0.65625*addScaleFactor,
            '40_patchwerk_3_adds': 0.45*addScaleFactor,
            '40_patchwerk_4_adds': 0.9*addScaleFactor,
            '40_patchwerk_5_adds': 0.8625*addScaleFactor,
            '45_patchwerk_3_adds': 0.4125*addScaleFactor,
            '45_patchwerk_4_adds': 0.69375*addScaleFactor,
            '45_patchwerk_5_adds': 0.19*addScaleFactor,
            patchwerk_na_st: (13.1275/3)*bossScaleFactor,
            lightmovement_na_st: 2.2625*bossScaleFactor,
            heavymovement_na_st: 0.4375*bossScaleFactor,
            patchwerk_na_2t: (0.8/3)*bossScaleFactor,
            lightmovement_na_2t: (0.47/3)*bossScaleFactor,
            patchwerk_ba_st: (3.1375/3)*bossScaleFactor,
            lightmovement_ba_st: (1.85/3)*bossScaleFactor,
            heavymovement_ba_st: 0.24*bossScaleFactor,
            patchwerk_sa_st: 1.855*bossScaleFactor,
            lightmovement_sa_st: (2.11/3)*bossScaleFactor,
            heavymovement_sa_st: 0.04*bossScaleFactor,
        },
        "timeModel": {
            90: 1.0
        }
    },
    {"dispName": "Mythic+ Trash Composite", "name": "mplus_trash",
        "model": {
            '30_lightmovement_4_adds': 0.025/addsTotal,
            '30_lightmovement_5_adds': 0.1/addsTotal,
            '35_lightmovement_3_adds': 0.0625/addsTotal,
            '35_lightmovement_4_adds': 0.15/addsTotal,
            '35_lightmovement_5_adds': 0.21875/addsTotal,
            '40_lightmovement_3_adds': 0.21875/addsTotal,
            '40_lightmovement_4_adds': 0.15/addsTotal,
            '40_lightmovement_5_adds': 0.3/addsTotal,
            '45_lightmovement_3_adds': 0.2875/addsTotal,
            '45_lightmovement_4_adds': 0.1375/addsTotal,
            '45_lightmovement_5_adds': 0.23125/addsTotal,
            '50_lightmovement_3_adds': 0.11875/addsTotal,
            '30_patchwerk_4_adds': 0.3/addsTotal,
            '30_patchwerk_5_adds': 0.1875/addsTotal,
            '35_patchwerk_3_adds': 0.45/addsTotal,
            '35_patchwerk_4_adds': 0.65625/addsTotal,
            '35_patchwerk_5_adds': 0.65625/addsTotal,
            '40_patchwerk_3_adds': 0.45/addsTotal,
            '40_patchwerk_4_adds': 0.9/addsTotal,
            '40_patchwerk_5_adds': 0.8625/addsTotal,
            '45_patchwerk_3_adds': 0.4125/addsTotal,
            '45_patchwerk_4_adds': 0.69375/addsTotal,
            '45_patchwerk_5_adds': 0.19/addsTotal
        },
        "timeModel": {
        }
    },
    {"dispName": "Garothi Worldbreaker", "name": "garothi",
        "model": {
            "lightmovement_na_st": 0.4,
            "patchwerk_na_st": 0.6,
        },
        "timeModel": {
            "250": 0.15,
            "400": 0.85
        }
    },
    {"dispName": "Felhounds of Sargeras", "name": "hounds",
        "model": {
            "lightmovement_na_2t": 0.4,
            "patchwerk_na_2t": 0.6
        },
        "timeModel": {
            "250": 0.15,
            "400": 0.85
        }
    },
    {"dispName": "Antoran High Command", "name": "command",
        "model": {
            "lightmovement_sa_st": 0.65,
            "patchwerk_sa_st": 0.35,
        },
        "timeModel": {
            "250": 0.15,
            "400": 0.85
        }
    },
    {"dispName": "Portal Keeper Hasabel", "name": "hasabel",
        "model": {
            "heavymovement_na_st": 0.25,
            "lightmovement_na_st": 0.5,
            "patchwerk_na_st": 0.25
        },
        "timeModel": {
            "250": 0.15,
            "400": 0.85
        }
    },
    {"dispName": "Imonar the Soulhunter", "name": "imonar",
        "model": {
            "heavymovement_sa_st": 0.9,
            "lightmovement_sa_st": 0.1
        },
        "timeModel": {
            "250": 0.15,
            "400": 0.85
        }
    },
    {"dispName": "Kin'garoth", "name": "kingaroth",
        "model": {
            "patchwerk_ba_st": 0.1,
            "lightmovement_na_st": 0.2,
            "patchwerk_na_st": 0.15,
            "lightmovement_ba_2t": 0.1,
            "lightmovement_na_2t": 0.25,
            "patchwerk_na_2t": 0.2
        },
        "timeModel": {
            "250": 0.15,
            "400": 0.85
        }
    },
    {"dispName": "Varimathras", "name": "varimathras",
        "model": {
            "lightmovement_ba_st": 0.1,
            "patchwerk_ba_st": 0.2,
            "lightmovement_na_st": 0.3,
            "patchwerk_na_st": 0.4
        },
        "timeModel": {
            "250": 0.15,
            "400": 0.85
        }
    },
    {"dispName": "The Coven of Shivarra", "name": "coven",
        "model": {
            "heavymovement_ba_2t": 0.15,
            "lightmovement_ba_2t": 0.6,
            "patchwerk_ba_2t": 0.25
        },
        "timeModel": {
            "250": 0.15,
            "400": 0.85
        }
    },
    {"dispName": "Aggramar", "name": "aggramar",
        "model": {
            "lightmovement_ba_st": 0.1,
            "patchwerk_ba_st": 0.15,
            "lightmovement_na_st": 0.1,
            "patchwerk_na_st": 0.1,
            "heavymovement_sa_st": 0.2,
            "lightmovement_sa_st": 0.15,
            "patchwerk_sa_st": 0.2
        },
        "timeModel": {
            "250": 0.15,
            "400": 0.85
        }
    },
    {"dispName": "Argus the Unmaker", "name": "argus",
        "model": {
            "lightmovement_ba_st": 0.1,
            "lightmovement_na_st": 0.45,
            "patchwerk_na_st": 0.4,
            "heavymovement_sa_st": 0.05,
        },
        "timeModel": {
            "250": 0.15,
            "400": 0.85
        }
    }
]