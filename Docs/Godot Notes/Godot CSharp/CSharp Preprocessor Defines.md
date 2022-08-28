#csharp #scripting

## Preprocessor defines[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_features.html#preprocessor-defines "Permalink to this headline")

Godot has a set of defines that allow you to change your [[CSharp Language Support|C#]] code depending on the environment you are compiling to.


**Platform Specific code**
```cs
    public override void _Ready()
    {
#if GODOT_SERVER
        // Don't try to load meshes or anything, this is a server!
        LaunchServer();
#elif GODOT_32 || GODOT_MOBILE || GODOT_WEB
        // Use simple objects when running on less powerful systems.
        SpawnSimpleObjects();
#else
        SpawnComplexObjects();
#endif
    }
```

**Cross-platform library**
```cs
    public void MyPlatformPrinter()
    {
#if GODOT
        GD.Print("This is Godot.");
#elif UNITY_5_3_OR_NEWER
        print("This is Unity.");
#else
        throw new InvalidWorkflowException("Only Godot and Unity are supported.");
#endif
    }
```

### Full list of defines[¶](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_features.html#full-list-of-defines "Permalink to this headline")

-   `GODOT` is always defined for Godot projects.
-   One of `GODOT_64` or `GODOT_32` is defined depending on if the architecture is 64-bit or 32-bit.
-   One of `GODOT_X11`, `GODOT_WINDOWS`, `GODOT_OSX`, `GODOT_ANDROID`, `GODOT_IOS`, `GODOT_HTML5`, or `GODOT_SERVER` depending on the OS. These names may change in the future. These are created from the `get_name()` method of the [OS](https://docs.godotengine.org/en/stable/classes/class_os.html#class-os) singleton, but not every possible OS the method returns is an OS that Godot with Mono runs on.


When **exporting**, the following may also be defined depending on the export features:
-   One of `GODOT_PC`, `GODOT_MOBILE`, or `GODOT_WEB` depending on the platform type.
-   One of `GODOT_ARM64_V8A` or `GODOT_ARMEABI_V7A` on Android only depending on the architecture.
-   One of `GODOT_ARM64` or `GODOT_ARMV7` on iOS only depending on the architecture.
-   Any of `GODOT_S3TC`, `GODOT_ETC`, and `GODOT_ETC2` depending on the texture compression type.
-   Any custom features added in the export menu will be capitalized and prefixed: `foo` -> `GODOT_FOO`.