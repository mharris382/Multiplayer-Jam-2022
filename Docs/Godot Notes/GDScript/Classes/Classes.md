# Classes[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#classes "Permalink to this headline")
(see [[Class Constructor]], [[Classes as a Resource]], [[Inner Classes]])
(seealso [[Exports]], [[Functions]], [[Data]])

By default, all script files are unnamed classes. In this case, you can only reference them using the file's path, using either a relative or an absolute path. For example, if you name a script file `character.gd`:
```
# Inherit from 'Character.gd'.

extends "res://path/to/character.gd"

# Load character.gd and create a new node instance from it.

var Character = load("res://path/to/character.gd")
var character_node = Character.new()
```

### Registering named classes[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#registering-named-classes "Permalink to this headline")
(seealso [[Registering named classes]])

You can give your class a name to register it as a new type in Godot's editor. For that, you use the `class_name` keyword. You can optionally add a comma followed by a path to an image, to use it as an icon. Your class will then appear with its new icon in the editor:

```
# Item.gd

extends Node
class_name Item, "res://interface/icons/item.png"
```
will show up in editor like this
![](https://docs.godotengine.org/en/stable/_images/class_name_editor_register_example.png)

---

NOTE: If the script is located in the `res://addons/` directory, `class_name` will only cause the node to show up in the **Create New Node** dialog if the script is part of an _enabled_ editor plugin. See [Making plugins](https://docs.godotengine.org/en/stable/tutorials/plugins/editor/making_plugins.html#doc-making-plugins) for more information.

---


Here's a class file example:
```
# Saved as a file named 'character.gd'.

class_name Character

var health = 5

func print_health():
    print(health)

func print_this_script_three_times():
    print(get_script())
    print(ResourceLoader.load("res://character.gd"))
    print(Character)
```

