#gdscript 

---
# Inheritance[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#inheritance "Permalink to this headline")

A class (stored as a file) can inherit from:
-   A global class.
-   Another class file.    
-   An inner class inside another class file.
    
Multiple inheritance is not allowed.
Inheritance uses the `extends` keyword:

```
# Inherit/extend a globally available class.
extends SomeClass

# Inherit/extend a named class file.
extends "somefile.gd"

# Inherit/extend an inner class in another file.
extends "somefile.gd".SomeInnerClass
```

To check if a given instance inherits from a given class, the `is` keyword can be used:

```
# Cache the enemy class.
const Enemy = preload("enemy.gd")

# [...]

# Use 'is' to check inheritance.
if entity is Enemy:
    entity.apply_damage()
```

To call a function in a _parent class_ (i.e. one `extend`-ed in your current class), prepend `.` to the function name:

```
.base_func(args)
```
This is especially useful because functions in extending classes replace functions with the same name in their parent classes. If you still want to call them, you can prefix them with `.` (like the `super` keyword in other languages):

```
func some_func(x):
    .some_func(x) # Calls the same function on the parent class.
```