#gdscript 
# Enums[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#enums "Permalink to this headline")

Enums are basically a shorthand for constants, and are pretty useful if you want to assign consecutive integers to some constant.

If you pass a name to the enum, it will put all the keys inside a constant dictionary of that name.

Important

In Godot 3.1 and later, keys in a named enum are not registered as global constants. They should be accessed prefixed by the enum's name (`Name.KEY`); see an example below.

```python
enum {TILE_BRICK, TILE_FLOOR, TILE_SPIKE, TILE_TELEPORT}
# Is the same as:
const TILE_BRICK = 0
const TILE_FLOOR = 1
const TILE_SPIKE = 2
const TILE_TELEPORT = 3

enum State {STATE_IDLE, STATE_JUMP = 5, STATE_SHOOT}
# Is the same as:
const State = {STATE_IDLE = 0, STATE_JUMP = 5, STATE_SHOOT = 6}
# Access values with State.STATE_IDLE, etc.
```
