#gdscript 

---
### Registering named classes[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#registering-named-classes "Permalink to this headline")

You can give your class a name to register it as a new type in Godot's editor. For that, you use the `class_name` keyword. You can optionally add a comma followed by a path to an image, to use it as an icon. Your class will then appear with its new icon in the editor:

# Item.gd

extends Node
class_name Item, "res://interface/icons/item.png"

![](https://docs.godotengine.org/en/stable/_images/class_name_editor_register_example.png)
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