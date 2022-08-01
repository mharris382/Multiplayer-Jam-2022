## Code order[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_styleguide.html#code-order "Permalink to this headline")
seealso [[Cheatsheet]]
This first section focuses on code order. For formatting, see [Formatting](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_styleguide.html#formatting). For naming conventions, see [Naming conventions](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_styleguide.html#naming-conventions).

We suggest to organize GDScript code this way:

01. tool
02. class_name
03. extends
04. \#docstring

05. [[signals]]
06. [[enums]]
07. [[constants]]
08. [[Exports |exported variables]]
09. public variables (see [[Data]])
10. private variables (see [[Data]])
11. [[onready Keyword |onready variables]]

12. optional built-in virtual _init method
13. built-in virtual _ready method
14. remaining built-in virtual methods
15. public methods (see [[Functions]])
16. private methods (see [[Functions]])

We optimized the order to make it easy to read the code from top to bottom, to help developers reading the code for the first time understand how it works, and to avoid errors linked to the order of variable declarations.

This code order follows four rules of thumb:

1.  Properties and signals come first, followed by methods. (see [[Properties]])
    
2.  Public comes before private. (see [[Functions]])
    
3.  Virtual callbacks come before the class's interface. (see [[Signals]], [[Coroutines and Signals]])
    
4.  The object's [[Class Constructor| construction]] and initialization functions, `_init` and `_ready`, come before functions that modify the object at runtime.
    
[[Style Guide - Signals and Properties]]
			
![[Cheatsheet]]