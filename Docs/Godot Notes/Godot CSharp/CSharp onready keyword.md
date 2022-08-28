#csharp #scripting 
## onready keyword[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_differences.html#onready-keyword "Permalink to this headline")

```cs
private Label _myLabel;

public override void _Ready()
{
    _myLabel = GetNode<Label>("MyLabel");
}
```