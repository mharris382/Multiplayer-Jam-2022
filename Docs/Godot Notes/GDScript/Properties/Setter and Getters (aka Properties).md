#gdscript 

---

# Setters/getters[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#setters-getters "Permalink to this headline")

It is often useful to know when a class' member variable changes for whatever reason. It may also be desired to encapsulate its access in some way.

For this, GDScript provides a _setter/getter_ syntax using the `setget` keyword. It is used directly after a variable definition:

```
var variable = value setget setterfunc, getterfunc
```

Whenever the value of `variable` is modified by an _external_ source (i.e. not from local usage in the class), the _setter_ function (`setterfunc` above) will be called. This happens _before_ the value is changed. The _setter_ must decide what to do with the new value. Vice versa, when `variable` is accessed, the _getter_ function (`getterfunc` above) must `return` the desired value. Below is an example:

```
var my_var setget my_var_set, my_var_get

func my_var_set(new_value):
    my_var = new_value

func my_var_get():
    return my_var # Getter must return a value.
```

## Read-Only or Write-Only 
Either of the _setter_ or _getter_ functions can be omitted:
```
# Only a setter.
var my_var = 5 setget my_var_set
# Only a getter (note the comma).
var my_var = 5 setget ,my_var_get
```
Setters and getters are useful when [[Exports|exporting variables]] to the editor in tool scripts or plugins, for validating input.

As said, _local_ access will _not_ trigger the setter and getter. Here is an illustration of this:

```
func _init():
    # Does not trigger setter/getter.
    my_integer = 5
    print(my_integer)

    # Does trigger setter/getter.
    self.my_integer = 5
    print(self.my_integer)
```