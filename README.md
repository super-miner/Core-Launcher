![Core Launcher](Banner.png)

# Core Launcher
![Screenshot of Core Launcher](https://github.com/user-attachments/assets/72a78cb0-9211-415a-912d-1e2388b4d9e1)

Core Launcher is a custom game launcher for the game <strong>Core Keeper</strong> that allows switching between multiple sets of mods known as mod profiles. It supports installing mods on both the Client and Server and runs on Windows and Linux.
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
  <a href="https://buymeacoffee.com/flown" target="_blank">
    Donate
  </a>
  •
  <a href="Info/UsingCL_Data.md">
    Info for Mod Devs
  </a>
</p>

## Installation
1. Downlaod the latest release from [here](https://github.com/super-miner/Core-Launcher/releases)
2. Extract the downloaded zip file
3. Run `CoreLauncher.exe`
4. Follow the setup instructions in the launcher

## Roadmap
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
