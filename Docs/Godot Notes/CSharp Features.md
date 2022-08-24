[Features][1]
--
Generic version of `$MySprite`
```cs
Sprite mySprite = GetNode<Sprite>("MySprite");
mySprite.SetFrame(0);
```

Generic null-safe version 
```cs
Sprite mySprite = GetNodeOrNull<Sprite>("MySprite");
// Only call SetFrame() if mySprite is not null
mySprite?.SetFrame(0);
```

[[CSharp Signals]]
[1]:https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_features.html

[[CSharp Preprocessor Defines]]