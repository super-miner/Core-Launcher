![Core Launcher](Banner.png)

# CoreLauncher
This is a custom game launcher made for the game Core Keeper. It uses the Mod IO API so you will need a Mod IO account. If you have any questions please contact me at `Core Keeper Discord Server > Modding > Available Mods > Core Launcher` or dm me at `super_miner_1`.

# Info for Mod Developers
From v1.1.0 onwards there will be the option to link things like donation links so that they show up in the launcher. Please read [this](Info/UsingExtraData.md) for more info.

# Why Godot?
### Godot is a game engine, so why am I using it for this project?
Godot provides lots of useful features -- especially for ui. Godot is also very lightweight for a game engine which makes it a good choice for this project.

# Roadmap/TODO
## ~~v1.1.0~~
- ~~Create system for mods to provide extra data in their descriptions~~
- ~~Allow use with the Core Keeper Dedicated Server~~
- ~~Add a button to toggle the options screen~~
- ~~Stop mods that have their security checks off from being loaded~~

## v1.2.0
- Manage the edge case of Core Keeper not being installed in the Steam path
- Custom background image
- Ask user to name their profile after pressing play if it remains unnamed
- Include the mod and game configs in the profile specific info

## Beyond
- Add option to view mods in a grid instead of a list
- Automatic updates (to the launcher)
- Menu for creating backups of the game / your world, character, etc. files
- More customizability
  - Custom themes
- More settings
  - Edit game and mod configs from the launcher
  - Rename characters and worlds from the launcher
- Faster installations (these things need testing, I'm not sure if they'll work)
  - Faster client mod installations by changing state.json
  - Faster dedicated server installations by only installing/uninstalling the modmanifest.json file
- Cache mod logos
- Asynchronous fetching
- A search bar for the mods list
