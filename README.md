# magicsim
Shadow priest tailored tool for simming stat weights in a fight-aware manner using SimulationCraft. Please note that although this tool is specifically designed by and for shadow priests, the methodology is extensible to other classes and depends on the quality of your class/specialization's simc profiles. The methodology used for this tool was borrowed from the How2Priest.com community.

How to run sims and analyze: "node run.js `<region>` `<realm>` `<name>`"

How to analyze (say if you have run sims and want to adjust weights): "node analyze.js"

Note: You must have 7zip and Node.js installed and in your PATH (see StackOverflow/Google for more information) for this tool to properly work. If you do not have 7zip in your PATH, you will most likely see the error "undefined" when running node run.js.

*Big disclaimer that follows all simulated stat weights: Please take me with a grain of salt. Trust and verify. These are made with the same methodology as the sims that H2P runs for pinned resources, but it doesn't make these results infallible. Use common sense and your brain with this tool.*

**Another disclaimer: I wrote the simc templates myself based on what I believe is the same simc templates (roughly) as the ones H2P uses. It is possible that these templates may give slightly different results and may influence weights. Again, use your brain, verify against pinned information. This tool is only an assistant that does a very specific task. It only gives you what it thinks is the best stat weights for your current gear/talent setup assuming a synthesized "average" nighthold raid.**

Special thanks to Djriff, Anshlun, N1gh7h4wk, Publik, the simc developers, and the general H2P community.
