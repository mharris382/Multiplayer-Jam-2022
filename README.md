# Multiplayer-Jam-2022

Jam Project for the Summer 2022 [Multiplayer Game Jam](https://itch.io/jam/multiplayer)

# Table of Contents (WIP)
- [Project Setup Instructions](https://github.com/mharris382/Multiplayer-Jam-2022/blob/main/README.md#project-setup-instructions)
- [UI Menu Screens](https://github.com/mharris382/Multiplayer-Jam-2022/blob/main/README.md#ui-menus-screens)
- [Audio Overview](https://github.com/mharris382/Multiplayer-Jam-2022/blob/main/README.md#audio-overview)

# Project Setup Instructions 

1. download the latest version of godot from [here](https://godotengine.org/download/windows).
Alternatively: you can [install it from Steam](https://store.steampowered.com/app/404790/Godot_Engine/) if you have a steam account
2. once you have godot installed, you need to clone this repository to a local path.  If you need help cloning the repo contact me for assistance.
3. open godot, and from the project browser select import.  Navigate to the directory where the repo is cloned, then open the folder `MP Jam Project`.  Select project.godot and choose open.  The Godot editor should open up the project.  Now you should be ready to go! 


# Project Standards

For scripting we will follow [the official godot scripting style guide](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_styleguide.html). 
```
class_name StateMachine
extends Node
# Hierarchical State machine for the player.
# Initializes states and delegates engine callbacks
# (_physics_process, _unhandled_input) to the state.


signal state_changed(previous, new)

export var initial_state = NodePath()
var is_active = true setget set_is_active

onready var _state = get_node(initial_state) setget set_state
onready var _state_name = _state.name


func _init():
    add_to_group("state_machine")


func _ready():
    connect("state_changed", self, "_on_state_changed")
    _state.enter()


func _unhandled_input(event):
    _state.unhandled_input(event)


func _physics_process(delta):
    _state.physics_process(delta)


func transition_to(target_state_path, msg={}):
    if not has_node(target_state_path):
        return

    var target_state = get_node(target_state_path)
    assert(target_state.is_composite == false)

    _state.exit()
    self._state = target_state
    _state.enter(msg)
    Events.emit_signal("player_state_changed", _state.name)


func set_is_active(value):
    is_active = value
    set_physics_process(value)
    set_process_unhandled_input(value)
    set_block_signals(not value)


func set_state(value):
    _state = value
    _state_name = _state.name


func _on_state_changed(previous, new):
    print("state changed")
    emit_signal("state_changed")
```

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
