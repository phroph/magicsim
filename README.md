# magicsim
Shadow priest tailored tool for simming stat weights in a fight-aware manner using SimulationCraft. *Now completely re-written in native application code*

Magicsim is a simming application that I originally created to help me replicate HowToPriest's composite simming easily so I could sim my own characters and have decent stat weights. Since then, it's changed drastically from my initial vision. Originally, Magicsim was a little console script that I gave my character armory link and it spat out a Pawn string to output. Since releasing it, I've learned a lot about the different ways people sim, and the kind of functionality that makes simming the smoothest. The old magicsim code was simply not designed with some of the needed features in mind and had a lot of issues with false flagging anti virus programs. So, a year later it became time to completely redesign it from the bottom up and support a vastly new feature base.

Magicsim is now a C# WPF application which supports x32 and x64 Windows. It comes pre-packaged with 7zip as before, but completely replaces the Node.js engine with a processing pipeline built into the WPF application.

The new Magicsim provides the following features and is supported *for all classes/specs*:
- Support for character inputs from Armory, SimC profiles (IE /simc), AutoSimc output (IE a string of profiles separated by 1 newline)
- Support for multiple characters simmed per fight.
- Ability to directly modify each uploaded character's name, crucible, ilvl, tiersets, and raid buffs.
- Ability to control basic sim parameters like PTR mode, stat weights toggles, which composite model to use, threads per sim, and concurrent simc processes per run.
- Ability to control advanced parameters like simulating raid pantheon trinkets and generating reforge plots (no support for viewing yet)
- Results are now persisted and can be named with tags for reference and loaded at a later time.
- When multiple characters are simmed, % differences are shown (compared against the minimum dps)
- HTML results are also generated now for manual consumption of individual sims (these are associated with the guid in the original tag
- Magicsim auto-updates itself *and* SimulationCraft now.

The new Magicsim should be a much more seamless experience and should be future proof going into Battle for Azeroth and will continue to serve your simming needs, better and better.


It goes without saying but... Special thanks to Djriff, Anshlun, N1gh7h4wk, Publik, the rest of Shadow Team (and the greater HowToPriest staff), the simc developers, the general H2P community, and really anyone who uses this. None of this would be possible without all of your help and support (as well as bug reports and feature suggestions).
