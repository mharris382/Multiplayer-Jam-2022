%%
#godot #gdscript #csharp #scripting
%%
seealso: [[Cross-Language Scripting]], [[Cross-Langauge Scripting - Fields]]

## [Calling methods](https://docs.godotengine.org/en/stable/tutorials/scripting/cross_language_scripting.html#calling-methods "Permalink to this headline")

### [Calling C# methods from GDScript](https://docs.godotengine.org/en/stable/tutorials/scripting/cross_language_scripting.html#calling-c-methods-from-gdscript "Permalink to this headline")

Again, calling C# methods from GDScript should be straightforward. The marshalling process will do its best to cast the arguments to match function signatures. If that's impossible, you'll see the following error: `` Invalid call. Nonexistent function `FunctionName` ``.

```python
my_csharp_node.PrintNodeName(self) # myGDScriptNode
# my_csharp_node.PrintNodeName() # This line will fail.

my_csharp_node.PrintNTimes("Hello there!", 2) # Hello there! Hello there!

my_csharp_node.PrintArray(["a", "b", "c"]) # a, b, c
my_csharp_node.PrintArray([1, 2, 3]) # 1, 2, 3
```

----

### [Calling GDScript methods from C#](https://docs.godotengine.org/en/stable/tutorials/scripting/cross_language_scripting.html#calling-gdscript-methods-from-c "Permalink to this headline")

To call GDScript methods from C# you'll need to use [Object.Call()](https://docs.godotengine.org/en/stable/classes/class_object.html#class-object-method-call). The first argument is the name of the method you want to call. The following arguments will be passed to said method.

```cs
myGDScriptNode.Call("print_node_name", this); // my_csharp_node
// myGDScriptNode.Call("print_node_name"); // This line will fail silently and won't error out.

myGDScriptNode.Call("print_n_times", "Hello there!", 2); // Hello there! Hello there!

// When dealing with functions taking a single array as arguments, we need to be careful.
// If we don't cast it into an object, the engine will treat each element of the array as a separate argument and the call will fail.
String[] arr = new String[] { "a", "b", "c" };
// myGDScriptNode.Call("print_array", arr); // This line will fail silently and won't error out.
myGDScriptNode.Call("print_array", (object)arr); // a, b, c
myGDScriptNode.Call("print_array", (object)new int[] { 1, 2, 3 }); // 1, 2, 3
// Note how the type of each array entry does not matter as long as it can be handled by t
```


