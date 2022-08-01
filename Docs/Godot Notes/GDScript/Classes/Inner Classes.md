#### Inner classes[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#inner-classes "Permalink to this headline")

A class file can contain inner classes. Inner classes are defined using the `class` keyword. They are instanced using the `ClassName.new()` function.

```
# Inside a class file.

# An inner class in this class file.
class SomeInnerClass:
    var a = 5

    func print_value_of_a():
        print(a)

# This is the constructor of the class file's main class.
func _init():
    var c = SomeInnerClass.new()
    c.print_value_of_a()
```