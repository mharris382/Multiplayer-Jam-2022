
### [[Classes |Class]] declaration[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_styleguide.html#class-declaration "Permalink to this headline")

If the code is meant to run in the editor, place the `tool` keyword on the first line of the script.

Follow with the class_name if necessary. You can turn a GDScript file into a global type in your project using this feature. For more information, see [GDScript basics](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#doc-gdscript).

Then, add the extends keyword if the class extends a built-in type.

Following that, you should have the class's optional docstring as comments. You can use that to explain the role of your class to your teammates, how it works, and how other developers should use it, for example.

```
class_name MyNode
extends Node
# A brief description of the class's role and functionality.
# Longer description.
```
