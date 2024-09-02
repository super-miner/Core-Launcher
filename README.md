![Core Launcher](Banner.png)

# Core Launcher
Core Launcher is a custom game launcher made for the game Core Keeper. It is made using Godot and supports Windows and Linux.
<p align="center">
  <a href="https://discord.com/channels/851842678340845600/1173510418690490458" target="_blank">
    Discord
  </a>
  •
  <a href="https://github.com/super-miner/Core-Launcher">
    GitHub
  </a>
  •
  <a href="https://github.com/super-miner/Core-Launcher/issues">
    Issues
  </a>
  •
  <a href="Info/UsingExtraData.md">
    Info for Mod Devs
  </a>
</p>

## Installation
To install Core Launcher download the latest full release from [here](https://github.com/super-miner/Core-Launcher/releases), extract the file, and run CoreLauncher.exe.

## Roadmap/TODO
### ~~v1.1.0~~ ([Released](https://github.com/super-miner/Core-Launcher/releases))
- ~~Create system for mods to provide extra data in their descriptions~~
- ~~Allow use with the Core Keeper Dedicated Server~~
- ~~Add a button to toggle the options screen~~
- ~~Stop mods that have their security checks off from being loaded~~

### v1.3.0
- Filtering out world saves from the Mods page
- Support for seperate server and client Mods folder paths
- Load mod thumbnails as needed instead of all at the start increasing startup times
- Filtering mods by version
- Replace the Godot boot screen with a Core Launcher one

### Beyond
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

## Info for Mod Developers (CL_Data)
From v1.1.0 onwards there is an option to link things like donation pagess so that they show up in the launcher. Please read [this](Info/UsingCL_Data.md) for more info.
