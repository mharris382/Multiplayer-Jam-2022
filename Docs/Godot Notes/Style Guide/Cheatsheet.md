%% #Style-Guide #example %%

```python
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
_*gdscript not python_