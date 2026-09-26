# Chihuahua Champ

You play a small chihuahua trying to become a powerlifting champion by mashing keys to lift heavier and heavier weights.

- Play: [itch.io](https://daniel-narvaez.itch.io/chihuahua-champ)
- Made: April to May 2024. We started it for Gamedev.js Jam 2024.
- Team: [@mfclinton](https://github.com/mfclinton) (programming), [@daniel-narvaez](https://github.com/daniel-narvaez) (game design), [@MrAozora](https://github.com/MrAozora) (music), [regatton](https://regatton.itch.io) (2D art), [Frog Moss Games](https://frogmossgames.itch.io) (UI and UX art), [theleonelrojas](https://www.instagram.com/theleonelrojas) (sound)
- Engine: Unity, C#

This is an export of a private repo with only the code we wrote. Art, audio, the Unity project files, and third party plugins aren't included. The history is squashed into one commit.

## What I built

- The mashing minigame. It times your left and right key presses to tell alternating apart from pressing both at once, and each exercise only counts the style it asks for.
- The keys you mash are random, and competitions give you new ones every rep. Only the input binding changes, and the rest of the game never notices. The hints on screen blink to show you the rhythm.
- I animated the dog's lift in code. Its joints move between two poses as the rep fills up, and some Perlin noise makes it shake like it's straining.
- The menu wheel, which spreads the menus out in an arc and moves the camera over to the one you pick.
