#gdscript 

---
### Memory management[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#memory-management "Permalink to this headline")

If a class inherits from [Reference](https://docs.godotengine.org/en/stable/classes/class_reference.html#class-reference), then instances will be freed when no longer in use. No garbage collector exists, just reference counting. By default, all classes that don't define inheritance extend **Reference**. If this is not desired, then a class must inherit [Object](https://docs.godotengine.org/en/stable/classes/class_object.html#class-object) manually and must call `instance.free()`. To avoid reference cycles that can't be freed, a [WeakRef](https://docs.godotengine.org/en/stable/classes/class_weakref.html#class-weakref) function is provided for creating weak references. Here is an example:


```
extends Node

var my_node_ref

func _ready():
    my_node_ref = weakref(get_node("MyNode"))

func _this_is_called_later():
    var my_node = my_node_ref.get_ref()
    if my_node:
        my_node.do_something()
```
Alternatively, when not using references, the `is_instance_valid(instance)` can be used to check if an object has been freed.