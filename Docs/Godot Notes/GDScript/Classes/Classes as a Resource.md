#### [[Classes]] as resources[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#classes-as-resources "Permalink to this headline")
Classes stored as files are treated as [resources](https://docs.godotengine.org/en/stable/classes/class_gdscript.html#class-gdscript). They must be loaded from disk to access them in other classes. This is done using either the `load` or `preload` functions (see below). Instancing of a loaded class resource is done by calling the `new` function on the class object:

```
# Load the class resource when calling load().
var MyClass = load("myclass.gd")

# Preload the class only once at compile time.
const MyClass = preload("myclass.gd")

func _init():
    var a = MyClass.new()
    a.some_function()
```