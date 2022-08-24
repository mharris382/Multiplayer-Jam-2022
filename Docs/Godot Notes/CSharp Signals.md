[CSharp Signals](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_features.html#c-signals)
==
[Full Example](https://docs.godotengine.org/en/stable/getting_started/step_by_step/scripting_languages.html#doc-scripting)
this will declare a signal which shows up in the signal inspector 

```cs
[Signal]
delegate void MySignal();

[Signal]
delegate void MySignalWithArguments(string foo, int bar);
```

Connecting Signals
==
These signals can then be connected either in the editor or from code with `Connect`. If you want to connect a signal in the editor, you need to (re)build the project assemblies to see the new signal. This build can be manually triggered by clicking the “Build” button at the top right corner of the editor window.

```cs
public void MyCallback()
{
    GD.Print("My callback!");
}

public void MyCallbackWithArguments(string foo, int bar)
{
    GD.Print("My callback with: ", foo, " and ", bar, "!");
}

public void SomeFunction()
{
    instance.Connect("MySignal", this, "MyCallback");
    instance.Connect(nameof(MySignalWithArguments), this, "MyCallbackWithArguments");
}
```

Emitting Signals
==
Emitting signals is done with the `EmitSignal` method.
```cs
public void SomeFunction()
{
    EmitSignal(nameof(MySignal));
    EmitSignal("MySignalWithArguments", "hello there", 28);
}
```


Binding additional parameters from CSharp
--
Signals support parameters and bound values of all the [built-in types](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/built-in-types-table) and Classes derived from [Godot.Object](https://docs.godotengine.org/en/stable/classes/class_object.html#class-object).
```cs
public int Value { get; private set; } = 0;

private void ModifyValue(int modifier)
{
    Value += modifier;
}

public void SomeFunction()
{
    var plusButton = (Button)GetNode("PlusButton");
    var minusButton = (Button)GetNode("MinusButton");

    plusButton.Connect("pressed", this, "ModifyValue", new Godot.Collections.Array { 1 });
    minusButton.Connect("pressed", this, "ModifyValue", new Godot.Collections.Array { -1 });
}
```

 any `Node` or `Reference` will be compatible automatically, but custom data objects will need to extend from Godot.Object or one of its subclasses. 
```cs
public class DataObject : Godot.Object
{
    public string Field1 { get; set; }
    public string Field2 { get; set; }
}
```