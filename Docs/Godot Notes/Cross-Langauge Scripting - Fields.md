%%
#godot #gdscript #csharp #scripting
%%
seealso: [[Cross-Language Scripting]]
## [Accessing fields](https://docs.godotengine.org/en/stable/tutorials/scripting/cross_language_scripting.html#accessing-fields "Permalink to this headline")

### Accessing C# fields from GDScript[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/cross_language_scripting.html#accessing-c-fields-from-gdscript "Permalink to this headline")
Accessing C# fields from GDScript is straightforward, you shouldn't have anything to worry about.
```cs
print(my_csharp_node.str1) # bar
my_csharp_node.str1 = "BAR"
print(my_csharp_node.str1) # BAR

print(my_csharp_node.str2) # barbar
# my_csharp_node.str2 = "BARBAR" # This line will hang and crash
```
Note that it doesn't matter if the field is defined as a property or an attribute. However, trying to set a value on a property that does not define a setter will result in a crash.



### Accessing GDScript fields from C#[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/cross_language_scripting.html#accessing-gdscript-fields-from-c "Permalink to this headline")
As C# is statically typed, accessing GDScript from C# is a bit more convoluted, you will have to use [Object.Get()](https://docs.godotengine.org/en/stable/classes/class_object.html#id1) and [Object.Set()](https://docs.godotengine.org/en/stable/classes/class_object.html#id4). The first argument is the name of the field you want to access.
```cs
GD.Print(myGDScriptNode.Get("str1")); // foo
myGDScriptNode.Set("str1", "FOO");
GD.Print(myGDScriptNode.Get("str1")); // FOO

GD.Print(myGDScriptNode.Get("str2")); // foofoo
// myGDScriptNode.Set("str2", "FOOFOO"); // This line won't do anything
```
Keep in mind that when setting a field value you should only use types the GDScript side knows about. Essentially, you want to work with built-in types as described in [GDScript basics](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#doc-gdscript) or classes extending [Object](https://docs.godotengine.org/en/stable/classes/class_object.html#class-object).
