![Core Launcher](Banner.png)

# Core Launcher
![Screenshot of Core Launcher](https://github.com/user-attachments/assets/72a78cb0-9211-415a-912d-1e2388b4d9e1)

Core Launcher is a custom game launcher for the game <strong>Core Keeper</strong> that allows switching between multiple sets of mods known as mod profiles. It supports installing mods on both the Client and Server and runs on Windows and Linux.
<p align="center">
  <a href="https://discord.com/channels/851842678340845600/1173510418690490458" target="_blank">
    Discord
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
  •
  <a href="CONTRIBUTING.md">
    Contributing
  </a>
</p>

## Installation
1. Download the latest release from [here](https://github.com/super-miner/Core-Launcher/releases)
2. Extract the downloaded zip file
3. Run `CoreLauncher.exe`
4. Follow the setup instructions in the launcher

## Usage

1. Create a profile using the add button to start adding mods

![The add button near the top left of the window](https://github.com/user-attachments/assets/d0cfa8c4-4041-4dcf-889c-3d1e828a8263)
  
2. Use profiles to create sets of mods to switch between
3. Clicking the "PLAY" button will launch the game through Steam

![PlayButton](https://github.com/user-attachments/assets/ee66adf7-e2aa-4e2d-81c3-2a8a87f48bda)

4. Once mods are installed, you don't need to launch via Core Launcher
5. Dedicated Servers at the default Steam install location will appear in the list and can also be managed

> [!Note]
> There is currently no way to change the server or client location

## Roadmap

### v1.3.0 (expands upon v1.1.x features, not v1.2.x)
- Replace the Godot boot screen with a Core Launcher one
- Controller support (partial steam deck support)
- Display mode versions on the Mods page
- Automatically create a server and client profile when starting up the app for the first time
- Partial support for seperate server and client Mods folder paths by editing config files
- Xbox Gamepass support

### v1.4.0
- Filtering out world saves from the Mods page
- Load mod thumbnails as needed instead of all at the start increasing startup times
- Filtering mods by version
- Sorting mods in the mods menu
- Full support for seperate server and client Mods folder paths

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

## Contributing
You can find information on how to contribute to Core Launcher [here](CONTRIBUTING.md).
