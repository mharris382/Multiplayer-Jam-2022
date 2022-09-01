#gdscript 

-----


https://docs.godotengine.org/en/stable/getting_started/step_by_step/signals.html

Signals are a tool to emit messages from an object that other objects can react to. To create custom signals for a class, use the `signal` keyword.

# Declare Signals
```
extends Node

# A signal named health_depleted.
signal health_depleted
```

# Connect Signals
You can connect these signals to methods the same way you connect built-in signals of nodes like [Button](https://docs.godotengine.org/en/stable/classes/class_button.html#class-button) or [RigidBody](https://docs.godotengine.org/en/stable/classes/class_rigidbody.html#class-rigidbody)

## Health Signal Example 1
In the example below, we connect the `health_depleted` signal from a `Character` node to a `Game` node. When the `Character` node emits the signal, the game node's `_on_Character_health_depleted` is called:
```
# Game.gd

func _ready():
    var character_node = get_node('Character')
    character_node.connect("health_depleted", self, "_on_Character_health_depleted")

func _on_Character_health_depleted():
    get_tree().reload_current_scene()
```

# Signal Arguments
You can emit as many arguments as you want along with a signal.

Here is an example where this is useful. Let's say we want a life bar on screen to react to health changes with an animation, but we want to keep the user interface separate from the player in our scene tree.

## Health Signal Example 2
```
# Character.gd

...
signal health_changed

func take_damage(amount):
    var old_health = health
    health -= amount

    # We emit the health_changed signal every time the
    # character takes damage.
    emit_signal("health_changed", old_health, health)
...
```

```
# Lifebar.gd

# Here, we define a function to use as a callback when the
# character's health_changed signal is emitted.

...
func _on_Character_health_changed(old_value, new_value):
    if old_value > new_value:
        progress_bar.modulate = Color.red
    else:
        progress_bar.modulate = Color.green

    # Imagine that `animate` is a user-defined function that animates the
    # bar filling up or emptying itself.
    progress_bar.animate(old_value, new_value)
...
```
In the `Game` node, we get both the `Character` and `Lifebar` nodes, then connect the character, that emits the signal, to the receiver, the `Lifebar` node in this case.

```
# Game.gd

func _ready():
    var character_node = get_node('Character')
    var lifebar_node = get_node('UserInterface/Lifebar')

    character_node.connect("health_changed", lifebar_node, "_on_Character_health_changed")
```
This allows the `Lifebar` to react to health changes without coupling it to the `Character` node.

# Naming Signal Arguments
You can write optional argument names in parentheses after the signal's definition:
```
# Defining a signal that forwards two arguments.
signal health_changed(old_value, new_value)
```
These arguments show up in the editor's node dock, and Godot can use them to generate callback functions for you. However, you can still emit any number of arguments when you emit signals; **it's up to you to emit the correct values.**


## Binding Signal Arguments
GDScript can bind an array of values to connections between a signal and a method. When the signal is emitted, the callback method receives the bound values. ***These bound arguments are unique to each connection***, and the values will stay the same.

You can use this array of values to add **extra constant information** to the connection if the emitted signal itself doesn't give you access to all the data that you need.

## Health Signal Example 3
Building on the example above, let's say we want to display a log of the damage taken by each character on the screen, like `Player1 took 22 damage.`. The `health_changed` signal doesn't give us the name of the character that took damage. So when we connect the signal to the in-game console, we can add the character's name in the binds array argument:

```
# Game.gd

func _ready():
    var character_node = get_node('Character')
    var battle_log_node = get_node('UserInterface/BattleLog')

    character_node.connect("health_changed", battle_log_node, "_on_Character_health_changed", [character_node.name])
```
Our `BattleLog` node receives each element in the binds array as an extra argument:

```
# BattleLog.gd

func _on_Character_health_changed(old_value, new_value, character_name):
    if not new_value <= old_value:
        return

    var damage = old_value - new_value
    label.text += character_name + " took " + str(damage) + " damage."
```


---


# Other Links
-  [SceneSetup](https://docs.godotengine.org/en/stable/getting_started/step_by_step/signals.html#scene-setup)
-  [Connect Signals in the editor](https://docs.godotengine.org/en/stable/getting_started/step_by_step/signals.html#connecting-a-signal-in-the-editor)
- [Connect Signals via code](https://docs.godotengine.org/en/stable/getting_started/step_by_step/signals.html#connecting-a-signal-via-code)
- [Custom Signals](https://docs.godotengine.org/en/stable/getting_started/step_by_step/signals.html#custom-signals)
