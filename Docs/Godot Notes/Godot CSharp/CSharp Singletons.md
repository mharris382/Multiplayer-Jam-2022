#csharp #scripting #godot #singleton
## Singletons[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_differences.html#singletons "Permalink to this headline")
Singletons are available as static classes rather than using the singleton pattern. This is to make code less verbose than it would be with an `Instance` property.

```cs
Input.IsActionPressed("ui_down");
`````

However, in some very rare cases this is not enough. For example, you may want to access a member from the base class `Godot.Object`, like `Connect`. For such use cases we provide a static property named `Singleton` that returns the singleton instance. The type of this instance is `Godot.Object`.

```cs
Input.Singleton.Connect("joy_connection_changed", this, nameof(Input_JoyConnectionChanged));
```

