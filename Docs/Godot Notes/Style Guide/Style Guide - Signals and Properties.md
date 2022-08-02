### Signals and properties[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_styleguide.html#signals-and-properties "Permalink to this headline")

Write signal declarations, followed by properties, that is to say, member variables, after the docstring.

Enums should come after signals, as you can use them as export hints for other properties.

Then, write constants, exported variables, public, private, and onready variables, in that order.
```
signal spawn_player(position)

enum Jobs {KNIGHT, WIZARD, ROGUE, HEALER, SHAMAN}

const MAX_LIVES = 3

export(Jobs) var job = Jobs.KNIGHT
export var max_health = 50
export var attack = 5

var health = max_health setget set_health

var _speed = 300.0

onready var sword = get_node("Sword")
onready var gun = get_node("Gun")
```
Note

The GDScript compiler evaluates onready variables right before the `_ready` callback. You can use that to cache node dependencies, that is to say, to get child nodes in the scene that your class relies on. This is what the example above shows.