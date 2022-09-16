%%
#godot #unit-testing #plugins 
seealso: [[Godot Plugins]]
%%
# Unit Testing Plugins
- [Gut - Godot Unit Testing](https://github.com/bitwes/Gut)
- [GdUnit3](https://godotengine.org/asset-library/asset/867)

|        | C# Support | Gdscript Support | Highest Compatible Version |
| ------- | ---------- | ---------------- | -------------------------- |
| **GUT**     | No         | Yes              | v3.5 (v4.x is WIP)             |
| **GdUnit3** | Yes        | Yes              | v3.5                        | 

# [GUT](https://github.com/bitwes/Gut)
## Features
-   Godot 3.4.x, 3.5.x compatible.
-   [Simple install](https://github.com/bitwes/Gut/wiki/Install) via the Asset Library.
-   A plethora of [asserts and utility methods](https://github.com/bitwes/Gut/wiki/Asserts-and-Methods) to help make your tests simple and concise.
-   Support for [Inner Test Classes](https://github.com/bitwes/Gut/wiki/Inner-Test-Classes) to give your tests some extra context and maintainability.
-   Doubling: [Full](https://github.com/bitwes/Gut/wiki/Doubles) and [Partial](https://github.com/bitwes/Gut/wiki/Partial-Doubles), [Stubbing](https://github.com/bitwes/Gut/wiki/Stubbing), [Spies](https://github.com/bitwes/Gut/wiki/Spies)
-   Command Line Interface [(CLI)](https://github.com/bitwes/Gut/wiki/Command-Line)
-   [Parameterized Tests](https://github.com/bitwes/Gut/wiki/ParameterizedTests)
-   [Export results](https://github.com/bitwes/Gut/wiki/Export-Test-Results) in standard JUnit XML format.
-   [Distribute your tests](https://github.com/bitwes/Gut/wiki/Running-On-Devices) with your project and run them on any platform Godot supports.

More info can be found in the [wiki](https://github.com/bitwes/Gut/wiki).


# [GdUnit3](https://github.com/MikeSchulze/gdUnit3)
## What is GdUnit3
GdUnit3 is a framework for testing Gd-Scrips/C# and Scenes within the Godot editor. GdUnit3 is very useful for test-driven development and will help you get your code bug-free.

## Features
-   Fully embedded in the Godot editor
-   Run test-suite(s) by using the context menu on FileSystem, ScriptEditor or GdUnitInspector
-   Create tests directly from the ScriptEditor
-   Configurable template for the creation of a new test-suite
-   A spacious set of Asserts use to verify your code
-   Argument matchers to verify the behavior of a function call by a specified argument type.
-   Fluent syntax support
-   Test Fuzzing support
-   Mocking a class to simulate the implementation in which you define the output of the certain function
-   Spy on an instance to verify that a function has been called with certain parameters.
-   Mock or Spy on a Scene
-   Provides a scene runner to simulate interactions on a scene
    -   Simulate by Input events like mouse and/or keyboard
    -   Simulate scene processing by a certain number of frames
    -   Simulate scene processing by waiting for a specific signal
-   Update Notifier to install the latest version from GitHub
-   Command Line Tool
-   CI - Continuous Integration support
    -   generates HTML report
    -   generates JUnit report
-   With v2.0.0 C# testing support (beta)
-   Visual Studio Code extension