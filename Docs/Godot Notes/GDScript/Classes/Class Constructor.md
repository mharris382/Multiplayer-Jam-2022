#### [[Classes]] constructor[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#class-constructor "Permalink to this headline")

The class constructor, called on class instantiation, is named `_init`. As mentioned earlier, the constructors of parent classes are called automatically when inheriting a class. So, there is usually no need to call `._init()` explicitly.

Unlike the call of a regular function, like in the above example with `.some_func`, if the constructor from the inherited class takes arguments, they are passed like this:

```
func _init(args).(parent_args):
   pass
```


## Example
```
# State.gd (inherited class)
var entity = null
var message = null

func _init(e=null):
    entity = e

func enter(m):
    message = m

# Idle.gd (inheriting class)
extends "State.gd"

func _init(e=null, m=null).(e):
    # Do something with 'e'.
    message = m
```

There are a few things to keep in mind here:

1.  If the inherited class (`State.gd`) defines a `_init` constructor that takes arguments (`e` in this case), then the inheriting class (`Idle.gd`) _must_ define `_init` as well and pass appropriate parameters to `_init` from `State.gd`.
2.  `Idle.gd` can have a different number of arguments than the parent class `State.gd`.
3.  In the example above, `e` passed to the `State.gd` constructor is the same `e` passed in to `Idle.gd`.
4.  If `Idle.gd`'s `_init` constructor takes 0 arguments, it still needs to pass some value to the `State.gd` parent class, even if it does nothing. This brings us to the fact that you can pass literals in the base constructor as well, not just variables, e.g.:
    
```
    # Idle.gd
    
    func _init().(5):
        pass
```
