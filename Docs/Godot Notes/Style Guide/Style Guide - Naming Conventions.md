## Naming conventions[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_styleguide.html#naming-conventions "Permalink to this headline")

These naming conventions follow the Godot Engine style. Breaking these will make your code clash with the built-in naming conventions, leading to inconsistent code.

### File names[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_styleguide.html#file-names "Permalink to this headline")

Use snake_case for file names. For named classes, convert the PascalCase class name to snake_case:

```
# This file should be saved as `weapon.gd`.
class_name Weapon
extends Node
```
`# This file should be saved as `yaml_parser.gd`.
class_name YAMLParser
extends Object`files are named in Godot's source code. This also avoids case sensitivity issues that can crop up when exporting a project from Windows to other platforms.

## [[Classes]] and [[Nodes]]
Use PascalCase for class and node names:
```
extends KinematicBody
```

Also use PascalCase when loading a class into a constant or a variable:
```
const Weapon = preload("res://weapon.gd")
```
### [[Functions]] and [[Data |variables]]

Use snake_case to name functions and variables:

```
var particle_effect
func load_level():
```

Prepend a single underscore (_) to virtual methods functions the user must override, private functions, and private variables:

```
var _counter = 0
func _recalculate_path():
```

## [[Signals]]

Use the past tense to name signals:

```
signal door_opened
signal score_changed
```

## [[Constants]] and [[Enums]]


Write constants with CONSTANT_CASE, that is to say in all caps with an underscore (_) to separate words:

```
const MAX_SPEED = 200
```
Use PascalCase for enum _names_ and CONSTANT_CASE for their members, as they are constants:

```
enum Element {
    EARTH,
    WATER,
    AIR,
    FIRE,
}
```


