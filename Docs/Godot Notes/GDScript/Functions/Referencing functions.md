# Referencing functions[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#referencing-functions "Permalink to this headline")

Contrary to Python, functions are _not_ first-class objects in GDScript. This means they cannot be stored in variables, passed as an argument to another function or be returned from other functions. This is for performance reasons.

To reference a function by name at run-time, (e.g. to store it in a variable, or pass it to another function as an argument) one must use the `call` or `funcref` helpers:

```
# Call a function by name in one step.
my_node.call("my_function", args)

# Store a function reference.
var my_func = funcref(my_node, "my_function")
# Call stored function reference.
my_func.call_func(args)
```