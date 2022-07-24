# Multiplayer-Jam-2022

Jam Project for the Summer 2022 [Multiplayer Game Jam](https://itch.io/jam/multiplayer)

# Table of Contents (WIP)


# Project Setup Instructions 

1. download the latest version of godot from [here](https://godotengine.org/download/windows).
Alternatively: you can [install it from Steam](https://store.steampowered.com/app/404790/Godot_Engine/) if you have a steam account
2. once you have godot installed, you need to clone this repository to a local path.  If you need help cloning the repo contact me for assistance.
3. open godot, and from the project browser select import.  Navigate to the directory where the repo is cloned, then open the folder `MP Jam Project`.  Select project.godot and choose open.  The Godot editor should open up the project.  Now you should be ready to go! 


# UI Menus Screens
 - [Main Menu](https://github.com/mharris382/Multiplayer-Jam-2022/blob/main/README.md#main-menu)
 - [Settings Menu](https://github.com/mharris382/Multiplayer-Jam-2022/blob/main/README.md#settings-menu)
 - [Pause Menu](https://github.com/mharris382/Multiplayer-Jam-2022/blob/main/README.md#pause-menu)

## Main Menu
- Buttons
  - New Game
  - Load Game (if on PC, disable this option for WebGL)
  - Settings
  - Quit Game

## Settings Menu
- fullscreen (toggle)
- master volume (slider)
- music volume (slider)
- sfx volume (slider)

## Pause Menu
- Buttons
  - Resume Game
  - Settings
  - Restart Level
  - Return to Level Selection Room
  - Quit Game

# Audio Overview
It is easy to implmenent audio settings if we use [godot's audio bus system](https://docs.godotengine.org/en/stable/tutorials/audio/audio_buses.html).  

- Master
  - SFX
  - Music
![](https://github.com/mharris382/Multiplayer-Jam-2022/blob/main/Docs/Images/AudioBusScreenshot.png)
