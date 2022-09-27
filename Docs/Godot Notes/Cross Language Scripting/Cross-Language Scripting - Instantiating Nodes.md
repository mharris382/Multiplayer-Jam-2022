%%
#godot #gdscript #csharp #scripting
%%
seealso: [[Cross-Language Scripting]]
## Instantiating nodes[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/cross_language_scripting.html#instantiating-nodes "Permalink to this headline")

If you're not using nodes from the scene tree, you'll probably want to instantiate nodes directly from the code.
```ad-warning
collapse: open
When creating `.cs` scripts, you should always keep in mind that the class Godot will use is the one named like the `.cs` file itself. If that class does not exist in the file, you'll see the following error: ``Invalid call. Nonexistent function `new` in base``.

For example, MyCoolNode.cs should contain a class named MyCoolNode.

You also need to check your `.cs` file is referenced in the project's `.csproj` file. Otherwise, the same error will occur.

```


### Instantiating C# nodes from GDScript[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/cross_language_scripting.html#instantiating-c-nodes-from-gdscript "Permalink to this headline")

Using C# from GDScript doesn't need much work. Once loaded (see [Classes as resources](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html#doc-gdscript-classes-as-resources)), the script can be instantiated with [new()](https://docs.godotengine.org/en/stable/classes/class_csharpscript.html#class-csharpscript-method-new).
```cs
var my_csharp_script = load("res://path_to_cs_file.cs")
var my_csharp_node = my_csharp_script.new()
print(my_csharp_node.str2) # barbar
```

### Instantiating GDScript nodes from C#[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/cross_language_scripting.html#instantiating-gdscript-nodes-from-c "Permalink to this headline")

From the C# side, everything work the same way. Once loaded, the GDScript can be instantiated with [GDScript.New()](https://docs.godotengine.org/en/stable/classes/class_gdscript.html#class-gdscript-method-new).

```
GDScript MyGDScript = (GDScript) GD.Load("res://path_to_gd_file.gd");
Object myGDScriptNode = (Godot.Object) MyGDScript.New(); // This is a Godot.Object
```

Here we are using an [Object](https://docs.godotengine.org/en/stable/classes/class_object.html#class-object), but you can use type conversion like explained in [Type conversion and casting](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_features.html#doc-c-sharp-features-type-conversion-and-casting).

