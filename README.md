![Core Launcher](Banner.png)

# CoreLauncher
This is a custom game launcher made for the game Core Keeper.

# Info for Mod Developers (CL_Data)
From v1.1.0 onwards there is an option to link things like donation pagess so that they show up in the launcher. Please read [this](Info/UsingExtraData.md) for more info.

# Why Godot?
### Godot is a game engine, so why am I using it for this project?
Godot provides lots of useful features -- especially for ui. Godot is also very lightweight for a game engine which makes it a good choice for this project.

# Roadmap/TODO
## ~~v1.1.0~~ (Done)
- ~~Create system for mods to provide extra data in their descriptions~~
- ~~Allow use with the Core Keeper Dedicated Server~~
- ~~Add a button to toggle the options screen~~
- ~~Stop mods that have their security checks off from being loaded~~

## v1.3.0
- Filtering out world saves from the Mods page
- Support for seperate server and client Mods folder paths
- Load mod thumbnails as needed instead of all at the start increasing startup times
- Filtering mods by version
- Replace the Godot boot screen with a Core Launcher one

## Beyond
- Make the launcher provide info on what mods are elevated access
- Edit mod configs from the launcher
- Add a search bar to the mods list
- Create backups of your game from the launcher
- Allow the launcher to work with mods that are not on mod.io (e.g. mods that you haven't published yet)
- Swap mods faster by only installing/uninstalling the modmanifest.json file
- Custom themes
- Change the icon of the .exe file to Core Launcher's icon
- Automatic updates (to the launcher)
- Add an option to view mods in a grid instead of a list
