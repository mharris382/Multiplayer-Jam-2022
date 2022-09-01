#csharp #scripting #exporting #godot

Use the `[Export]` attribute instead of the [[GDScript]] `export` keyword. This attribute can also be provided with optional [PropertyHint](https://docs.godotengine.org/en/stable/classes/class_%40globalscope.html#enum-globalscope-propertyhint) and `hintString` parameters. Default values can be set by assigning a value.

```cs
using Godot;

public class MyNode : Node
{
    [Export]
    private NodePath _nodePath;

    [Export]
    private string _name = "default";

    [Export(PropertyHint.Range, "0,100000,1000,or_greater")]
    private int _income;

    [Export(PropertyHint.File, "*.png,*.jpg")]
    private string _icon;
}
```