# magicsim
Shadow priest tailored tool for simming stat weights in a fight-aware manner using SimulationCraft. Please note that although this tool is specifically designed by and for shadow priests, the methodology is extensible to other classes and depends on the quality of your class/specialization's simc profiles. The methodology used for this tool was borrowed from the How2Priest.com community.

**There is a beta UI in works that runs as an executable on Windows. It is technically ready-to-go and can be built as-is using nw-builder, however it needs some TLC before it is ready for prime-time. It will be available under Releases *soon****

How to run sims and analyze: ```node run.js <region> <realm> <name>```

How to analyze (say if you have run sims and want to adjust weights): `node analyze.js`

Note: This has a portable, self-bootstrapping version of node. If you cannot run the scripts using this, delete node.cmd, install Node.js locally on your machine and add it to PATH. Run `npm install` from the magicsim directory and then the commands should run as usual.

**Tests:**

This tool has a mode where you can run it in verification mode. It uses the same nighthold ilvl 905 4set gear model that the H2P sims and generates stat weights and shows you how close they compare to the H2P raid stat weights posted for 12k haste SL 4set. I ran it and it was within acceptable parameters of closeness (<=.01 delta for all values typically), so you can use this to also verify if the simming tool is working properly or is accurate.

Run it as such:

`node run.js sim_test`

or after running sims (or to compare your own sims against H2P 12k haste SL 4set):

`node analyze.js sim_test`

**This script only runs on Windows**

*Big disclaimer that follows all simulated stat weights: Please take me with a grain of salt. Trust and verify. These are made with the same methodology as the sims that H2P runs for pinned resources, but it doesn't make these results infallible. Use common sense and your brain with this tool.*

**Another disclaimer: I wrote the simc templates myself based on what I believe is the same simc templates (roughly) as the ones H2P uses. It is possible that these templates may give slightly different results and may influence weights. Again, use your brain, verify against pinned information. This tool is only an assistant that does a very specific task. It only gives you what it thinks is the best stat weights for your current gear/talent setup assuming a synthesized "average" nighthold raid.**

Special thanks to Djriff, Anshlun, N1gh7h4wk, Publik, the simc developers, and the general H2P community.
