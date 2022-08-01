### onready keyword[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#onready-keyword "Permalink to this headline")

When using nodes, it's common to desire to keep references to parts of the scene in a variable. As scenes are only warranted to be configured when entering the active scene tree, the sub-nodes can only be obtained when a call to `Node._ready()` is made.

```
var my_label

func _ready():
    my_label = get_node("MyLabel")
```

This can get a little cumbersome, especially when nodes and external references pile up. For this, GDScript has the `onready` keyword, that defers initialization of a member variable until `_ready()` is called. It can replace the above code with a single line:
```
onready var my_label = get_node("MyLabel")
```